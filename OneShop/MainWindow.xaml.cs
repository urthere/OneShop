﻿using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Linq;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StockViewModel stock;

        public MainWindow()
        {
            InitializeComponent();

            stock = new StockViewModel(App.DBConnectionString) { CurrentUser = "admin" };
            this.grdDetail.ItemsSource = stock.StockList;
            this.tbTotal.DataContext = stock;
        }

        private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var length = this.txtBarcode.Text.Trim().Length;
            if (0 != ConstantVariables.BarcodeLengthSet.FirstOrDefault(x => x == length))
            {
                if (!stock.GetStock(this.txtBarcode.Text.Trim()))
                {
                    MessageBox.Show("条码错误或商品库存不足！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtBarcode_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.-]+").IsMatch(e.Text);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (0 == this.grdDetail.Items.Count)
            {
                return;
            }
            var errMsg = string.Empty;
            
            stock.SaveOrder(out errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                MessageBox.Show(errMsg);
                return;
            }

            var receipt = new Receipt(this.stock);
            receipt.ShowDialog();
            stock = new StockViewModel(App.DBConnectionString);
        }        

        private void PrintReceipt()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new NewStock();
            wnd.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = this.grdDetail.SelectedItem as StockListModel;
            if (null != item)
            {
                this.stock.StockList.Remove(item);
                this.stock.ReCalculate();
            }
        }
    }
}

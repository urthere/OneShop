using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OneShop.Model;
using System.Text.RegularExpressions;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : UserControl
    {
        private StockViewModel stock;
        public Sales()
        {
            InitializeComponent();

            stock = new StockViewModel(App.DBConnectionString) { CurrentUser = "admin" };
            this.grdDetail.ItemsSource = stock.StockList;
            this.tbTotal.DataContext = stock;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (0 == this.grdDetail.Items.Count)
            {
                return;
            }
            var errMsg = string.Empty;

            stock.SaveOrder(out errMsg, GetPayMethod());

            if (!string.IsNullOrEmpty(errMsg))
            {
                MessageBox.Show(errMsg);
                return;
            }

            var receipt = new Receipt(this.stock);
            receipt.Height = Properties.Settings.Default.PrintingWindowHeight;
            receipt.ShowDialog();
            this.stock.OrderPrice = 0;
            this.stock.ClearAll();
        }

        private void txtBarcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.-]+").IsMatch(e.Text);
        }

        private void txtBarcode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!stock.GetStock(this.txtBarcode.Text.Trim()))
                {
                    MessageBox.Show("条码错误或商品库存不足！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.txtBarcode.SelectAll();
                }
                else
                {
                    this.txtBarcode.Text = string.Empty;
                }

                e.Handled = true;
            }
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

        private string GetPayMethod()
        {

            if (rdbAlipay.IsChecked == true)
            {
                return "支付宝";
            }
            if (rdbWechatPay.IsChecked == true)
            {
                return "微信";
            }
            if (rdbCash.IsChecked == true)
            {
                return "现金";
            }
            return string.Empty;
        }
    }
}

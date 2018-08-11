using System.Windows;
using System.Linq;
using System.Windows.Data;
using OneShop.Model;
using System.Collections;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for NewStock.xaml
    /// </summary>
    public partial class NewStock : Window
    {
        private StockNewViewModel stockNewViewModel;
        public NewStock()
        {
            InitializeComponent();

            stockNewViewModel = new StockNewViewModel(App.DBConnectionString, null);
            this.dgStocks.ItemsSource = stockNewViewModel.Stocks;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var barcode = this.txtBarcode.Text.Trim();
            if (null != stockNewViewModel.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(barcode)))
            {
                var stock = this.gbStock.DataContext as Stock;
                stockNewViewModel.EditStock(stock);
            }
            else
            {
                NewItem();
            }
            this.gbStock.DataContext = null;
            ClearTxtBox();
        }

        private void NewItem()
        {            
            var stock = new Stock();

            stock.ItemBarcode = this.txtBarcode.Text.Trim();
            stock.ItemName = this.txtName.Text.Trim();
            if (decimal.TryParse(this.txtUnitPrice.Text.Trim(), out decimal i))
            {
                stock.ItemPrice = i;
            }
            if (int.TryParse(this.txtCount.Text.Trim(), out int n))
            {
                stock.ItemCount = n;
            }
            stock.StoredBy = "admin";
            stock.ModBy = stock.StoredBy;
            stock.StoredDate = System.DateTime.Now;
            stock.ModDate = System.DateTime.Now;
            stockNewViewModel.AddStocks(stock);
        }

        private void ClearTxtBox()
        {
            this.txtAmount.Text = string.Empty;
            this.txtBarcode.Text = string.Empty;
            this.txtCount.Text = string.Empty;
            this.txtName.Text = string.Empty;
            this.txtUnitPrice.Text = string.Empty;            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stockNewViewModel.MutilSave();
        }

        private void dgStocks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = this.dgStocks.SelectedItem as Stock;
            if (null != item)
            {
                this.gbStock.DataContext = item;
            }
        }
    }
}

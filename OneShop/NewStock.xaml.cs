using System.Windows;
using System.Linq;
using System.Windows.Data;
using OneShop.Model;
using System.Collections;
using System.Windows.Input;

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
            this.gbStock.DataContext = this.stockNewViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }       

        private void ClearTxtBox()
        {
            //this.txtAmount.Text = string.Empty;
            this.txtBarcode.Text = string.Empty;
            this.txtCount.Text = string.Empty;
            this.txtName.Text = string.Empty;
            this.txtUnitPrice.Text = string.Empty;            
        }
       
        private void dgStocks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = this.dgStocks.SelectedItem as Stock;
            if (null != item)
            {
                this.stockNewViewModel.SetStockInfo(item);                
            }
        }

        private void txtBarcode_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.stockNewViewModel.GetStock(this.txtBarcode.Text.Trim());
            }    
        }
    }
}

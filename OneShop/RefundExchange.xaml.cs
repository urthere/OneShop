using System.Windows;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for RefundExchange.xaml
    /// </summary>
    public partial class RefundExchange : Window
    {
        public RefundExchange()
        {
            InitializeComponent();

            this.DataContext = new StockViewModel(App.DBConnectionString) { CurrentUser = "admin" };
        }

        private void txtBarcode_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtBarcode.MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next));
            }
            e.Handled = true;
        }
    }
}

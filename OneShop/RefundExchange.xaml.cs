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

            this.DataContext = new OrderViewModel(App.DBConnectionString);
        }
    }
}

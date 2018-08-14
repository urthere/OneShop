using System.Windows;
using System.Windows.Controls;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for StockQuery.xaml
    /// </summary>
    public partial class StockQuery : Window
    {
        private StockNewViewModel stockNewViewModel;
        public StockQuery()
        {
            InitializeComponent();

            stockNewViewModel = new StockNewViewModel(App.DBConnectionString);
            this.DataContext = stockNewViewModel;
        }
    }
}

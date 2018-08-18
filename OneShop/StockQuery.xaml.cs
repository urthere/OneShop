using System.Windows.Controls;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for StockQuery.xaml
    /// </summary>
    public partial class StockQuery : UserControl
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

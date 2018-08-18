using System.Windows.Controls;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for OrderQuery.xaml
    /// </summary>
    public partial class OrderQuery : UserControl
    {
        public OrderQuery()
        {
            InitializeComponent();

            this.DataContext = new OrderViewModel(App.DBConnectionString);
        }
    }
}

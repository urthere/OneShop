using System.Windows;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for OrderQuery.xaml
    /// </summary>
    public partial class OrderQuery : Window
    {
        public OrderQuery()
        {
            InitializeComponent();

            this.DataContext = new OrderViewModel(App.DBConnectionString);
        }
    }
}

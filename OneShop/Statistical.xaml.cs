using System.Windows.Controls;
using OneShop.Model;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for Statistical.xaml
    /// </summary>
    public partial class Statistical : UserControl
    {
        public Statistical()
        {
            InitializeComponent();

            this.DataContext = new OrderViewModel(App.DBConnectionString);
        }
    }
}

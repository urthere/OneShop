using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneShop.Model
{
    public class DiscountMaintainanceViewModel : NotifyBase
    {
        private string conn;
        private ObservableCollection<Discount> discounts;
        private Discount discount;

        public DiscountMaintainanceViewModel(string con)
        {
            conn = con;
            discounts = new ObservableCollection<Discount>();
            discount = new Discount();
        }

        public ICommand QueryListCommand { get; set; }
        public ICommand QueryByKeyCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeteteCommand { get; set; }

        public Discount Discount => discount;
    }
}

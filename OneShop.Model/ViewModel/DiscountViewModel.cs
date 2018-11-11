using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OneShop.Model
{
    public class DiscountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IList<Discount> discounts;

        public DiscountViewModel(string con)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = con;

                try
                {
                    discounts = context.Discounts.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public IList<Discount> Discounts => discounts;
    }
}

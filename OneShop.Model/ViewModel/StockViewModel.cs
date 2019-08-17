using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OneShop.Model
{
    public class StockListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private decimal detailPrice;
        private decimal discount = 0;
        private Stock stock;
        private DiscountViewModel discountViewModel;
        private int itemSoldCount = 1;

        public StockListModel(string barcode, string con)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = con;
                try
                {
                    this.stock = context.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(barcode));
                    this.discountViewModel = new DiscountViewModel(con);                    
                }
                catch (System.Data.Entity.Core.MetadataException ex)
                {
                    throw;
                }
                
            }
        }

        public IList<Discount> DiscountList
        {
            get => this.discountViewModel.Discounts;
            set
            {
                var dis = (Discount)value;
                this.discount = dis.DiscountPercent;
                CalculateDetailPrice();
            }
        }


        public Stock Stock => this.stock;

        public Action CalAct { get; set; }

        public decimal Discount
        {
            get => this.discount;
            set
            {
                if (decimal.TryParse(value.ToString(), out decimal i))
                {
                    this.discount = this.DiscountList[(int)i].DiscountPercent / 100;
                    CalculateDetailPrice();
                }
            }
        }

        private decimal RegDiscount(decimal i)
        {
            return i == 0 ? 1 : i;
        }

        private void CalculateDetailPrice()
        {
            this.detailPrice = Math.Round(itemSoldCount * RegDiscount(this.discount) * (decimal)stock.ItemPrice, MidpointRounding.AwayFromZero);            
            RaisePropertyChanged("DetailPrice");
        }

        public decimal DetailPrice
        {
            get
            {
                if (0 == detailPrice)
                {
                    this.detailPrice = itemSoldCount * RegDiscount(this.discount) * (decimal)stock.ItemPrice;
                    this.detailPrice = Math.Round(this.detailPrice, MidpointRounding.AwayFromZero);
                    RaisePropertyChanged("DetailPrice");
                }
                return this.detailPrice;
            }
        }
        public string ItemBarcode
        {
            get => stock.ItemBarcode;
        }
        public string ItemName { get => stock.ItemName; set => stock.ItemName = value; }       

        public int? ItemCount
        {
            get => this.itemSoldCount;
            set
            {
                if (int.TryParse(value.ToString(), out int i))
                {
                    itemSoldCount = i;
                    CalculateDetailPrice();
                }
            }
        }
        public decimal? ItemPrice { get => stock.ItemPrice; }
        public string StoredBy { get; }
        public DateTime StoredDate { get;  }
        public string ModBy { get;  }
        public DateTime ModDate { get;  }

        private void RaisePropertyChanged(string property)
        {
            CalAct?.Invoke();
            var PropertyChange = PropertyChanged;
            if (null != PropertyChange)
            {
                PropertyChange.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

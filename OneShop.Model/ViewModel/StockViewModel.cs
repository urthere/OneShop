using System;
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
        private int itemSoldCount = 1;

        public StockListModel(string barcode, string con)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = con;
                try
                {
                    this.stock = context.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(barcode) && x.ItemCount > 0);
                }
                catch (System.Data.Entity.Core.MetadataException ex)
                {
                    throw;
                }
                
            }
        }

        public Stock Stock
        {
            get => this.stock;
        }

        public Action CalAct { get; set; }

        public decimal Discount
        {
            get => this.discount;
            set
            {
                if (decimal.TryParse(value.ToString(), out decimal i))
                {
                    this.discount = i;
                    CalculateDetailPrice();
                }
            }
        }

        private decimal RegDiscount(decimal i)
        {
            switch (i)
            {
                case 0: return 1;
                case 1: return 0.98m;
                case 2:
                default:
                    return 0.95m;
            }
        }

        private void CalculateDetailPrice()
        {
            this.detailPrice = itemSoldCount * RegDiscount(this.discount) * (decimal)stock.ItemPrice;
            RaisePropertyChanged("DetailPrice");
        }

        public decimal DetailPrice
        {
            get
            {
                if (0 == detailPrice)
                {
                    this.detailPrice = itemSoldCount * RegDiscount(this.discount) * (decimal)stock.ItemPrice;
                    this.detailPrice = Math.Round(this.detailPrice, 2, MidpointRounding.AwayFromZero);
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
            get => itemSoldCount;
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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace OneShop.Model
{
    public class StockNewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OneShopEntities entity;
        private Stock Stock;
        private ObservableCollection<Stock> stocks;

        public StockNewViewModel(string conStr, string barcode = "")
        {
            entity = new OneShopEntities();
            entity.Database.Connection.ConnectionString = conStr;
            Stock = string.IsNullOrEmpty(barcode) ? new Stock() : this.GetStock(barcode);
            stocks = new ObservableCollection<Stock>(entity.Stocks.ToList());
        }
        
        public ICommand AddStockListCommand { get; set; }
        public ICommand SaveStockListCommand { get; set; }
        public ICommand SaveStockCommand { get; set; }
        
        public ObservableCollection<Stock> Stocks
        {
            get => this.stocks;
        }

        public string Barcode
        {
            get => Stock.ItemBarcode;
            set => Stock.ItemBarcode = value;
        }

        public string Name
        {
            get => Stock.ItemName;
            set => Stock.ItemName = value;
        }

        public decimal UnitPrice
        {
            get => (decimal)Stock.ItemPrice;
            set => Stock.ItemPrice = value;
        }

        public int Count
        {
            get => (int)Stock.ItemCount;
            set => Stock.ItemCount = value;
        }

        public decimal Amount
        {
            get => (decimal)Stock.ItemCount * (decimal)Stock.ItemPrice;
        }

        public int Save()
        {
            var sql = $"if exists(select barcode from stock where itembarcode='{Stock.ItemBarcode}'" +
                $"update stock set " +
                $"itemname='{Stock.ItemName}'," +
                $"itemcount={Stock.ItemCount}," +
                $"itemprice={Stock.ItemPrice}," +
                $"modby='{Stock.ModBy}'," +
                $"moddate=getdate()" +
                $"where itembarcode='{Stock.ItemBarcode}'" +
                $"else" +
                $"insert into stock value(" +
                $"'{Stock.ItemBarcode}'," +
                $"'{Stock.ItemName}'," +
                $"{Stock.ItemCount.ToString()}," +
                $"{Stock.ItemPrice.ToString()}" +
                $"'{Stock.StoredBy}'," +
                $"getdate()," +
                $"'{Stock.ModBy}'," +
                $"getdate())";
            return entity.Database.ExecuteSqlCommand(sql);
        }

        public int MutilSave()
        {            
            try
            {
                return entity.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                return -1;
            }
        }

        public Stock GetStock(string barcode)
        {
            return entity.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(barcode));
        }

        public void AddStocks(Stock stock)
        {
            this.stocks.Add(stock);
            entity.Stocks.Add(stock);
        }

        public void EditStock(Stock stock)
        {
            entity.Entry(stock).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

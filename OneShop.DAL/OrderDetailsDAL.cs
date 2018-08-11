using System.Collections.Generic;
using System.Linq;
using OneShop.Model;

namespace OneShop.DAL
{
    public class OrderDetailsDAL
    {
        private static OneShopEntities shopEntity;

        public OrderDetailsDAL(string connectionString)
        {
            shopEntity = new OneShopEntities();
            shopEntity.Database.Connection.ConnectionString = connectionString;
        }

        public Stock GetItem(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                return shopEntity.Stocks.FirstOrDefault(x => barcode.Equals(x.ItemBarcode));
            }
            return null;
        }

        public int SaveNewStock(IList<Stock> stocks)
        {
            foreach (var item in stocks)
            {
                shopEntity.Stocks.Add(item);
            }
            return shopEntity.SaveChanges();
        }
    }
}

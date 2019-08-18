using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneShop.Model
{
    public class StockViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string ConnectionString;
        private Order objOrder;
        private IList<OrderDetail> objOrderDetail;

        public StockViewModel(string conStr)
        {
            this.ConnectionString = conStr;

            objOrder = new Order() { IsValid = true };
            objOrderDetail = new List<OrderDetail>();
            StockList = new ObservableCollection<StockListModel>();
        }

        public void ClearAll()
        {
            this.objOrder = new Order() { IsValid = true, OrderPrice = 0 };
            this.objOrderDetail.Clear();
            this.StockList.Clear();
            
        }

        public void CancelStock(StockListModel model)
        {
            this.StockList.Remove(model);
        }

        private void PrepareOrderDetail()
        {
            foreach (var item in StockList)
            {
                objOrderDetail.Add(new OrderDetail()
                {
                    ItemBarcode = item.ItemBarcode,
                    ItemCount = (int)item.ItemCount,
                    DetailPrice = item.DetailPrice,
                    IsValid = true,
                    DatailDate = DateTime.Now,
                    UnitPrice = (decimal)item.ItemPrice,
                    Discount = item.Discount,
                    ActualPrice = item.DetailSalePrice
                });
            }
        }

        public void SaveOrder(out string stockNotEnough, string payMethod)
        {
            stockNotEnough = string.Empty;

            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = this.ConnectionString;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        PrepareOrderDetail();
                        this.objOrder.SerialNumber = this.GenerateSerialNumber();
                        this.objOrder.OrderDate = DateTime.Now;
                        this.objOrder.ModBy = DateTime.Now;
                        this.objOrder.Remarks = payMethod;

                        context.Orders.Add(this.objOrder);
                        context.SaveChanges();

                        foreach (var item in objOrderDetail)
                        {
                            item.OrderID = this.objOrder.OrderID;
                            item.IsValid = 0 > item.ItemCount ? false : true;
                            context.OrderDetails.Add(item);
                            var stockItem = context.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(item.ItemBarcode));
                            stockItem.ItemCount -= item.ItemCount;
                            if (stockItem.ItemCount < 0)
                            {
                                stockNotEnough = $"[{stockItem.ItemName}] 库存不足";
                                this.objOrderDetail.Clear();
                                transaction.Rollback();
                                return;
                            }
                            context.Set<Stock>().Attach(stockItem);
                            context.Entry(stockItem).State = System.Data.Entity.EntityState.Modified;
                        }
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        private string GenerateSerialNumber()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }

        public bool GetStock(string barcode)
        {
            var stockView = new StockListModel(barcode, this.ConnectionString) { CalAct = this.ReCalculate};
            if (null == stockView.Stock)
            {
                return false;
            }
            this.StockList.Add(stockView);
            ReCalculate();
            return true;
        }

        public decimal OrderPrice
        {
            get => objOrder.OrderPrice;
            set
            {
                objOrder.OrderPrice = value;
                RaisePropertyChanged("OrderPrice");
            }
        }

        public string SerialNumber { get => this.objOrder.SerialNumber; }

        public void ReCalculate()
        {
            objOrder.OrderPrice = 0.0m;
            foreach (var item in StockList)
            {
                objOrder.OrderPrice += item.DetailSalePrice;
            }

            this.RaisePropertyChanged("OrderPrice");
        }

        public IList<StockListModel> StockList { get; }
        public string CurrentUser { get; set; }

        private void RaisePropertyChanged(string name)
        {
            var handler = this.PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

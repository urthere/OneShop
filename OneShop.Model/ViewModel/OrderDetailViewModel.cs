﻿using System;
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
        private IList<StockListModel> stockList;
        private string currentUser;

        public StockViewModel(string conStr)
        {
            this.ConnectionString = conStr;

            objOrder = new Order() { IsValid = true, OrderDate = DateTime.Now, ModBy = DateTime.Now };
            objOrderDetail = new List<OrderDetail>();
            stockList = new ObservableCollection<StockListModel>();
        }

        public void CancelStock(StockListModel model)
        {
            this.stockList.Remove(model);
        }

        private void PrepareOrderDetail()
        {
            foreach (var item in stockList)
            {
                objOrderDetail.Add(new OrderDetail()
                {
                    ItemBarcode = item.ItemBarcode,
                    ItemCount = (int)item.ItemCount,
                    DetailPrice = (int)item.ItemCount * (decimal)item.ItemPrice,
                    IsValid = true,
                    DatailDate = DateTime.Now,
                    UnitPrice = (decimal)item.ItemPrice,
                    Discount=item.Discount
                });
            }
        }

        public void SaveOrder(out string stockNotEnough)
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
                        context.Orders.Add(this.objOrder);
                        context.SaveChanges();

                        foreach (var item in objOrderDetail)
                        {
                            item.OrderID = this.objOrder.OrderID;
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

        public bool GetStock(string barcode)
        {
            var stockView = new StockListModel(barcode, this.ConnectionString) { CalAct = this.ReCalculate};
            if (null == stockView.Stock)
            {
                return false;
            }
            this.stockList.Add(stockView);
            ReCalculate();
            return true;
        }

        public decimal OrderPrice
        {
            get => objOrder.OrderPrice;
            set => objOrder.OrderPrice = value;
        }

        public void ReCalculate()
        {
            objOrder.OrderPrice = 0.0m;
            foreach (var item in stockList)
            {
                objOrder.OrderPrice += item.DetailPrice;
            }

            this.RaisePropertyChanged("OrderPrice");
        }
                   
        public IList<StockListModel> StockList { get => stockList;}
        public string CurrentUser { get => currentUser; set => currentUser = value; }

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

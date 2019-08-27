using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Data.Entity;

namespace OneShop.Model
{
    public class OrderViewModel : NotifyBase
    {
        private Order order = new Order();
        private ObservableCollection<Order> orders;
        private string conn;

        public OrderViewModel(string con)
        {
            this.orders = new ObservableCollection<Order>();
            OrderDetailsNameModels = new List<OrderDetailsNameModel>();
            conn = con;
            QueryDetailsCommand = new DelegateCommand(this.GetOrderDetails, this.IsValid);
            QueryOrderCommand = new DelegateCommand(this.GetOrder, this.IsValid);
            UpdateRefundCommand = new DelegateCommand(this.RefundDetail, this.CanRefund);
            GetDailySummaryReport = new DelegateCommand(this.GetDailySummary, this.IsValid);
            QueryOrderSumCommand = new DelegateCommand(this.GetOrderSum, IsValid);
        }

        public ICommand QueryDetailsCommand { get; set; }
        public ICommand QueryOrderCommand { get; set; }
        public ICommand UpdateRefundCommand { get; set; }
        public ICommand QueryOrderSumCommand { get; set; }
        public ICommand GetDailySummaryReport { get; set; }

        private void GetOrder(object para)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = this.conn;
                OrderDetailsNameModels = Common<OrderDetailsNameModel>.QueryJoin(context, para.ToString());                
            }
            RaisePropertyChanged("OrderDetailsNameModels");
            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("TotalPriceWechat");
            RaisePropertyChanged("TotalPriceAlipay");
            RaisePropertyChanged("TotalPriceCash");
        }

        private void GetOrderSum(object para)
        {
            var dates = para.ToString().Split('|');
            var startDate = DateTime.Parse(dates[0]).Date;
            var endDate = DateTime.Parse(dates[1]).Date.AddDays(1);
            try
            {
                using (var context = new OneShopEntities())
                {
                    context.Database.Connection.ConnectionString = this.conn;
                    OrderDetailsNameModels = Common<OrderDetailsNameModel>.QuerySumJoin(context, startDate, endDate);
                }
                RaisePropertyChanged("OrderDetailsNameModels");
                RaisePropertyChanged("TotalPrice");
                RaisePropertyChanged("TotalActualPrice");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void GetOrderDetails(object para)
        {
            var dates = para.ToString().Split('|');
            var startDate = DateTime.Parse(dates[0]).Date;
            var endDate = DateTime.Parse(dates[1]).Date.AddDays(1);
            try
            {
                using (var context = new OneShopEntities())
                {
                    context.Database.Connection.ConnectionString = this.conn;
                    OrderDetailsNameModels = Common<OrderDetailsNameModel>.QueryJoin(context, startDate, endDate);
                }
                RaisePropertyChanged("OrderDetailsNameModels");
                RaisePropertyChanged("TotalPrice");
                RaisePropertyChanged("TotalActualPrice");
                RaisePropertyChanged("TotalPriceWechat");
                RaisePropertyChanged("TotalPriceAlipay");
                RaisePropertyChanged("TotalPriceCash");
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void GetDailySummary(object para)
        {
            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            try
            {
                using (var context = new OneShopEntities())
                {
                    context.Database.Connection.ConnectionString = this.conn;

                    var sum = context.OrderDetails
                        .GroupBy(x => DbFunctions.TruncateTime(x.DatailDate))
                        .Select(f => new { SumDate = f.Key, Rows = f.Count(), DailySum = f.Sum(w => w.DetailPrice) })
                        .Where(w => w.SumDate > firstDay && w.SumDate < DateTime.Now);
                    var list = sum.ToList();
                    this.DailyMax = list.Max(x => x.DailySum) + 500m;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public decimal Days
        {
            get => DateTime.Now.Day;
        }

        public decimal DailyMax { get; private set; }

        private void RefundDetail(object para)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = this.conn;
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        var details = context.OrderDetails.FirstOrDefault(x => x.DetailID == (int)para);
                        details.IsValid = false;
                        details.DatailDate = DateTime.Now;
                        var order = context.Orders.FirstOrDefault(x => x.OrderID == details.OrderID);
                        order.OrderPrice -= details.DetailPrice;
                        var stock = context.Stocks.FirstOrDefault(x => x.ItemBarcode.Equals(details.ItemBarcode));
                        stock.ItemCount += details.ItemCount;
                        context.Orders.Attach(order);
                        context.OrderDetails.Attach(details);
                        context.Stocks.Attach(stock);
                        context.Entry(order).State = EntityState.Modified;
                        context.Entry(details).State = EntityState.Modified;
                        context.Entry(stock).State = EntityState.Modified;
                        context.SaveChanges();

                        tran.Commit();

                        this.GetOrder(order.SerialNumber);
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public int TotalCount { get; set; }

        public IList<OrderDetailsNameModel> OrderDetailsNameModels { get; private set; }

        public decimal TotalPrice
        {
            get
            {
                if (this.OrderDetailsNameModels.Count > 0)
                {
                    return this.OrderDetailsNameModels.Sum(x => x.DetailPrice);
                }
                return 0;
            }
        }

        public decimal TotalActualPrice
        {
            get
            {
                if (this.OrderDetailsNameModels.Count > 0)
                {
                    return this.OrderDetailsNameModels.Sum(x => (decimal)x.ActualPrice);
                }
                return 0;
            }
        }

        public decimal TotalPriceWechat
        {
            get
            {
                if (this.OrderDetailsNameModels.Count > 0)
                {
                    return this.OrderDetailsNameModels.Where(w => null != w.Remarks && w.Remarks.Contains("微信")).ToList().Sum(x => (decimal)x.ActualPrice);
                }
                return 0;
            }
        }

        public decimal TotalPriceAlipay
        {
            get
            {
                if (this.OrderDetailsNameModels.Count > 0)
                {
                    return this.OrderDetailsNameModels.Where(w => null != w.Remarks && w.Remarks.Contains("支付宝")).ToList().Sum(x => (decimal)x.ActualPrice);
                }
                return 0;
            }
        }

        public decimal TotalPriceCash
        {
            get
            {
                if (this.OrderDetailsNameModels.Count > 0)
                {
                    return this.OrderDetailsNameModels.Where(w => null!= w.Remarks && w.Remarks.Contains("现金")).ToList().Sum(x => (decimal)x.ActualPrice);
                }
                return 0;
            }
        }


        private bool IsValid(object para)
        {
            return true;
        }

        private bool CanRefund(object para)
        {
            return true;
        }
    }
}

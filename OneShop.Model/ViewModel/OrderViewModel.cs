using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Windows.Input;

namespace OneShop.Model
{
    public class OrderViewModel : NotifyBase
    {
        private Order order = new Order();
        private ObservableCollection<Order> orders;
        private IList<OrderDetailsNameModel> orderDetailsNameModel;
        private string conn;
        private int totalCount;

        public OrderViewModel(string con)
        {
            this.orders = new ObservableCollection<Order>();
            orderDetailsNameModel = new List<OrderDetailsNameModel>();
            conn = con;
            QueryDetailsCommand = new DelegateCommand(this.GetOrderDetails, this.IsValid);
            QueryOrderCommand = new DelegateCommand(this.GetOrder, this.IsValid);
            UpdateRefundCommand = new DelegateCommand(this.RefundDetail, this.CanRefund);
        }
        
        public ICommand QueryDetailsCommand { get; set; }
        public ICommand QueryOrderCommand { get; set; }
        public ICommand UpdateRefundCommand { get; set; }

        private void GetOrder(object para)
        {
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = this.conn;
                orderDetailsNameModel = Common<OrderDetailsNameModel>.QueryJoin(context, para.ToString());
                RaisePropertyChanged("OrderDetailsNameModels");
                RaisePropertyChanged("TotalPrice");
            }
        }

        private void GetOrderDetails(object para)
        {
            var dates = para.ToString().Split('|');
            var startDate = DateTime.Parse(dates[0]).Date;
            var endDate = DateTime.Parse(dates[1]).Date.AddDays(1);
            using (var context = new OneShopEntities())
            {
                context.Database.Connection.ConnectionString = this.conn;
                orderDetailsNameModel = Common<OrderDetailsNameModel>.QueryJoin(context, startDate, endDate);
                RaisePropertyChanged("OrderDetailsNameModels");
                RaisePropertyChanged("TotalPrice");
            }
        }

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
                        context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        context.Entry(details).State = System.Data.Entity.EntityState.Modified;
                        context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
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
        public int TotalCount
        {
            get => this.totalCount;
            set => this.totalCount = value;
        }

        public IList<OrderDetailsNameModel> OrderDetailsNameModels { get => this.orderDetailsNameModel; }

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

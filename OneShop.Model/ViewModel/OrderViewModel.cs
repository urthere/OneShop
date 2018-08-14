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
        }
        
        public ICommand QueryDetailsCommand { get; set; }

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

        private bool IsValid(object para)
        {
            return true;
        }
    }
}

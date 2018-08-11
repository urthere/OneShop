using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OneShop.Model;

namespace OneShop
{
    public class OrderDetailsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly StockViewModel stock;

        public OrderDetailsCommand(StockViewModel stockVM)
        {
            stock = stockVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}

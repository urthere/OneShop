using System;
using System.Windows.Input;

namespace OneShop.Model
{
    public class CommandBase : ICommand
    {

        private readonly Action<object> _command;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public CommandBase(Action<object> command, Func<object, bool> canExecute = null)
        {
            _command = command ?? throw new ArgumentNullException("command");
            _canExecute = canExecute;
        }
        public void Execute(object parameter)
        {
            _command(parameter);
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute(parameter);
        }
    }
    
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> executeAction;
        bool canExecuteCache;
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute = null)
        {
            this.executeAction = executeAction ?? throw new ArgumentNullException("command");
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}

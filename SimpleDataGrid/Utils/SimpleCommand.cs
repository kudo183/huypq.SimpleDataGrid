using System;
using System.Windows.Input;

namespace SimpleDataGrid
{
    public class SimpleCommand : ICommand
    {
        //information for debug
        private string _commandName;

        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public SimpleCommand(string commandName, Action execute, Func<bool> canExecute)
        {
            _commandName = commandName;
            _canExecute = canExecute;
            _execute = execute;
        }

        #region Implementation of ICommand

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute();

            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
        #endregion
    }
}

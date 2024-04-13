using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    // Class implementing the ICommand interface
    internal class RelayCommand : ICommand
    {
        // This field holds the Action delegate, which represents the method to be executed when the command is called.
        private readonly Action<object> execute;
        // This delegate, which determines whether a command can be executed at a given time.
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        // The value of CanExecuteChanged is checked to see if it is different from null, and if not,
        // the event is triggered, which notifies the UI to check again if the command can be executed.
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

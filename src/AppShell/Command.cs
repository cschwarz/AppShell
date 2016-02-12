using System;
using System.Windows.Input;

namespace AppShell
{
    public class Command : ICommand
    {
        private Action executeAction;
        private Action<object> executeActionWithParameter;
        private Func<bool> canExecuteAction;
        private Func<object, bool> canExecuteActionWithParameter;

        public Command(Action execute)
        {
            executeAction = execute;
        }

        public Command(Action<object> execute)
        {
            executeActionWithParameter = execute;
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            executeAction = execute;
            canExecuteAction = canExecute;
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            executeActionWithParameter = execute;
            canExecuteActionWithParameter = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteAction != null)
                return canExecuteAction();
            else if (canExecuteActionWithParameter != null)
                return canExecuteActionWithParameter(parameter);

            return true;
        }

        public void Execute(object parameter)
        {
            if (executeAction != null)
                executeAction();
            else if (executeActionWithParameter != null)
                executeActionWithParameter(parameter);
        }

        public void ChangeCanExecute()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}

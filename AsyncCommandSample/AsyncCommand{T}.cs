namespace AsyncCommandSample
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> execute;
        private readonly Func<T, bool> canExecute;
        private bool isRunning;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? (_ => true);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return !this.isRunning &&
                   this.canExecute((T)parameter);
        }

        public async void Execute(object parameter)
        {
            try
            {
                this.isRunning = true;
                await this.execute((T)parameter);
            }
            finally
            {
                this.isRunning = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}

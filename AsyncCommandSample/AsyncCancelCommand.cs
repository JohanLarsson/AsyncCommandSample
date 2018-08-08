namespace AsyncCommandSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class AsyncCancelCommand : ICommand
    {
        private readonly Func<CancellationToken, Task> execute;
        private readonly Func<bool> canExecute;
        private CancellationTokenSource cts;

        public AsyncCancelCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? (() => true);
            this.CancelCommand = new RelayCommand(
                () => this.cts?.Cancel(),
                () => this.cts?.IsCancellationRequested == false);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public ICommand CancelCommand { get; }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                throw new InvalidOperationException("Expected parameter to be null.");
            }

            return this.cts?.IsCancellationRequested == false &&
                   canExecute();
        }

        public async void Execute(object parameter)
        {
            if (parameter != null)
            {
                throw new InvalidOperationException("Expected parameter to be null.");
            }

            try
            {
                this.cts = new CancellationTokenSource();
                await this.execute(cts.Token);
            }
            finally
            {
                this.cts.Dispose();
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
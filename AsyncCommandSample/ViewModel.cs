namespace AsyncCommandSample
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class ViewModel : INotifyPropertyChanged
    {
        private int delay = 200;

        public ViewModel()
        {
            this.AsyncCommand = new AsyncCommand(
                () => Task.Delay(this.delay),
                () => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AsyncCommand { get; }

        public int Delay
        {
            get => this.delay;
            set
            {
                if (value == this.delay)
                {
                    return;
                }

                this.delay = value;
                OnPropertyChanged();
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

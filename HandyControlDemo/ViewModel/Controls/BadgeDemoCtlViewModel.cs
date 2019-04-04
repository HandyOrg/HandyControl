using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace HandyControlDemo.ViewModel.Controls
{
    public class BadgeDemoCtlViewModel : ViewModelBase
    {
        private int _count;

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public ICommand CountCommand { get; }

        public BadgeDemoCtlViewModel()
        {
            Count = 1;
            CountCommand = new RelayCommand(() => Count++);
        }
    }
}

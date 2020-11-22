using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class BadgeDemoCtlViewModel : BindableBase
    {
        private int _count = 1;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
        private DelegateCommand _CountCmd;
        public DelegateCommand CountCmd =>
            _CountCmd ?? (_CountCmd = new DelegateCommand(OnCount));

        void OnCount()
        {
            Count++;
        }
    }
}

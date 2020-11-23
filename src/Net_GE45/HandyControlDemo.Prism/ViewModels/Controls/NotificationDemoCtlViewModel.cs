using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class NotificationDemoCtlViewModel : BindableBase
    {
        private DelegateCommand _OpenCmd;
        public DelegateCommand OpenCmd =>
            _OpenCmd ?? (_OpenCmd = new DelegateCommand(() => Notification.Show(new AppNotification(), ShowAnimation, StaysOpen)));

        private ShowAnimation _showAnimation;
        public ShowAnimation ShowAnimation
        {
            get => _showAnimation;
            set => SetProperty(ref _showAnimation, value);
        }

        private bool _staysOpen = true;
        public bool StaysOpen
        {
            get => _staysOpen;
            set => SetProperty(ref _staysOpen, value);
        }
    }
}

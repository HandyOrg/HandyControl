using GalaSoft.MvvmLight.Command;
using System;
using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel
{
    public class NotificationDemoViewModel : ViewModelBase
    {
        public RelayCommand OpenCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Notification.Show(new AppNotification(), ShowAnimation, StaysOpen))).Value;

        private ShowAnimation _showAnimation;

        public ShowAnimation ShowAnimation
        {
            get => _showAnimation;
#if NET40
            set => Set(nameof(ShowAnimation) ,ref _showAnimation, value);
#else
            set => Set(ref _showAnimation, value);
#endif
        }

        private bool _staysOpen = true;

        public bool StaysOpen
        {
            get => _staysOpen;
#if NET40
            set => Set(nameof(StaysOpen) ,ref _staysOpen, value);
#else
            set => Set(ref _staysOpen, value);
#endif
        }
    }
}

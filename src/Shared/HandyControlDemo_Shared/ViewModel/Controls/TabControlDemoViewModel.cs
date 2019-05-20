using System;
using System.Windows;
using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Data;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
#endif

namespace HandyControlDemo.ViewModel
{
    public class TabControlDemoViewModel : ViewModelBase
    {
        public RelayCommand<CancelRoutedEventArgs> ClosingCmd => new Lazy<RelayCommand<CancelRoutedEventArgs>>(() =>
            new RelayCommand<CancelRoutedEventArgs>(Closing)).Value;

        private void Closing(CancelRoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closing");
        }

        public RelayCommand<RoutedEventArgs> ClosedCmd => new Lazy<RelayCommand<RoutedEventArgs>>(() =>
            new RelayCommand<RoutedEventArgs>(Closed)).Value;

        private void Closed(RoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closed");
        }
    }
}
using System.Windows;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class TabControlDemoViewModel : DemoViewModelBase<TabControlDemoModel>
    {
        public TabControlDemoViewModel(DataService dataService) => DataList = dataService.GetTabControlDemoDataList();

        public RelayCommand<CancelRoutedEventArgs> ClosingCmd => new RelayCommand<CancelRoutedEventArgs>(Closing);

        private void Closing(CancelRoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closing");
        }

        public RelayCommand<RoutedEventArgs> ClosedCmd => new RelayCommand<RoutedEventArgs>(Closed);

        private void Closed(RoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closed");
        }
    }
}
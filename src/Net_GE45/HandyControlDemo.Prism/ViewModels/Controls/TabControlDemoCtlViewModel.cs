using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using Prism.Commands;
using System.Windows;

namespace HandyControlDemo.ViewModels
{
    public class TabControlDemoCtlViewModel : DemoViewModelBase<TabControlDemoModel>
    {
        public TabControlDemoCtlViewModel(DataService dataService) => DataList = dataService.GetTabControlDemoDataList();

        private DelegateCommand<CancelRoutedEventArgs> _ClosingCmd;
        public DelegateCommand<CancelRoutedEventArgs> ClosingCmd =>
            _ClosingCmd ?? (_ClosingCmd = new DelegateCommand<CancelRoutedEventArgs>(Closing));

        private void Closing(CancelRoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closing");
        }

        private DelegateCommand<RoutedEventArgs> _ClosedCmd;
        public DelegateCommand<RoutedEventArgs> ClosedCmd =>
            _ClosedCmd ?? (_ClosedCmd = new DelegateCommand<RoutedEventArgs>(Closed));

        private void Closed(RoutedEventArgs args)
        {
            Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closed");
        }
    }
}

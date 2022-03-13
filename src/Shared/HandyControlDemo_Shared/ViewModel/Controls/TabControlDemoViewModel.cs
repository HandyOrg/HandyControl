using System.Windows;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class TabControlDemoViewModel : DemoViewModelBase<TabControlDemoModel>
{
    public TabControlDemoViewModel(DataService dataService) => DataList = dataService.GetTabControlDemoDataList();

    public RelayCommand<CancelRoutedEventArgs> ClosingCmd => new(Closing);

    private void Closing(CancelRoutedEventArgs args)
    {
        Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closing");
    }

    public RelayCommand<RoutedEventArgs> ClosedCmd => new(Closed);

    private void Closed(RoutedEventArgs args)
    {
        Growl.Info($"{(args.OriginalSource as TabItem)?.Header} Closed");
    }
}

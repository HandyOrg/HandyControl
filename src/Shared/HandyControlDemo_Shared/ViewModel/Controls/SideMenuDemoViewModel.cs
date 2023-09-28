using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.ViewModel;

public class SideMenuDemoViewModel : ViewModelBase
{
    public RelayCommand<FunctionEventArgs<object>> SwitchItemCmd => new(SwitchItem);

    private void SwitchItem(FunctionEventArgs<object> info) => Growl.Info((info.Info as SideMenuItem)?.Header.ToString());

    public RelayCommand<string> SelectCmd => new(Select);

    private void Select(string header) => Growl.Success(header);
}

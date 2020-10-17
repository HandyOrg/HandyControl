using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Data;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class SideMenuDemoViewModel : ViewModelBase
    {
        public RelayCommand<FunctionEventArgs<object>> SwitchItemCmd => new RelayCommand<FunctionEventArgs<object>>(SwitchItem);

        private void SwitchItem(FunctionEventArgs<object> info) => Growl.Info((info.Info as SideMenuItem)?.Header.ToString());

        public RelayCommand<string> SelectCmd => new RelayCommand<string>(Select);

        private void Select(string header) => Growl.Success(header);
    }
}
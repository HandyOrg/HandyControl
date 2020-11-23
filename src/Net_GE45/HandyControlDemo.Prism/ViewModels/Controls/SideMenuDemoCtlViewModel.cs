using HandyControl.Controls;
using HandyControl.Data;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class SideMenuDemoCtlViewModel : BindableBase
    {
        private DelegateCommand<FunctionEventArgs<object>> _SwitchItemCmd;
        public DelegateCommand<FunctionEventArgs<object>> SwitchItemCmd =>
            _SwitchItemCmd ?? (_SwitchItemCmd = new DelegateCommand<FunctionEventArgs<object>>(SwitchItem));


        private void SwitchItem(FunctionEventArgs<object> info) => Growl.Info((info.Info as SideMenuItem)?.Header.ToString());

        private DelegateCommand<string> _SelectCmd;
        public DelegateCommand<string> SelectCmd =>
            _SelectCmd ?? (_SelectCmd = new DelegateCommand<string>(Select));

        private void Select(string header) => Growl.Success(header);
    }
}

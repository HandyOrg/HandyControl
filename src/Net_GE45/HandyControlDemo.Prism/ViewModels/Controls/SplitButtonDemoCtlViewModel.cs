using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class SplitButtonDemoCtlViewModel : BindableBase
    {
        private DelegateCommand<string> _SelectCmd;
        public DelegateCommand<string> SelectCmd =>
            _SelectCmd ?? (_SelectCmd = new DelegateCommand<string>(str => Growl.Info(str)));

    }
}

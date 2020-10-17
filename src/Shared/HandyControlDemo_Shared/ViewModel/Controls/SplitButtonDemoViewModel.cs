using GalaSoft.MvvmLight;
using HandyControl.Controls;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class SplitButtonDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> SelectCmd => new RelayCommand<string>(str => Growl.Info(str));
    }
}

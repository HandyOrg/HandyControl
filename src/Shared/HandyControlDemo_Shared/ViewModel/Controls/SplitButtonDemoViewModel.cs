using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class SplitButtonDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> SelectCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(str => Growl.Info(str))).Value;
    }
}

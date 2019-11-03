using System;
using GalaSoft.MvvmLight;
using HandyControl.Controls;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif

namespace HandyControlDemo.ViewModel
{
    public class SplitButtonDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> SelectCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(str => Growl.Info(str))).Value;
    }
}

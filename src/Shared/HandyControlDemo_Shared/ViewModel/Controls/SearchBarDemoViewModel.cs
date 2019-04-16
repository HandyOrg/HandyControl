using System;
using GalaSoft.MvvmLight;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class SearchBarDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> SearchCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(Search)).Value;

        private void Search(string key)
        {
            Growl.Info(key);
        }
    }
}
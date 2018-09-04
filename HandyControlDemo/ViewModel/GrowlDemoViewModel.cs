using System;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools;

namespace HandyControlDemo.ViewModel
{
    public class GrowlDemoViewModel
    {
        public RelayCommand InfoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Info(ResourceHelper.GetString("GrowlInfo")))).Value;

        public RelayCommand SuccessCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Success(ResourceHelper.GetString("GrowlSuccess")))).Value;

        public RelayCommand WarningCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Warning(ResourceHelper.GetString("GrowlWarning")))).Value;

        public RelayCommand ErrorCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Error(ResourceHelper.GetString("GrowlError")))).Value;

        public RelayCommand AskCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Ask("GrowlAsk", (closeAction, b) =>
            {
                Growl.Info(b.ToString());
                closeAction?.Invoke();
            }))).Value;

        public RelayCommand FatalCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Fatal(ResourceHelper.GetString("GrowlFatal")))).Value;

        public RelayCommand ClearCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(Growl.Clear)).Value;
    }
}
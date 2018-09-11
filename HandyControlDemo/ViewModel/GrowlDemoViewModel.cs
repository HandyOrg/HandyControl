using System;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class GrowlDemoViewModel
    {
        public RelayCommand InfoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Info(Properties.Langs.Lang.GrowlInfo))).Value;

        public RelayCommand SuccessCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess))).Value;

        public RelayCommand WarningCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Warning(Properties.Langs.Lang.GrowlWarning))).Value;

        public RelayCommand ErrorCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Error(Properties.Langs.Lang.GrowlError))).Value;

        public RelayCommand AskCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Ask(Properties.Langs.Lang.GrowlAsk, (closeAction, b) =>
            {
                Growl.Info(b.ToString());
                closeAction?.Invoke();
            }))).Value;

        public RelayCommand FatalCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Fatal(Properties.Langs.Lang.GrowlFatal))).Value;

        public RelayCommand ClearCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(Growl.Clear)).Value;
    }
}
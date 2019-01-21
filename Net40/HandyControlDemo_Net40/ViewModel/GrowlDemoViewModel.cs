using System;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.ViewModel
{
    public class GrowlDemoViewModel
    {
        public RelayCommand InfoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Info(Properties.Langs.Lang.GrowlInfo))).Value;

        public RelayCommand SuccessCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess))).Value;

        public RelayCommand WarningCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Warning(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlWarning,
                CancelStr = Properties.Langs.Lang.Ignore,
                ActionBeforeClose = isConfirmed =>
                {
                    Growl.Info(isConfirmed.ToString());
                    return true;
                }
            }))).Value;

        public RelayCommand ErrorCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Error(Properties.Langs.Lang.GrowlError))).Value;

        public RelayCommand AskCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
            {
                Growl.Info(isConfirmed.ToString());
                return true;
            }))).Value;

        public RelayCommand FatalCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Fatal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlFatal,
                ShowDateTime = false
            }))).Value;

        public RelayCommand ClearCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(Growl.Clear)).Value;
    }
}
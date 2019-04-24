using System;
using System.Windows;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Window;

namespace HandyControlDemo.ViewModel
{
    public class GrowlDemoViewModel
    {
        private readonly string _token;

        public GrowlDemoViewModel()
        {
            
        }

        public GrowlDemoViewModel(string token)
        {
            _token = token;
        }

        public RelayCommand InfoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Info(Properties.Langs.Lang.GrowlInfo, _token))).Value;

        public RelayCommand SuccessCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess, _token))).Value;

        public RelayCommand WarningCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Warning(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlWarning,
                CancelStr = Properties.Langs.Lang.Ignore,
                ActionBeforeClose = isConfirmed =>
                {
                    Growl.Info(isConfirmed.ToString());
                    return true;
                },
                Token = _token
            }))).Value;

        public RelayCommand ErrorCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Error(Properties.Langs.Lang.GrowlError, _token))).Value;

        public RelayCommand AskCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
            {
                Growl.Info(isConfirmed.ToString());
                return true;
            }, _token))).Value;

        public RelayCommand FatalCmd => new Lazy<RelayCommand>(() => 
            new RelayCommand(() => Growl.Fatal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlFatal,
                ShowDateTime = false,
                Token = _token
            }))).Value;

        public RelayCommand ClearCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Clear())).Value;

        public RelayCommand NewWindowCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => new GrowlDemoWindow
            {
                Owner = Application.Current.MainWindow
            }.Show())).Value;
    }
}
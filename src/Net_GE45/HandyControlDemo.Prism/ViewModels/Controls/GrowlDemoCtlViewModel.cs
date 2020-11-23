using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Window;
using Prism.Commands;
using System.Windows;

namespace HandyControlDemo.ViewModels
{
    public class GrowlDemoCtlViewModel
    {
        private readonly string _token;
        public GrowlDemoCtlViewModel()
        {

        }
        public GrowlDemoCtlViewModel(string token)
        {
            _token = token;
        }

        #region Window

        private DelegateCommand _InfoCmd;
        public DelegateCommand InfoCmd =>
            _InfoCmd ?? (_InfoCmd = new DelegateCommand(() => Growl.Info(Properties.Langs.Lang.GrowlInfo, _token)));

        private DelegateCommand _SuccessCmd;
        public DelegateCommand SuccessCmd =>
            _SuccessCmd ?? (_SuccessCmd = new DelegateCommand(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess, _token)));

        private DelegateCommand _WarningCmd;
        public DelegateCommand WarningCmd =>
            _WarningCmd ?? (_WarningCmd = new DelegateCommand(() => Growl.Warning(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlWarning,
                CancelStr = Properties.Langs.Lang.Ignore,
                ActionBeforeClose = isConfirmed =>
                {
                    Growl.Info(isConfirmed.ToString());
                    return true;
                },
                Token = _token
            })));

        private DelegateCommand _ErrorCmd;
        public DelegateCommand ErrorCmd =>
            _ErrorCmd ?? (_ErrorCmd = new DelegateCommand(() => Growl.Error(Properties.Langs.Lang.GrowlError, _token)));

        private DelegateCommand _AskCmd;
        public DelegateCommand AskCmd =>
            _AskCmd ?? (_AskCmd = new DelegateCommand(() => Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
            {
                Growl.Info(isConfirmed.ToString());
                return true;
            }, _token)));

        private DelegateCommand _FatalCmd;
        public DelegateCommand FatalCmd =>
            _FatalCmd ?? (_FatalCmd = new DelegateCommand(() => Growl.Fatal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlFatal,
                ShowDateTime = false,
                Token = _token
            })));

        private DelegateCommand _ClearCmd;
        public DelegateCommand ClearCmd =>
            _ClearCmd ?? (_ClearCmd = new DelegateCommand(() => Growl.Clear(_token)));

        #endregion

        #region Desktop

        private DelegateCommand _InfoGlobalCmd;
        public DelegateCommand InfoGlobalCmd =>
            _InfoGlobalCmd ?? (_InfoGlobalCmd = new DelegateCommand(() => Growl.InfoGlobal(Properties.Langs.Lang.GrowlInfo)));

        private DelegateCommand _SuccessGlobalCmd;
        public DelegateCommand SuccessGlobalCmd =>
            _SuccessGlobalCmd ?? (_SuccessGlobalCmd = new DelegateCommand(() => Growl.SuccessGlobal(Properties.Langs.Lang.GrowlSuccess)));

        private DelegateCommand _WarningGlobalCmd;
        public DelegateCommand WarningGlobalCmd =>
            _WarningGlobalCmd ?? (_WarningGlobalCmd = new DelegateCommand(() => Growl.WarningGlobal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlWarning,
                CancelStr = Properties.Langs.Lang.Ignore,
                ActionBeforeClose = isConfirmed =>
                {
                    Growl.InfoGlobal(isConfirmed.ToString());
                    return true;
                }
            })));

        private DelegateCommand _ErrorGlobalCmd;
        public DelegateCommand ErrorGlobalCmd =>
            _ErrorGlobalCmd ?? (_ErrorGlobalCmd = new DelegateCommand(() => Growl.ErrorGlobal(Properties.Langs.Lang.GrowlError)));

        private DelegateCommand _AskGlobalCmd;
        public DelegateCommand AskGlobalCmd =>
            _AskGlobalCmd ?? (_AskGlobalCmd = new DelegateCommand(() => Growl.AskGlobal(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
            {
                Growl.InfoGlobal(isConfirmed.ToString());
                return true;
            })));

        private DelegateCommand _FatalGlobalCmd;
        public DelegateCommand FatalGlobalCmd =>
            _FatalGlobalCmd ?? (_FatalGlobalCmd = new DelegateCommand(() => Growl.FatalGlobal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlFatal,
                ShowDateTime = false
            })));

        private DelegateCommand _ClearGlobalCmd;
        public DelegateCommand ClearGlobalCmd =>
            _ClearGlobalCmd ?? (_ClearGlobalCmd = new DelegateCommand(Growl.ClearGlobal));

        #endregion

        private DelegateCommand _NewWindowCmd;
        public DelegateCommand NewWindowCmd =>
            _NewWindowCmd ?? (_NewWindowCmd = new DelegateCommand(() => new GrowlDemoWindow
            {
                Owner = Application.Current.MainWindow
            }.Show()));

    }
}
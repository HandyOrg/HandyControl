using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.ViewModel
{
    public class GrowlDemoViewModel
    {
        private RelayCommand _infoCmd;

        public RelayCommand InfoCmd =>
            _infoCmd ?? (_infoCmd = new RelayCommand(() => Growl.Info(Properties.Langs.Lang.GrowlInfo)));

        private RelayCommand _successCmd;

        public RelayCommand SuccessCmd =>
            _successCmd ?? (_successCmd = new RelayCommand(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess)));

        private RelayCommand _warningCmd;

        public RelayCommand WarningCmd =>
            _warningCmd ?? (_warningCmd = new RelayCommand(() =>
                Growl.Warning(new GrowlInfo
                {
                    Message = Properties.Langs.Lang.GrowlWarning,
                    CancelStr = Properties.Langs.Lang.Ignore,
                    ActionBeforeClose = isConfirmed =>
                    {
                        Growl.Info(isConfirmed.ToString());
                        return true;
                    }
                })));

        private RelayCommand _errorCmd;

        public RelayCommand ErrorCmd =>
            _errorCmd ?? (_errorCmd = new RelayCommand(() => Growl.Error(Properties.Langs.Lang.GrowlError)));

        private RelayCommand _askCmd;

        public RelayCommand AskCmd =>
            _askCmd ?? (_askCmd = new RelayCommand(() =>
                Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
                {
                    Growl.Info(isConfirmed.ToString());
                    return true;
                })));

        private RelayCommand _fatalCmd;

        public RelayCommand FatalCmd =>
            _fatalCmd ?? (_fatalCmd = new RelayCommand(() => Growl.Fatal(new GrowlInfo
            {
                Message = Properties.Langs.Lang.GrowlFatal,
                ShowDateTime = false
            })));

        private RelayCommand _clearCmd;

        public RelayCommand ClearCmd => _clearCmd ?? (_clearCmd = new RelayCommand(Growl.Clear));
    }
}
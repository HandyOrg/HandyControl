using System.Windows;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Window;

namespace HandyControlDemo.ViewModel;

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

    #region Window

    public RelayCommand InfoCmd => new(() => Growl.Info(Properties.Langs.Lang.GrowlInfo, _token));

    public RelayCommand SuccessCmd => new(() => Growl.Success(Properties.Langs.Lang.GrowlSuccess, _token));

    public RelayCommand WarningCmd => new(() => Growl.Warning(new GrowlInfo
    {
        Message = Properties.Langs.Lang.GrowlWarning,
        CancelStr = Properties.Langs.Lang.Ignore,
        ActionBeforeClose = isConfirmed =>
        {
            Growl.Info(isConfirmed.ToString());
            return true;
        },
        Token = _token
    }));

    public RelayCommand ErrorCmd => new(() => Growl.Error(Properties.Langs.Lang.GrowlError, _token));

    public RelayCommand AskCmd => new(() => Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
    {
        Growl.Info(isConfirmed.ToString());
        return true;
    }, _token));

    public RelayCommand FatalCmd => new(() => Growl.Fatal(new GrowlInfo
    {
        Message = Properties.Langs.Lang.GrowlFatal,
        ShowDateTime = false,
        Token = _token
    }));

    public RelayCommand ClearCmd => new(() => Growl.Clear(_token));

    #endregion

    #region Desktop

    public RelayCommand InfoGlobalCmd => new(() => Growl.InfoGlobal(Properties.Langs.Lang.GrowlInfo));

    public RelayCommand SuccessGlobalCmd => new(() => Growl.SuccessGlobal(Properties.Langs.Lang.GrowlSuccess));

    public RelayCommand WarningGlobalCmd => new(() => Growl.WarningGlobal(new GrowlInfo
    {
        Message = Properties.Langs.Lang.GrowlWarning,
        CancelStr = Properties.Langs.Lang.Ignore,
        ActionBeforeClose = isConfirmed =>
        {
            Growl.InfoGlobal(isConfirmed.ToString());
            return true;
        }
    }));

    public RelayCommand ErrorGlobalCmd => new(() => Growl.ErrorGlobal(Properties.Langs.Lang.GrowlError));

    public RelayCommand AskGlobalCmd => new(() => Growl.AskGlobal(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
    {
        Growl.InfoGlobal(isConfirmed.ToString());
        return true;
    }));

    public RelayCommand FatalGlobalCmd => new(() => Growl.FatalGlobal(new GrowlInfo
    {
        Message = Properties.Langs.Lang.GrowlFatal,
        ShowDateTime = false
    }));

    public RelayCommand ClearGlobalCmd => new(Growl.ClearGlobal);

    #endregion

    public RelayCommand NewWindowCmd => new(() => new GrowlDemoWindow
    {
        Owner = Application.Current.MainWindow
    }.Show());
}

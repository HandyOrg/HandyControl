using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;

namespace HandyControlDemo.ViewModel;

public class NonClientAreaViewModel : ViewModelBase
{
    public NonClientAreaViewModel()
    {
        VersionInfo = VersionHelper.GetVersion();
    }

    public RelayCommand<string> OpenViewCmd => new(OpenView);

    private void OpenView(string viewName)
    {
        Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
        Messenger.Default.Send(true, MessageToken.FullSwitch);
        Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{viewName}"), MessageToken.LoadShowContent);
    }

    private string _versionInfo;

    public string VersionInfo
    {
        get => _versionInfo;
#if NET40
        set => Set(nameof(VersionInfo), ref _versionInfo, value);
#else
        set => Set(ref _versionInfo, value);
#endif
    }
}

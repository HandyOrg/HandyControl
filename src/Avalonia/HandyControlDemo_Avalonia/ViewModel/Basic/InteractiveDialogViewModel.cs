using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.ViewModel;

public partial class InteractiveDialogViewModel : ObservableObject, IDialogResultable<string>
{
    public Action CloseAction { get; set; }

    [ObservableProperty]
    private string _result;

    [ObservableProperty]
    private string _message;

    [RelayCommand]
    private void Close() => CloseAction?.Invoke();
}

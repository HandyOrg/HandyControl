using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;
using HandyControlDemo.UserControl;
using HandyControlDemo.Window;

namespace HandyControlDemo.ViewModel;

public partial class DialogDemoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _dialogResult;

    [RelayCommand]
    private void ShowText(Control? element)
    {
        if (element == null)
        {
            Dialog.Show(new TextDialog());
        }
        else
        {
            Dialog.Show(new TextDialog(), MessageToken.DialogContainer);
        }
    }

    [RelayCommand]
    private async Task ShowInteractiveDialog(bool withTimer)
    {
        if (!withTimer)
        {
            DialogResult = await Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>();
        }
        else
        {
            await Dialog.Show<TextDialogWithTimer>(MessageToken.MainWindow).GetResultAsync<string>();
        }
    }

    [RelayCommand]
    private void NewWindow()
    {
        var window = new DialogDemoWindow();
        window.Show();
    }

    [RelayCommand]
    private void ShowWithToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            Dialog.Show(new TextDialog(), token);
        }
    }

    [RelayCommand]
    private void CloseMainWindowDialog(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            Dialog.Close(token);
        }
    }
}

using System;
using System.Windows;
using GalaSoft.MvvmLight;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
#endif
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;
using HandyControlDemo.UserControl;
using HandyControlDemo.Window;

namespace HandyControlDemo.ViewModel
{
    public class DialogDemoViewModel : ViewModelBase
    {
        private string _dialogResult;

        public string DialogResult
        {
            get => _dialogResult;
#if netle40
            set => Set(nameof(DialogResult), ref _dialogResult, value);
#else
            set => Set(ref _dialogResult, value);
#endif
        }

        public RelayCommand<FrameworkElement> ShowTextCmd => new Lazy<RelayCommand<FrameworkElement>>(() =>
            new RelayCommand<FrameworkElement>(ShowText)).Value;

        private void ShowText(FrameworkElement element)
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

#if netle40
        public RelayCommand<bool> ShowInteractiveDialogCmd => new Lazy<RelayCommand<bool>>(() =>
            new RelayCommand<bool>(ShowInteractiveDialog)).Value;

        private void ShowInteractiveDialog(bool withTimer)
        {
            if (!withTimer)
            {
                Dialog.Show<InteractiveDialog>()
                    .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                    .GetResultAsync<string>().ContinueWith(str => DialogResult = str.Result);
            }
            else
            {
                Dialog.Show<TextDialogWithTimer>(MessageToken.MainWindow).GetResultAsync<string>();
            }
        }
#else
        public RelayCommand<bool> ShowInteractiveDialogCmd => new Lazy<RelayCommand<bool>>(() =>
            new RelayCommand<bool>(async withTimer => await ShowInteractiveDialog(withTimer))).Value;

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
#endif

        public RelayCommand NewWindowCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => new DialogDemoWindow
            {
                Owner = Application.Current.MainWindow
            }.Show())).Value;

        public RelayCommand<string> ShowWithTokenCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(token => Dialog.Show(new TextDialog(), token))).Value;
    }
}
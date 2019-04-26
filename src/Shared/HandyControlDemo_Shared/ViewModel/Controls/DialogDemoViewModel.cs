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
using HandyControlDemo.UserControl;
using HandyControlDemo.UserControl.Basic;
using HandyControlDemo.ViewModel.Basic;
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

        public RelayCommand ShowTextCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowText)).Value;

        private void ShowText()
        {
            Dialog.Show(new TextDialog());
        }

#if netle40
        public RelayCommand ShowInteractiveDialogCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowInteractiveDialog)).Value;

        private void ShowInteractiveDialog()
        {
            Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>().ContinueWith(str => DialogResult = str.Result);
        }
#else
        public RelayCommand ShowInteractiveDialogCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(async () => await ShowInteractiveDialog())).Value;

        private async Task ShowInteractiveDialog()
        {
            DialogResult = await Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>();
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
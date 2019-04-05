using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControlDemo.UserControl;
using HandyControlDemo.UserControl.Basic;
using HandyControlDemo.ViewModel.Basic;

namespace HandyControlDemo.ViewModel
{
    public class DialogDemoViewModel : ViewModelBase
    {
        private string _dialogResult;

        public string DialogResult
        {
            get => _dialogResult;
            set => Set(nameof(DialogResult), ref _dialogResult, value);
        }

        public RelayCommand ShowTextCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowText)).Value;

        private void ShowText()
        {
            Dialog.Show(new TextDialog());
        }

        public RelayCommand ShowInteractiveDialogCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowInteractiveDialog)).Value;

        private void ShowInteractiveDialog()
        {
            Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>().ContinueWith(str => DialogResult = str.Result);
        }
    }
}
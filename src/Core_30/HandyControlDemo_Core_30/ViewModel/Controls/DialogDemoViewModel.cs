using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
            set => Set(ref _dialogResult, value);
        }

        public RelayCommand ShowTextCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowText)).Value;

        private void ShowText()
        {
            Dialog.Show(new TextDialog());
        }

        public RelayCommand ShowInteractiveDialogCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(async () => await ShowInteractiveDialog())).Value;

        private async Task ShowInteractiveDialog()
        {
            DialogResult = await Dialog.Show<InteractiveDialog>()
                .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                .GetResultAsync<string>();
        }
    }
}
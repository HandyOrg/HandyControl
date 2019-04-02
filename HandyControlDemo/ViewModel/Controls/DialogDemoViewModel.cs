using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.UserControl;
using HandyControlDemo.UserControl.Basic;
using HandyControlDemo.ViewModel.Basic;

namespace HandyControlDemo.ViewModel
{
    public class DialogDemoViewModel : ViewModelBase
    {
        private string _dialogResult;

        public RelayCommand ShowTextCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowText)).Value;

        public string DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        public ICommand ShowInteractiveDialogCmd { get; }

        public DialogDemoViewModel()
        {
            DialogResult = Lang.PleaseInput;

            ShowInteractiveDialogCmd = new RelayCommand<string>(async message =>
            {
                DialogResult = await Dialog.Show<InteractiveDialog>()
                    .Initialize<InteractiveDialogViewModel>(vm => vm.Message = message)
                    .GetResultAsync<string>();
            });
        }

        private void ShowText()
        {
            Dialog.Show(new TextDialog());
        }
    }
}
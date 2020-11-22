using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControlDemo.Views;
using HandyControlDemo.Window;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace HandyControlDemo.ViewModels
{
    public class DialogDemoCtlViewModel : BindableBase
    {
        private string _dialogResult;

        public string DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        private DelegateCommand<FrameworkElement> _ShowTextCmd;
        public DelegateCommand<FrameworkElement> ShowTextCmd =>
            _ShowTextCmd ?? (_ShowTextCmd = new DelegateCommand<FrameworkElement>(ShowText));

        private void ShowText(FrameworkElement element)
        {
            if (element == null)
            {
                Dialog.Show(new TextDialog());
            }
            else
            {
                Dialog.Show(new TextDialog(), "DialogContainer");
            }
        }

        private DelegateCommand<object> _ShowInteractiveDialogCmd;
        public DelegateCommand<object> ShowInteractiveDialogCmd =>
            _ShowInteractiveDialogCmd ?? (_ShowInteractiveDialogCmd = new DelegateCommand<object>(async withTimer => await ShowInteractiveDialog(withTimer)));

        //Todo: Not Working
        private async Task ShowInteractiveDialog(object withTimer)
        {
            if (withTimer is bool?)
            {
                bool? arg = withTimer as bool?;
                if (!arg.Value)
                {
                    DialogResult = await Dialog.Show<InteractiveDialog>()
                        .Initialize<InteractiveDialogViewModel>(vm => vm.Message = DialogResult)
                        .GetResultAsync<string>();
                }
                else
                {
                    await Dialog.Show<TextDialogWithTimer>("MainWindow").GetResultAsync<string>();
                }
            }
        }

        private DelegateCommand _NewWindowCmd;
        public DelegateCommand NewWindowCmd =>
            _NewWindowCmd ?? (_NewWindowCmd = new DelegateCommand(() => new DialogDemoWindow
            {
                Owner = Application.Current.MainWindow
            }.Show()));

        private DelegateCommand<string> _ShowWithTokenCmd;
        public DelegateCommand<string> ShowWithTokenCmd =>
            _ShowWithTokenCmd ?? (_ShowWithTokenCmd = new DelegateCommand<string>(token => Dialog.Show(new TextDialog(), token)));

    }
}
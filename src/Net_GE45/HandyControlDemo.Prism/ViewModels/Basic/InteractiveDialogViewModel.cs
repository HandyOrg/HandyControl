using HandyControl.Tools.Extension;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace HandyControlDemo.ViewModels
{
    public class InteractiveDialogViewModel : BindableBase, IDialogResultable<string>
    {
        public Action CloseAction { get; set; }

        private string _result;

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private DelegateCommand _CloseCmd;
        public DelegateCommand CloseCmd =>
            _CloseCmd ?? (_CloseCmd = new DelegateCommand(() => CloseAction?.Invoke()));

    }
}

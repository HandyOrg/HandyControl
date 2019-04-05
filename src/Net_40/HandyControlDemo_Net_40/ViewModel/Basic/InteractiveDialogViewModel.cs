using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.ViewModel.Basic
{
    public class InteractiveDialogViewModel : ViewModelBase, IDialogResultable<string>
    {
        public Action CloseAction { get; set; }

        private string _result;

        public string Result
        {
            get => _result;
            set => Set(nameof(Result), ref _result, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => Set(nameof(Message), ref _message, value);
        }

        public RelayCommand CloseCmd => new Lazy<RelayCommand>(() => new RelayCommand(() => CloseAction?.Invoke())).Value;
    }
}

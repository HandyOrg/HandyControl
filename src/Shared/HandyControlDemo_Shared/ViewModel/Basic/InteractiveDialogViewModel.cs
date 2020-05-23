using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.ViewModel
{
    public class InteractiveDialogViewModel : ViewModelBase, IDialogResultable<string>
    {
        public Action CloseAction { get; set; }

        private string _result;

        public string Result
        {
            get => _result;
#if NET40
            set => Set(nameof(Result), ref _result, value);
#else
            set => Set(ref _result, value);  
#endif
        }

        private string _message;

        public string Message
        {
            get => _message;
#if NET40
            set => Set(nameof(Message), ref _message, value);
#else
            set => Set(ref _message, value);
#endif
        }

        public RelayCommand CloseCmd => new Lazy<RelayCommand>(() => new RelayCommand(() => CloseAction?.Invoke())).Value;
    }
}

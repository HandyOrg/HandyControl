using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
#if !NET35
using HandyControl.Tools.Extension;
#endif

namespace HandyControlDemo.ViewModel
{
    public class InteractiveDialogViewModel : ViewModelBase
#if !NET35
        , IDialogResultable<string>
#endif
    {
        public Action CloseAction { get; set; }

        private string _result;

        public string Result
        {
            get => _result;
#if NET35 || NET40
            set => Set(nameof(Result), ref _result, value);
#else
            set => Set(ref _result, value);  
#endif
        }

        private string _message;

        public string Message
        {
            get => _message;
#if NET35 || NET40
            set => Set(nameof(Message), ref _message, value);
#else
            set => Set(ref _message, value);
#endif
        }

        public RelayCommand CloseCmd => new RelayCommand(() => CloseAction?.Invoke());
    }
}

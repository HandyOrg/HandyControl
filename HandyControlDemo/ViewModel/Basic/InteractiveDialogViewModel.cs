using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.ViewModel.Basic
{
    public class InteractiveDialogViewModel : ViewModelBase, IDialogResultable
    {
        private string _message;


        public object Result { get; set; }

        public Action ClosureToken { get; set; }

        public ICommand CloseCommand { get; }

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }


        public InteractiveDialogViewModel()
        {
            CloseCommand = new RelayCommand<string>(result =>
            {
                Result = result;
                ClosureToken();
            });
        }
    }
}

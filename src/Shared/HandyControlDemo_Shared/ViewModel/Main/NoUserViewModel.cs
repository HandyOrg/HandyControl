using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;

namespace HandyControlDemo.ViewModel
{
    public class NoUserViewModel : ViewModelBase
    {
        public RelayCommand<string> OpenViewCmd => new Lazy<RelayCommand<string>>(() =>
            new RelayCommand<string>(OpenView)).Value;

        private void OpenView(string viewName)
        {
            Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
            Messenger.Default.Send(true, MessageToken.FullSwitch);
            Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{viewName}"), MessageToken.LoadShowContent);
        }
    }
}
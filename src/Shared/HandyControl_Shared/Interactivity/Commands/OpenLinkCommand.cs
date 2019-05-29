using System;
using System.Diagnostics;
using System.Windows.Input;

namespace HandyControl.Interactivity
{
    public class OpenLinkCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is string link)
            {
                Process.Start(link);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
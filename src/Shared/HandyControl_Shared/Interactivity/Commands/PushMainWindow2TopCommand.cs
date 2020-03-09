using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using HandyControl.Tools.Interop;

namespace HandyControl.Interactivity
{
    public class PushMainWindow2TopCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility != Visibility.Visible)
            {
                Application.Current.MainWindow.Show();
                var hwndSource = (HwndSource)PresentationSource.FromDependencyObject(Application.Current.MainWindow);
                if (hwndSource != null)
                {
                    InteropMethods.SetForegroundWindow(hwndSource.Handle);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
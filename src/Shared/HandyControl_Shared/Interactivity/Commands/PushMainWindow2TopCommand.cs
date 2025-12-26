using System;
using System.Windows;
using System.Windows.Input;
using HandyControl.Tools;

namespace HandyControl.Interactivity;

public class PushMainWindow2TopCommand : ICommand
{
    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        Window mainWindow = Application.Current.MainWindow;
        if (mainWindow != null && mainWindow.Visibility != Visibility.Visible || mainWindow.WindowState == WindowState.Minimized)
        {
            WindowHelper.SetWindowToForeground(mainWindow);
        }
    }

    public event EventHandler CanExecuteChanged;
}

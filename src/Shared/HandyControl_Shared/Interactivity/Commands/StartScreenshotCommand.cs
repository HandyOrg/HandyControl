using System;
using System.Windows.Input;
using HandyControl.Controls;

namespace HandyControl.Interactivity;

public class StartScreenshotCommand : ICommand
{
    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => new Screenshot().Start();

    public event EventHandler CanExecuteChanged;
}

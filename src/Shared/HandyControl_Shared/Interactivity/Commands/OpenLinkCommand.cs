using System;
using System.Diagnostics;
using System.Windows.Input;

namespace HandyControl.Interactivity;

public class OpenLinkCommand : ICommand
{
    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        if (parameter is string link)
        {
            link = link.Replace("&", "^&");
            try
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {link}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch
            {
                // ignored
            }
        }
    }

    public event EventHandler CanExecuteChanged;
}

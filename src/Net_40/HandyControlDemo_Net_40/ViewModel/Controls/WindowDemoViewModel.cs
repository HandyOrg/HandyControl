using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using HandyControlDemo.Tools;

namespace HandyControlDemo.ViewModel
{
    public class WindowDemoViewModel
    {
        public RelayCommand<string> OpenWindowCmd => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(OpenWindow)).Value;

        private void OpenWindow(string windowTag)
        {
            if (AssemblyHelper.CreateInternalInstance($"Window.{windowTag}") is System.Windows.Window window)
            {
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
        }
    }
}
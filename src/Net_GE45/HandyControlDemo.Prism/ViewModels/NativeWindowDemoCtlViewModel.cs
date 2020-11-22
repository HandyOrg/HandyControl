using HandyControlDemo.Tools;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace HandyControlDemo.ViewModels
{
    public class NativeWindowDemoCtlViewModel : BindableBase
    {
        private DelegateCommand<string> _OpenWindowCmd;
        public DelegateCommand<string> OpenWindowCmd =>
            _OpenWindowCmd ?? (_OpenWindowCmd = new DelegateCommand<string>(OpenWindow));

        void OpenWindow(string param)
        {
            if (AssemblyHelper.CreateInternalInstance($"Window.{param}") is System.Windows.Window window)
            {
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }

            //if (param.Equals("NativeCommonWindow"))
            //{

            //}
            //else if (param.Equals("NavigationWindow"))
            //{

            //}
        }
    }
}

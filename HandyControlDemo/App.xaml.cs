using System.Globalization;
using System.Threading;
using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalData.Init();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(GlobalData.Config.Lang);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GlobalData.Save();
        }
    }
}

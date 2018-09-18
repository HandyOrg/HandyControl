using System.Globalization;
using System.Threading;
using System.Windows;

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
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        }
    }
}

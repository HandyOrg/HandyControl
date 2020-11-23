using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControlDemo.Views
{
    public partial class ScreenshotDemoCtl : IDisposable
    {
        public ScreenshotDemoCtl()
        {
            InitializeComponent();
            Screenshot.Snapped += Screenshot_Snapped;
        }

        private void Screenshot_Snapped(object sender, FunctionEventArgs<ImageSource> e)
        {
            new HandyControl.Controls.Window
            {
                Content = new Image
                {
                    Source = e.Info,
                    Stretch = Stretch.None
                },
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }.ShowDialog();
        }

        public void Dispose() => Screenshot.Snapped -= Screenshot_Snapped;
    }
}

using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    internal class GrowlWindow : Window
    {
        public Panel GrowlPanel { get; set; }

        public GrowlWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;

            GrowlPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 10, 10)
            };

            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                IsEnableInertia = true,
                Content = GrowlPanel
            };
        }

        public void Init()
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Height = desktopWorkingArea.Height;
            Left = desktopWorkingArea.Right - Width;
            Top = 0;
        }
    }
}
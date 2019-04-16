using System.Windows;

namespace HandyControlDemo.UserControl
{
    public partial class NotifyIconDemoCtl
    {
        public NotifyIconDemoCtl()
        {
            InitializeComponent();
        }

        private void ButtonPush_OnClick(object sender, RoutedEventArgs e) => NotifyIconContextContent.CloseContextControl();
    }
}

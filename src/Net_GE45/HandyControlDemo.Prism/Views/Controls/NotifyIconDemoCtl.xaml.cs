using HandyControlDemo.Tools;
using HandyControlDemo.ViewModels;
using System.Windows;

namespace HandyControlDemo.Views
{
    public partial class NotifyIconDemoCtl
    {
        public NotifyIconDemoCtl()
        {
            InitializeComponent();

            AssemblyHelper.Register(nameof(NotifyIconDemoCtl), this);
            Unloaded += NotifyIconDemoCtl_Unloaded;
        }

        private void NotifyIconDemoCtl_Unloaded(object sender, RoutedEventArgs e)
        {
            NotifyIconDemoCtlViewModel.Instance.Cleanup();
        }

        private void ButtonPush_OnClick(object sender, RoutedEventArgs e) => NotifyIconContextContent.CloseContextControl();
    }
}

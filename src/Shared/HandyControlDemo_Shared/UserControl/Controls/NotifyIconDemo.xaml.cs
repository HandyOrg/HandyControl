using System.Windows;
using HandyControlDemo.Tools;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo.UserControl;

public partial class NotifyIconDemo
{
    public NotifyIconDemo()
    {
        InitializeComponent();

        AssemblyHelper.Register(nameof(NotifyIconDemo), this);
        Unloaded += NotifyIconDemo_Unloaded;
    }

    private void NotifyIconDemo_Unloaded(object sender, RoutedEventArgs e) => ViewModelLocator.Instance.NotifyIconDemo.Cleanup();

    private void ButtonPush_OnClick(object sender, RoutedEventArgs e) => NotifyIconContextContent.CloseContextControl();
}

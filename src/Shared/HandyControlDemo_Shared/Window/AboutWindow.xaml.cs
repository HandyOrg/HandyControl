using System.Windows;
using HandyControlDemo.Tools;

namespace HandyControlDemo.Window;

public partial class AboutWindow
{
    public AboutWindow()
    {
        InitializeComponent();

        DataContext = this;
        CopyRight = VersionHelper.GetCopyright();
        Version = VersionHelper.GetVersion();
    }

    public static readonly DependencyProperty CopyRightProperty = DependencyProperty.Register(
        "CopyRight", typeof(string), typeof(AboutWindow), new PropertyMetadata(default(string)));

    public string CopyRight
    {
        get => (string) GetValue(CopyRightProperty);
        set => SetValue(CopyRightProperty, value);
    }

    public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
        "Version", typeof(string), typeof(AboutWindow), new PropertyMetadata(default(string)));

    public string Version
    {
        get => (string) GetValue(VersionProperty);
        set => SetValue(VersionProperty, value);
    }
}

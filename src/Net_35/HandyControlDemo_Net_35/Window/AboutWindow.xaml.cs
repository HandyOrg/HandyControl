using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace HandyControlDemo.Window
{
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();

            DataContext = this;

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            CopyRight = versionInfo.LegalCopyright;
            Version = $"v {versionInfo.FileVersion}";
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
}

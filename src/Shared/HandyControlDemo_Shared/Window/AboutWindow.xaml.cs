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
#if NET40
            var netVersion = "NET 40";
#elif NET45
            var netVersion = "NET 45";
#elif NET462
            var netVersion = "NET 462";
#elif NET47
            var netVersion = "NET 47";
#elif NET48
            var netVersion = "NET 48";
#elif NETCOREAPP3_0
            var netVersion = "CORE 30";
#elif NETCOREAPP3_1
            var netVersion = "CORE 31";
#else
            var netVersion = "NET 50";
#endif
            Version = $"v {versionInfo.FileVersion} {netVersion}";
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

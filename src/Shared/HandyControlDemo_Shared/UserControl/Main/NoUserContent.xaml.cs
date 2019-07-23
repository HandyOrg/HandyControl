using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Window;

namespace HandyControlDemo.UserControl
{
    public partial class NoUserContent
    {
        public NoUserContent()
        {
            InitializeComponent();
        }

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is string tag)
            {
                PopupConfig.IsOpen = false;
                if (tag.Equals(GlobalData.Config.Lang)) return;
                Growl.Ask(Properties.Langs.Lang.ChangeLangAsk, b =>
                {
                    if (!b) return true;
                    GlobalData.Config.Lang = tag;
                    GlobalData.Save();
                    var processModule = Process.GetCurrentProcess().MainModule;
                    if (processModule != null)
                        Process.Start(processModule.FileName);
                    Application.Current.Shutdown();
                    return true;
                });
            }
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is SkinType tag)
            {
                PopupConfig.IsOpen = false;
                if (tag.Equals(GlobalData.Config.Skin)) return;
                GlobalData.Config.Skin = tag;
                GlobalData.Save();
                ((App)Application.Current).UpdateSkin(tag);
            }
        }

        private void MenuAbout_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutWindow
            {
                Owner = Application.Current.MainWindow
            }.ShowDialog();
        }
    }
}

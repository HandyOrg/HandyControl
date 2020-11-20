using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Tools.Extension;
using HandyControlDemo.Window;
using System.Windows;
using System.Windows.Controls;

namespace HandyControlDemo.Views
{
    /// <summary>
    /// Interaction logic for NonClientAreaContent.xaml
    /// </summary>
    public partial class NonClientAreaContent
    {
        public NonClientAreaContent()
        {
            InitializeComponent();
        }

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is string langName)
            {
                PopupConfig.IsOpen = false;
                if (langName.Equals(GlobalData.Config.Lang))
                {
                    return;
                }
                LocalizationManager.Instance.CurrentCulture = new System.Globalization.CultureInfo(langName);
                ConfigHelper.Instance.SetLang(langName);
                GlobalData.Config.Lang = langName;
                GlobalData.Save();
            }
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is SkinType tag)
            {
                PopupConfig.IsOpen = false;
                if (tag.Equals(GlobalData.Config.Skin))
                {
                    return;
                }

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

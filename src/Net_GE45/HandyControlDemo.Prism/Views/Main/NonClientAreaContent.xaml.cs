using HandyControl.Data;
using HandyControlDemo.Data;
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

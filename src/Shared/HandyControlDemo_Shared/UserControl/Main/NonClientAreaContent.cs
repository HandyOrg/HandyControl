using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Window;

namespace HandyControlDemo.UserControl;

public partial class NonClientAreaContent
{
    public NonClientAreaContent()
    {
        InitializeComponent();
    }

    private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button { Tag: string langName })
        {
            PopupConfig.IsOpen = false;
            if (langName.Equals(GlobalData.Config.Lang))
            {
                return;
            }

            ConfigHelper.Instance.SetLang(langName);
            LangProvider.Culture = new CultureInfo(langName);
            Messenger.Default.Send<object>(null, MessageToken.LangUpdated);

            GlobalData.Config.Lang = langName;
            GlobalData.Save();
        }
    }

    private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

    private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button { Tag: SkinType skinType })
        {
            PopupConfig.IsOpen = false;
            if (skinType.Equals(GlobalData.Config.Skin))
            {
                return;
            }

            GlobalData.Config.Skin = skinType;
            GlobalData.Save();
            ((App) Application.Current).UpdateSkin(skinType);
            Messenger.Default.Send(skinType, MessageToken.SkinUpdated);
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

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Controls;
using HandyControlDemo.Data;

// ReSharper disable once CheckNamespace
namespace HandyControlDemo.UserControl
{
    public partial class NoUserContent
    {
        public NoUserContent()
        {
            InitializeComponent();

            ImageLang.Source = BitmapFrame.Create(new Uri($"pack://application:,,,/HandyControlDemo;component/Resources/Img/Flag/{GlobalData.Config.Lang}.png"));
        }

        private void ButtonLang_OnClick(object sender, RoutedEventArgs e) => PopupLang.IsOpen = true;

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is string tag)
            {
                PopupLang.IsOpen = false;
                if (tag.Equals(GlobalData.Config.Lang)) return;
                Growl.Ask(Properties.Langs.Lang.ChangeLangAsk, b =>
                {
                    if (!b) return true;
                    GlobalData.Config.Lang = tag;
                    GlobalData.Save();
                    Process.Start(Assembly.GetExecutingAssembly().Location);
                    Environment.Exit(0);
                    return true;
                });
            }
        }
    }
}

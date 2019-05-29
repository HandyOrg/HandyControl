using System;
using System.Net;
using System.Security.Authentication;
using System.Windows;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;

namespace HandyControlDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalData.Init();
            ConfigHelper.Instance.SetLang(GlobalData.Config.Lang);

            if (GlobalData.Config.Skin != SkinType.Default)
            {
                UpdateSkin(GlobalData.Config.Skin);
            }

#if !Core
            ConfigHelper.Instance.SetSystemVersionInfo(CommonHelper.GetSystemVersionInfo());
#endif
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(SslProtocols)0x00000C00;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GlobalData.Save();
        }

        internal void UpdateSkin(SkinType skin)
        {
            var skins0 = Resources.MergedDictionaries[0];
            skins0.MergedDictionaries.Clear();
            skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
            skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

            var skins1 = Resources.MergedDictionaries[1];
            skins1.MergedDictionaries.Clear();
            skins1.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            skins1.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControlDemo;component/Resources/Themes/Theme.xaml")
            });
            Current.MainWindow?.OnApplyTemplate();
        }
    }
}

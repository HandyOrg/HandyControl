using System;
using System.Windows;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;

namespace HandyControlDemo.Resources
{
    public class DemoTheme : Theme
    {
        public override Uri ThemeUri => new Uri("pack://application:,,,/HandyControlDemo;component/Resources/Themes/Theme.xaml");

        public override ResourceDictionary GetSkin(SkinType skinType) =>
            ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skinType);
    }
}

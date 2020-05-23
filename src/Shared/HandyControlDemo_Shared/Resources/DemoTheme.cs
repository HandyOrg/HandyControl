using System;
using System.Windows;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;

namespace HandyControlDemo.Resources
{
    public class DemoTheme : Theme
    {
        public override ResourceDictionary GetSkin(SkinType skinType) =>
            ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skinType);

        public override ResourceDictionary GetTheme() => new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HandyControlDemo;component/Resources/Themes/Theme.xaml")
        };
    }
}

using System;
using System.Collections.ObjectModel;
using System.Windows;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Themes
{
    public class Theme : ResourceDictionary
    {
        public Theme() => UpdateResource();

        private SkinType _skin;

        public virtual SkinType Skin
        {
            get => _skin;
            set
            {
                if (_skin == value) return;
                _skin = value;

                UpdateResource();
            }
        }

        public string Name { get; set; }

        public virtual Uri ThemeUri => new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml");

        public virtual ResourceDictionary GetSkin(SkinType skinType) => ResourceHelper.GetSkin(skinType);

        private void UpdateResource()
        {
            base.MergedDictionaries.Clear();
            base.MergedDictionaries.Add(GetSkin(Skin));
            base.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = ThemeUri
            });
        }

        public new Collection<ResourceDictionary> MergedDictionaries => throw new NotSupportedException();
    }
}

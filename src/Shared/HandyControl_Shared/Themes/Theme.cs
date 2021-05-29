using System;
using System.Linq;
using System.Windows;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Themes
{
    public class Theme : ResourceDictionary
    {
        public Theme()
        {
            if (DesignerHelper.IsInDesignMode)
            {
                MergedDictionaries.Add(new()
                {
                    Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml")
                });
                MergedDictionaries.Add(new()
                {
                    Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
                });
            }
            else
            {
                UpdateResource();
            }
        }

        private Uri _source;

        public new Uri Source
        {
            get => DesignerHelper.IsInDesignMode ? null : _source;
            set => _source = value;
        }

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

        public static readonly DependencyProperty SkinProperty = DependencyProperty.RegisterAttached(
            "Skin", typeof(SkinType), typeof(Theme), new PropertyMetadata(default(SkinType), OnSkinChanged));

        private static void OnSkinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                var skin = (SkinType) e.NewValue;

                if (element.Resources is Theme resource)
                {
                    resource.Skin = skin;
                }
                else
                {
                    var themes = element.Resources.MergedDictionaries.OfType<Theme>();
                    if (!themes.Any())
                    {
                        element.Resources.MergedDictionaries.Add(new Theme
                        {
                            Skin = skin
                        });
                    }
                    else
                    {
                        foreach (var subResource in element.Resources.MergedDictionaries.OfType<Theme>())
                        {
                            subResource.Skin = skin;
                        }
                    }
                }
            }
        }

        public static void SetSkin(DependencyObject element, SkinType value)
            => element.SetValue(SkinProperty, value);

        public static SkinType GetSkin(DependencyObject element)
            => (SkinType) element.GetValue(SkinProperty);

        public string Name { get; set; }

        public virtual ResourceDictionary GetSkin(SkinType skinType) => ResourceHelper.GetSkin(skinType);

        public virtual ResourceDictionary GetTheme() => ResourceHelper.GetTheme();

        private void UpdateResource()
        {
            if (DesignerHelper.IsInDesignMode) return;
            MergedDictionaries.Clear();
            MergedDictionaries.Add(GetSkin(Skin));
            MergedDictionaries.Add(GetTheme());
        }
    }
}

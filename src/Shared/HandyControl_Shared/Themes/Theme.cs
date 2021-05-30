using System;
using System.Collections.Generic;
using System.Windows;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using Microsoft.Win32;

namespace HandyControl.Themes
{
    public class Theme : ResourceDictionary
    {
        public Theme()
        {
            if (DesignerHelper.IsInDesignMode)
            {
                MergedDictionaries.Add(ResourceHelper.GetSkin(SkinType.Default));
                MergedDictionaries.Add(ResourceHelper.GetTheme());
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

        private SkinType _manualSkinType;

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
            if (d is not FrameworkElement element)
            {
                return;
            }

            var skin = (SkinType) e.NewValue;

            var themes = new List<Theme>();
            GetAllThemes(element.Resources, ref themes);

            if (themes.Count > 0)
            {
                foreach (var theme in themes)
                {
                    theme.Skin = skin;
                }
            }
            else
            {
                element.Resources.MergedDictionaries.Add(new Theme
                {
                    Skin = skin
                });
            }
        }

        private static void GetAllThemes(ResourceDictionary resourceDictionary, ref List<Theme> themes)
        {
            if (resourceDictionary is Theme theme)
            {
                themes.Add(theme);
            }

            // we must consider it's MergedDictionaries
            foreach (var dictionaryMergedDictionary in resourceDictionary.MergedDictionaries)
            {
                GetAllThemes(dictionaryMergedDictionary, ref themes);
            }
        }

        public static void SetSkin(DependencyObject element, SkinType value)
            => element.SetValue(SkinProperty, value);

        public static SkinType GetSkin(DependencyObject element)
            => (SkinType) element.GetValue(SkinProperty);

        private bool _syncWithSystem;

        public bool SyncWithSystem
        {
            get => _syncWithSystem;
            set
            {
                _syncWithSystem = value;

                if (value)
                {
                    _manualSkinType = Skin;
                    SyncWithSystemTheme();
                    SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
                }
                else
                {
                    SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
                    Skin = _manualSkinType;
                }
            }
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                SyncWithSystemTheme();
            }
        }

        private void SyncWithSystemTheme() => Skin = SystemHelper.DetermineIfInLightThemeMode() ? SkinType.Default : SkinType.Dark;

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

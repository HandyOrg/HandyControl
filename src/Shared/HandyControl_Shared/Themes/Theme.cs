using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;
using Microsoft.Win32;

namespace HandyControl.Themes;

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
            InitResource();
        }
    }

    #region Skin

    private SkinType _manualSkinType;

    private SkinType _skin;

    public virtual SkinType Skin
    {
        get => _skin;
        set
        {
            if (_skin == value) return;
            _skin = value;

            UpdateSkin();
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

    #endregion

    #region SyncWithSystem

    private bool _syncWithSystem;

    public bool SyncWithSystem
    {
        get => _syncWithSystem;
        set
        {
            _syncWithSystem = value;

            if (value)
            {
                _manualSkinType = _skin;
                SyncWithSystemTheme();

                SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            }
            else
            {
                SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;

                _skin = _manualSkinType;
                UpdateSkin();
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

    private void SyncWithSystemTheme()
    {
        _skin = SystemHelper.DetermineIfInLightThemeMode() ? SkinType.Default : SkinType.Dark;
        UpdateSkin();
    }

    #endregion

    #region AccentColor

    private Color? _accentColor;

    public Color? AccentColor
    {
        get => _accentColor;
        set
        {
            _accentColor = value;

            if (value == null)
            {
                _precSkin = null;
            }

            UpdateSkin();
        }
    }

    #endregion

    #region Source

    private Uri _source;

    public new Uri Source
    {
        get => DesignerHelper.IsInDesignMode ? null : _source;
        set => _source = value;
    }

    #endregion

    public string Name { get; set; }

    private SkinType _prevSkinType;

    private ResourceDictionary _precSkin;

    public virtual ResourceDictionary GetSkin(SkinType skinType)
    {
        if (_precSkin == null || _prevSkinType != skinType)
        {
            _precSkin = ResourceHelper.GetSkin(skinType);
            _prevSkinType = skinType;
        }

        if (!SyncWithSystem)
        {
            if (AccentColor != null)
            {
                UpdateAccentColor(AccentColor.Value);
            }
        }
        else
        {
            InteropMethods.DwmGetColorizationColor(out var color, out _);
            UpdateAccentColor(ColorHelper.ToColor(color));
        }

        return _precSkin;
    }

    public static Theme GetTheme(string name, ResourceDictionary resourceDictionary)
    {
        if (string.IsNullOrEmpty(name) || resourceDictionary == null)
        {
            return null;
        }

        return resourceDictionary.MergedDictionaries.OfType<Theme>().FirstOrDefault(item => Equals(item.Name, name));
    }

    public virtual ResourceDictionary GetTheme() => ResourceHelper.GetTheme();

    private void InitResource()
    {
        if (DesignerHelper.IsInDesignMode)
        {
            return;
        }

        MergedDictionaries.Clear();
        MergedDictionaries.Add(GetSkin(Skin));
        MergedDictionaries.Add(GetTheme());
    }

    private void UpdateAccentColor(Color color)
    {
        _precSkin[ResourceToken.PrimaryColor] = color;
        _precSkin[ResourceToken.DarkPrimaryColor] = color;
        _precSkin[ResourceToken.TitleColor] = color;
        _precSkin[ResourceToken.SecondaryTitleColor] = color;
    }

    private void UpdateSkin() => MergedDictionaries[0] = GetSkin(Skin);
}

public class StandaloneTheme : Theme
{
    public override ResourceDictionary GetTheme() => ResourceHelper.GetStandaloneTheme();
}

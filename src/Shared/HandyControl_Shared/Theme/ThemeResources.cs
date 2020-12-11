using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace HandyControl.Controls
{
    public class ThemeResources : ResourceDictionaryEx, ISupportInitialize
    {
        private static ThemeResources _current;

        private ResourceDictionary _lightResources;
        private ResourceDictionary _darkResources;
        private ResourceDictionary _violetResources;

        private bool _canBeAccessedAcrossThreads;

        public ThemeResources()
        {
            if (Current == null)
            {
                Current = this;
            }
        }

        internal static ThemeResources Current
        {
            get => _current;
            set
            {
                if (_current != null)
                {
                    throw new InvalidOperationException(nameof(Current) + " cannot be changed after it's set.");
                }

                _current = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines the light-dark preference for the overall
        /// theme of an app.
        /// </summary>
        /// <returns>
        /// A value of the enumeration. The initial value is the default theme set by the
        /// user in Windows settings.
        /// </returns>
        public ApplicationTheme? RequestedTheme
        {
            get => ThemeManager.Current.ApplicationTheme;
            set
            {
                if (ThemeManager.Current.ApplicationTheme != value)
                {
                    ThemeManager.Current.SetCurrentValue(ThemeManager.ApplicationThemeProperty, value);

                    if (DesignMode.DesignModeEnabled)
                    {
                        UpdateDesignTimeThemeDictionary();
                    }
                }
            }
        }

        public bool CanBeAccessedAcrossThreads
        {
            get => _canBeAccessedAcrossThreads;
            set
            {
                if (DesignMode.DesignModeEnabled)
                {
                    return;
                }

                if (IsInitialized)
                {
                    throw new InvalidOperationException();
                }

                _canBeAccessedAcrossThreads = value;
            }
        }

        #region Design Time

        private void DesignTimeInit()
        {
            Debug.Assert(DesignMode.DesignModeEnabled);
            UpdateDesignTimeSystemColors();
            UpdateDesignTimeThemeDictionary();
        }

        private void UpdateDesignTimeSystemColors()
        {
            Debug.Assert(DesignMode.DesignModeEnabled);

            if (IsInitializePending)
            {
                return;
            }

            var colors = GetDesignTimeSystemColors();
            MergedDictionaries.InsertOrReplace(0, colors);

        }

        private void UpdateDesignTimeThemeDictionary()
        {
            Debug.Assert(DesignMode.DesignModeEnabled);

            if (IsInitializePending)
            {
                return;
            }

            var appTheme = RequestedTheme ?? ApplicationTheme.Default;
            switch (appTheme)
            {
                case ApplicationTheme.Default:
                    EnsureLightResources();
                    updateTo(_lightResources);
                    break;
                case ApplicationTheme.Dark:
                    EnsureDarkResources();
                    updateTo(_darkResources);
                    break;
                case ApplicationTheme.Violet:
                    EnsureVioletResources();
                    updateTo(_violetResources);
                    break;
            }

            void updateTo(ResourceDictionary themeDictionary)
            {
                MergedDictionaries.RemoveIfNotNull(_lightResources);
                MergedDictionaries.RemoveIfNotNull(_darkResources);
                MergedDictionaries.RemoveIfNotNull(_violetResources);
                MergedDictionaries.Insert(1, themeDictionary);
            }
        }

        private ResourceDictionary GetDesignTimeSystemColors()
        {
            Debug.Assert(DesignMode.DesignModeEnabled);
            return new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Basic/Colors/Colors.xaml") };
        }

        #endregion

        #region ISupportInitialize

        private bool IsInitialized { get; set; }

        private bool IsInitializePending { get; set; }

        public new void BeginInit()
        {
            base.BeginInit();
            IsInitializePending = true;
            IsInitialized = false;
        }

        public new void EndInit()
        {
            IsInitializePending = false;
            IsInitialized = true;

            if (DesignMode.DesignModeEnabled)
            {
                DesignTimeInit();
            }
            else
            {
                ThemeManager.Current.Initialize();

                if (CanBeAccessedAcrossThreads)
                {
                    EnsureLightResources();
                    EnsureDarkResources();
                    EnsureVioletResources();

                    _lightResources.SealValues();
                    _darkResources.SealValues();
                    _violetResources.SealValues();
                }
            }

            base.EndInit();
        }

        void ISupportInitialize.BeginInit()
        {
            BeginInit();
        }

        void ISupportInitialize.EndInit()
        {
            EndInit();
        }

        #endregion

        private int MergedThemeDictionaryCount
        {
            get
            {
                int count = 0;
                if (IsMerged(_lightResources)) { count++; };
                if (IsMerged(_darkResources)) { count++; };
                if (IsMerged(_violetResources)) { count++; };
                return count;
            }
        }

        internal void ApplyApplicationTheme(ApplicationTheme theme)
        {
            int targetIndex = DesignMode.DesignModeEnabled ? 1 : 0;

            if (theme == ApplicationTheme.Default)
            {
                EnsureLightResources();
                MergedDictionaries.InsertOrReplace(targetIndex, _lightResources);
                MergedDictionaries.RemoveIfNotNull(_darkResources);
                MergedDictionaries.RemoveIfNotNull(_violetResources);
            }
            else if (theme == ApplicationTheme.Dark)
            {
                EnsureDarkResources();
                MergedDictionaries.InsertOrReplace(targetIndex, _darkResources);
                MergedDictionaries.RemoveIfNotNull(_lightResources);
                MergedDictionaries.RemoveIfNotNull(_violetResources);
            }
            else if (theme == ApplicationTheme.Violet)
            {
                EnsureVioletResources();
                MergedDictionaries.InsertOrReplace(targetIndex, _violetResources);
                MergedDictionaries.RemoveIfNotNull(_lightResources);
                MergedDictionaries.RemoveIfNotNull(_darkResources);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(theme));
            }
        }

        internal void ApplyElementTheme(ResourceDictionary target, ElementTheme theme)
        {
            ResourceDictionary mergedAppThemeDictionary = null;

            if (theme == ElementTheme.Default)
            {
                EnsureLightResources();
                target.MergedDictionaries.RemoveIfNotNull(_darkResources);
                target.MergedDictionaries.RemoveIfNotNull(_violetResources);
                target.MergedDictionaries.InsertIfNotExists(0, _lightResources);
                mergedAppThemeDictionary = _lightResources;
            }
            else if (theme == ElementTheme.Dark)
            {
                EnsureDarkResources();
                target.MergedDictionaries.RemoveIfNotNull(_lightResources);
                target.MergedDictionaries.RemoveIfNotNull(_violetResources);
                target.MergedDictionaries.InsertIfNotExists(0, _darkResources);
                mergedAppThemeDictionary = _darkResources;
            }
            else if (theme == ElementTheme.Violet)
            {
                EnsureVioletResources();
                target.MergedDictionaries.RemoveIfNotNull(_lightResources);
                target.MergedDictionaries.RemoveIfNotNull(_darkResources);
                target.MergedDictionaries.InsertIfNotExists(0, _violetResources);
                mergedAppThemeDictionary = _violetResources;
            }
            else // Default
            {
                target.MergedDictionaries.RemoveIfNotNull(_lightResources);
                target.MergedDictionaries.RemoveIfNotNull(_darkResources);
                target.MergedDictionaries.RemoveIfNotNull(_violetResources);
            }

            if (target is ResourceDictionaryEx etr)
            {
                etr.MergedAppThemeDictionary = mergedAppThemeDictionary;
            }
        }

        internal ResourceDictionary GetThemeDictionary(string key)
        {
            switch (key)
            {
                case ThemeManager.LightKey:
                    EnsureLightResources();
                    return _lightResources;
                case ThemeManager.DarkKey:
                    EnsureDarkResources();
                    return _darkResources;
                case ThemeManager.VioletKey:
                    EnsureVioletResources();
                    return _violetResources;
                default:
                    throw new ArgumentException();
            }
        }

        internal ResourceDictionary TryGetThemeDictionary(string key)
        {
            return key switch
            {
                ThemeManager.LightKey => _lightResources,
                ThemeManager.DarkKey => _darkResources,
                ThemeManager.VioletKey => _violetResources,
                _ => null,
            };
        }

        private void EnsureLightResources()
        {
            if (_lightResources == null)
            {
                _lightResources = InitializeThemeDictionary(ThemeManager.LightKey);
            }
        }

        private void EnsureDarkResources()
        {
            if (_darkResources == null)
            {
                _darkResources = InitializeThemeDictionary(ThemeManager.DarkKey);
            }
        }

        private void EnsureVioletResources()
        {
            if (_violetResources == null)
            {
                _violetResources = InitializeThemeDictionary(ThemeManager.VioletKey);
            }
        }

        private bool IsMerged(ResourceDictionary dictionary)
        {
            return dictionary != null && MergedDictionaries.Contains(dictionary);
        }

        private ResourceDictionary InitializeThemeDictionary(string key)
        {
            ResourceDictionary defaultThemeDictionary = ThemeManager.GetDefaultThemeDictionary(key);

            if (ThemeDictionaries.TryGetValue(key, out ResourceDictionary themeDictionary))
            {
                if (!ContainsDefaultThemeResources(themeDictionary, defaultThemeDictionary))
                {
                    themeDictionary.MergedDictionaries.Insert(0, defaultThemeDictionary);
                }
            }
            else
            {
                themeDictionary = defaultThemeDictionary;
            }

            return themeDictionary;
        }

        private static bool ContainsDefaultThemeResources(ResourceDictionary dictionary, ResourceDictionary defaultResources)
        {
            if (dictionary == defaultResources ||
                SourceEquals(dictionary.Source, defaultResources.Source))
            {
                return true;
            }

            foreach (var mergedDictionary in dictionary.MergedDictionaries)
            {
                if (mergedDictionary != null && ContainsDefaultThemeResources(mergedDictionary, defaultResources))
                {
                    return true;
                }
            }

            return false;

            static bool SourceEquals(Uri x, Uri y)
            {
                if (x == null || y == null)
                    return false;

                string xString = x.IsAbsoluteUri ? x.LocalPath : x.ToString();
                string yString = y.IsAbsoluteUri ? y.LocalPath : y.ToString();

                return string.Equals(xString, yString, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}

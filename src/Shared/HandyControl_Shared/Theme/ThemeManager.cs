using HandyControl.DesignTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace HandyControl.Controls
{
    public class ThemeManager : DependencyObject
    {
        internal const string LightKey = "Colors";
        internal const string DarkKey = "ColorsDark";
        internal const string VioletKey = "ColorsViolet";

        private static readonly RoutedEventArgs _actualThemeChangedEventArgs;

        private static readonly Dictionary<string, ResourceDictionary> _defaultThemeDictionaries = new Dictionary<string, ResourceDictionary>();

        private readonly Data _data;
        private bool _isInitialized;
        private bool _applicationInitialized;

        static ThemeManager()
        {
            ThemeProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(OnThemeChanged));

            _actualThemeChangedEventArgs = new RoutedEventArgs(ActualThemeChangedEvent);

            if (DesignMode.DesignModeEnabled)
            {
                _ = GetDefaultThemeDictionary(LightKey);
                _ = GetDefaultThemeDictionary(DarkKey);
                _ = GetDefaultThemeDictionary(VioletKey);
            }
        }

        private ThemeManager()
        {
            _data = new Data(this);
        }

        #region ApplicationTheme

        /// <summary>
        /// Identifies the ApplicationTheme dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationThemeProperty =
            DependencyProperty.Register(
                nameof(ApplicationTheme),
                typeof(ApplicationTheme?),
                typeof(ThemeManager),
                new PropertyMetadata(OnApplicationThemeChanged));

        /// <summary>
        /// Gets or sets a value that determines the light-dark preference for the overall
        /// theme of an app.
        /// </summary>
        /// <returns>
        /// A value of the enumeration. The initial value is the default theme set by the
        /// user in Windows settings.
        /// </returns>
        public ApplicationTheme? ApplicationTheme
        {
            get => (ApplicationTheme?)GetValue(ApplicationThemeProperty);
            set => SetValue(ApplicationThemeProperty, value);
        }

        private static void OnApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ThemeManager)d).UpdateActualApplicationTheme();
        }

        #endregion

        #region ActualApplicationTheme

        private static readonly DependencyPropertyKey ActualApplicationThemePropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(ActualApplicationTheme),
                typeof(ApplicationTheme),
                typeof(ThemeManager),
                new PropertyMetadata(HandyControl.Controls.ApplicationTheme.Default, OnActualApplicationThemeChanged));

        public static readonly DependencyProperty ActualApplicationThemeProperty =
            ActualApplicationThemePropertyKey.DependencyProperty;

        public ApplicationTheme ActualApplicationTheme
        {
            get => (ApplicationTheme)GetValue(ActualApplicationThemeProperty);
            private set => SetValue(ActualApplicationThemePropertyKey, value);
        }

        private static void OnActualApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tm = (ThemeManager)d;
            var newValue = (ApplicationTheme)e.NewValue;

            switch (newValue)
            {
                case HandyControl.Controls.ApplicationTheme.Default:
                    tm._defaultActualTheme = ElementTheme.Default;
                    break;
                case HandyControl.Controls.ApplicationTheme.Dark:
                    tm._defaultActualTheme = ElementTheme.Dark;
                    break;
                case HandyControl.Controls.ApplicationTheme.Violet:
                    tm._defaultActualTheme = ElementTheme.Violet;
                    break;
            }

            tm._data.ActualApplicationTheme = newValue;
            tm.ApplyApplicationTheme();
            tm.ActualApplicationThemeChanged?.Invoke(tm, null);
        }

        private void UpdateActualApplicationTheme()
        {
            ActualApplicationTheme = ApplicationTheme ?? HandyControl.Controls.ApplicationTheme.Default;
        }

        private void ApplyApplicationTheme()
        {
            if (_applicationInitialized)
            {
                Debug.Assert(ThemeResources.Current != null);
                ThemeResources.Current.ApplyApplicationTheme(ActualApplicationTheme);
            }
        }

        #endregion

        #region RequestedTheme

        /// <summary>
        /// Gets the UI theme that is used by the UIElement (and its child elements)
        /// for resource determination. The UI theme you specify with RequestedTheme can
        /// override the app-level RequestedTheme.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>A value of the enumeration, for example **Light**.</returns>
        public static ElementTheme GetRequestedTheme(FrameworkElement element)
        {
            return (ElementTheme)element.GetValue(RequestedThemeProperty);
        }

        /// <summary>
        /// Sets the UI theme that is used by the UIElement (and its child elements)
        /// for resource determination. The UI theme you specify with RequestedTheme can
        /// override the app-level RequestedTheme.
        /// </summary>
        /// <param name="element">The element on which to set the attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetRequestedTheme(FrameworkElement element, ElementTheme value)
        {
            element.SetValue(RequestedThemeProperty, value);
        }

        /// <summary>
        /// Identifies the RequestedTheme dependency property.
        /// </summary>
        public static readonly DependencyProperty RequestedThemeProperty = DependencyProperty.RegisterAttached(
            "RequestedTheme",
            typeof(ElementTheme),
            typeof(ThemeManager),
            new PropertyMetadata(ElementTheme.Default, OnRequestedThemeChanged));

        private static void OnRequestedThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            if (element.IsInitialized)
            {
                ApplyRequestedTheme(element);
            }
            else
            {
                SetSubscribedToInitialized(element, true);
            }
        }

        private static void ApplyRequestedTheme(FrameworkElement element)
        {
            var resources = element.Resources;
            var requestedTheme = GetRequestedTheme(element);
            ThemeResources.Current.ApplyElementTheme(resources, requestedTheme);

          
            if (requestedTheme != ElementTheme.Default)
            {
                SetTheme(element, requestedTheme);
            }
            else
            {
                element.ClearValue(ThemeProperty);
            }
        }

        #endregion

        #region Theme

        private static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached(
                "Theme",
                typeof(ElementTheme),
                typeof(ThemeManager),
                new FrameworkPropertyMetadata(
                    ElementTheme.Default,
                    FrameworkPropertyMetadataOptions.Inherits));

        private static void SetTheme(FrameworkElement element, ElementTheme value)
        {
            element.SetValue(ThemeProperty, value);
        }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                UpdateActualTheme(fe, (ElementTheme)e.NewValue);
            }
        }
        #endregion

        #region ActualTheme

        /// <summary>
        /// Gets the UI theme that is currently used by the element, which might be different
        /// than the RequestedTheme.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>A value of the enumeration, for example **Light**.</returns>
        public static ElementTheme GetActualTheme(FrameworkElement element)
        {
            return (ElementTheme)element.GetValue(ActualThemeProperty);
        }

        private static readonly DependencyPropertyKey ActualThemePropertyKey =
            DependencyProperty.RegisterAttachedReadOnly(
                "ActualTheme",
                typeof(ElementTheme),
                typeof(ThemeManager),
                new FrameworkPropertyMetadata(
                    ElementTheme.Default,
                    OnActualThemeChanged));

        /// <summary>
        /// Identifies the ActualTheme dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualThemeProperty =
            ActualThemePropertyKey.DependencyProperty;

        private static void OnActualThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                if (GetHasThemeResources(fe))
                {
                    UpdateThemeResourcesForElement(fe);
                }

                RaiseActualThemeChanged(fe);
            }
        }

        private static void UpdateActualTheme(FrameworkElement element, ElementTheme theme)
        {
            ElementTheme actualTheme;

            if (theme != ElementTheme.Default)
            {
                actualTheme = theme;
            }
            else
            {
                actualTheme = Current._defaultActualTheme;
            }

            element.SetValue(ActualThemePropertyKey, actualTheme);
        }

        private ElementTheme _defaultActualTheme = ElementTheme.Default;

        #endregion

        #region ActualThemeChanged

        public static readonly RoutedEvent ActualThemeChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ActualThemeChanged",
                RoutingStrategy.Direct,
                typeof(RoutedEventHandler),
                typeof(ThemeManager));

        public static void AddActualThemeChangedHandler(FrameworkElement element, RoutedEventHandler handler)
        {
            element.AddHandler(ActualThemeChangedEvent, handler);
        }

        public static void RemoveActualThemeChangedHandler(FrameworkElement element, RoutedEventHandler handler)
        {
            element.RemoveHandler(ActualThemeChangedEvent, handler);
        }

        private static void RaiseActualThemeChanged(FrameworkElement element)
        {
            element.RaiseEvent(_actualThemeChangedEventArgs);
        }

        #endregion

        #region IsThemeAware

        public static bool GetIsThemeAware(Window window)
        {
            return (bool)window.GetValue(IsThemeAwareProperty);
        }

        public static void SetIsThemeAware(Window window, bool value)
        {
            window.SetValue(IsThemeAwareProperty, value);
        }

        public static readonly DependencyProperty IsThemeAwareProperty =
            DependencyProperty.RegisterAttached(
                "IsThemeAware",
                typeof(bool),
                typeof(ThemeManager),
                new PropertyMetadata(OnIsThemeAwareChanged));

        private static void OnIsThemeAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if ((bool)e.NewValue)
                {
                    window.SetBinding(
                        InheritedApplicationThemeProperty,
                        new Binding(nameof(ActualApplicationTheme))
                        {
                            Source = Current._data
                        });

                    //UpdateWindowTheme((Window)d);
                }
                else
                {
                    window.ClearValue(InheritedApplicationThemeProperty);
                }
            }
        }

        private static readonly DependencyProperty InheritedApplicationThemeProperty =
            DependencyProperty.RegisterAttached(
                "InheritedApplicationTheme",
                typeof(ApplicationTheme),
                typeof(ThemeManager),
                new PropertyMetadata(HandyControl.Controls.ApplicationTheme.Default, OnInheritedApplicationThemeChanged));

        private static void OnInheritedApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //UpdateWindowTheme((Window)d);
        }

        #endregion

        #region HasThemeResources

        public static readonly DependencyProperty HasThemeResourcesProperty =
            DependencyProperty.RegisterAttached(
                "HasThemeResources",
                typeof(bool),
                typeof(ThemeManager),
                new PropertyMetadata(OnHasThemeResourcesChanged));

        public static bool GetHasThemeResources(FrameworkElement element)
        {
            return (bool)element.GetValue(HasThemeResourcesProperty);
        }

        public static void SetHasThemeResources(FrameworkElement element, bool value)
        {
            element.SetValue(HasThemeResourcesProperty, value);
        }

        private static void OnHasThemeResourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            if ((bool)e.NewValue)
            {
                if (element.IsInitialized)
                {
                    UpdateThemeResourcesForElement(element);
                }
                else
                {
                    SetSubscribedToInitialized(element, true);
                }
            }
            else
            {
                UpdateSubscribedToInitialized(element);
            }
        }

        private static void UpdateThemeResourcesForElement(FrameworkElement element)
        {
            UpdateElementThemeResources(element.Resources, GetEffectiveThemeKey(element));
        }

        private static void UpdateElementThemeResources(ResourceDictionary resources, string themeKey)
        {
            if (resources is ResourceDictionaryEx themeResources)
            {
                themeResources.Update(themeKey);
            }

            foreach (ResourceDictionary dictionary in resources.MergedDictionaries)
            {
                UpdateElementThemeResources(dictionary, themeKey);
            }
        }

        private static string GetEffectiveThemeKey(FrameworkElement element)
        {
            switch (GetActualTheme(element))
            {
                case ElementTheme.Default:
                    return LightKey;
                case ElementTheme.Dark:
                    return DarkKey;
                case ElementTheme.Violet:
                    return VioletKey;
            }

            throw new InvalidOperationException();
        }

        #endregion

        #region SubscribedToInitialized

        private static readonly DependencyProperty SubscribedToInitializedProperty =
            DependencyProperty.RegisterAttached(
                "SubscribedToInitialized",
                typeof(bool),
                typeof(ThemeManager),
                new PropertyMetadata(false, OnSubscribedToInitializedChanged));

        private static void SetSubscribedToInitialized(FrameworkElement element, bool value)
        {
            element.SetValue(SubscribedToInitializedProperty, value);
        }

        private static void OnSubscribedToInitializedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            if ((bool)e.NewValue)
            {
                element.Initialized += OnElementInitialized;
            }
            else
            {
                element.Initialized -= OnElementInitialized;
            }
        }

        private static void UpdateSubscribedToInitialized(FrameworkElement element)
        {
            if (ShouldSubscribeToInitialized(element))
            {
                SetSubscribedToInitialized(element, true);
            }
            else
            {
                element.ClearValue(SubscribedToInitializedProperty);
            }
        }

        private static bool ShouldSubscribeToInitialized(FrameworkElement element)
        {
            return !element.IsInitialized && (GetRequestedTheme(element) != ElementTheme.Default || GetHasThemeResources(element));
        }

        private static void OnElementInitialized(object sender, EventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.ClearValue(SubscribedToInitializedProperty);

            if (GetRequestedTheme(element) != ElementTheme.Default)
            {
                ApplyRequestedTheme(element);
            }

            if (GetHasThemeResources(element))
            {
                UpdateThemeResourcesForElement(element);
            }
        }

        #endregion

        public static ThemeManager Current { get; } = new ThemeManager();


        public TypedEventHandler<ThemeManager, object> ActualApplicationThemeChanged;

        internal static ResourceDictionary GetDefaultThemeDictionary(string key)
        {
            if (!_defaultThemeDictionaries.TryGetValue(key, out ResourceDictionary dictionary))
            {
                dictionary = new ResourceDictionary { Source = GetDefaultSource(key) };
                _defaultThemeDictionaries[key] = dictionary;
            }
            return dictionary;
        }

        internal static void SetDefaultThemeDictionary(string key, ResourceDictionary dictionary)
        {
            _defaultThemeDictionaries[key] = dictionary;
        }

        private static Uri GetDefaultSource(string theme)
        {
            return new Uri($"pack://application:,,,/HandyControl;component/Themes/Basic/Colors/{theme}.xaml");
        }

        private static ResourceDictionary FindDictionary(ResourceDictionary parent, Uri source)
        {
            if (parent.Source == source)
            {
                return parent;
            }
            else
            {
                foreach (var md in parent.MergedDictionaries)
                {
                    if (md != null)
                    {
                        if (md.Source == source)
                        {
                            return md;
                        }
                        else
                        {
                            var result = FindDictionary(md, source);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            return null;
        }

        internal void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            if (Application.Current != null)
            {
                var appResources = Application.Current.Resources;
                appResources.MergedDictionaries.RemoveAll<IntellisenseResourcesBase>();

                UpdateActualApplicationTheme();

                _applicationInitialized = true;

                ApplyApplicationTheme();
            }

            _isInitialized = true;
        }

        private void RunOnMainThread(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.BeginInvoke(action);
            }
        }

        private class Data : INotifyPropertyChanged
        {
            public Data(ThemeManager owner)
            {
                _actualApplicationTheme = owner.ActualApplicationTheme;
            }

            public ApplicationTheme ActualApplicationTheme
            {
                get => _actualApplicationTheme;
                set => Set(ref _actualApplicationTheme, value);
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
            {
                if (!Equals(storage, value))
                {
                    storage = value;
                    RaisePropertyChanged(propertyName);
                }
            }

            private ApplicationTheme _actualApplicationTheme;
        }
    }

    /// <summary>
    /// Declares the theme preference for an app.
    /// </summary>
    public enum ApplicationTheme
    {
        /// <summary>
        /// Use the **Light** default theme.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use the **Dark** default theme.
        /// </summary>
        Dark = 1,

        /// <summary>
        /// Use the **Violet** default theme
        /// </summary>
        Violet = 2
    }

    /// <summary>
    /// Specifies a UI theme that should be used for individual UIElement parts of an app UI.
    /// </summary>
    public enum ElementTheme
    {
        /// <summary>
        /// Use the **Light** default theme.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use the **Dark** default theme.
        /// </summary>
        Dark = 1,

        /// <summary>
        /// Use the **Violet** default theme
        /// </summary>
        Violet =2
    }
}

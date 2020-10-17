using System.ComponentModel;
using System.Globalization;
#if !NET35 && !NET40
using System.Runtime.CompilerServices;
#endif
#if !NET35
using System;
#endif
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using HandyControl.Data;
using HandyControl.Properties.Langs;

namespace HandyControl.Tools
{
    public class ConfigHelper : INotifyPropertyChanged
    {
        private ConfigHelper()
        {

        }

#if NET35
        private static ConfigHelper _configHelper;

        public static ConfigHelper Instance => _configHelper ??= new ConfigHelper();
#else
        public static ConfigHelper Instance = new Lazy<ConfigHelper>(() => new ConfigHelper()).Value; 
#endif

        private XmlLanguage _lang = XmlLanguage.GetLanguage("zh-cn");

        public XmlLanguage Lang
        {
            get => _lang;
            set
            {
                if (!_lang.IetfLanguageTag.Equals(value.IetfLanguageTag))
                {
                    _lang = value;
                    OnPropertyChanged(nameof(Lang));
                }
            }
        }

        public void SetLang(string lang)
        {
            LangProvider.Culture = new CultureInfo(lang);
            Application.Current.Dispatcher.Thread.CurrentUICulture = new CultureInfo(lang);
            Lang = XmlLanguage.GetLanguage(lang);
        }

        public void SetConfig(HandyControlConfig config)
        {
            SetLang(config.Lang);
            SetTimelineFrameRate(config.TimelineFrameRate);
        }

        public void SetTimelineFrameRate(int rate) => 
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(rate));

        public void SetWindowDefaultStyle(object resourceKey = null)
        {
            var metadata = resourceKey == null
                ? new FrameworkPropertyMetadata(Application.Current.FindResource(typeof(Window)))
                : new FrameworkPropertyMetadata(Application.Current.FindResource(resourceKey));

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), metadata);
        }

        public void SetNavigationWindowDefaultStyle(object resourceKey = null)
        {
            var metadata = resourceKey == null
                ? new FrameworkPropertyMetadata(Application.Current.FindResource(typeof(NavigationWindow)))
                : new FrameworkPropertyMetadata(Application.Current.FindResource(resourceKey));

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(NavigationWindow), metadata);
        }

        public event PropertyChangedEventHandler PropertyChanged;

#if NET35 || NET40
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#else
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
#endif
    }
}

using System;
using System.ComponentModel;
using System.Globalization;
#if !netle40
using System.Runtime.CompilerServices;
#endif
using System.Windows;
using System.Windows.Markup;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class ConfigHelper : INotifyPropertyChanged
    {
        private ConfigHelper()
        {

        }

        public static ConfigHelper Instance = new Lazy<ConfigHelper>(() => new ConfigHelper()).Value; 

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

        public void SetSystemVersionInfo(SystemVersionInfo info)
        {
            BlurWindow.SystemVersionInfo = info;           
        }

        public void SetLang(string lang)
        {
            Application.Current.Dispatcher.Thread.CurrentUICulture = new CultureInfo(lang);
            Lang = XmlLanguage.GetLanguage(lang);
        }

        public void SetConfig(HandyControlConfig config)
        {
            SetSystemVersionInfo(config.SystemVersionInfo);
            SetLang(config.Lang);
        }

        public event PropertyChangedEventHandler PropertyChanged;

#if netle40
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
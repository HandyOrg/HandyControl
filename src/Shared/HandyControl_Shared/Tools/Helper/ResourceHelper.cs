using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using HandyControl.Data;
using HandyControl.Themes;

namespace HandyControl.Tools
{
    /// <summary>
    ///     资源帮助类
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        ///     获取资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetResource<T>(string key)
        {
            if (Application.Current.TryFindResource(key) is T resource)
            {
                return resource;
            }

            return default;
        }

        internal static T GetResourceInternal<T>(string key)
        {
            if (GetTheme()[key] is T resource)
            {
                return resource;
            }

            return default;
        }

        public static Theme GetTheme(string name, ResourceDictionary resourceDictionary)
        {
            if (string.IsNullOrEmpty(name) || resourceDictionary == null)
            {
                return null;
            }

            return resourceDictionary.MergedDictionaries.OfType<Theme>().FirstOrDefault(item => Equals(item.Name, name));
        }

        /// <summary>
        ///     获取皮肤
        /// </summary>
        public static ResourceDictionary GetSkin(Assembly assembly, string themePath, SkinType skin)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{skin}.xaml");
                return new ResourceDictionary
                {
                    Source = uri
                };
            }
            catch
            {
                return new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{SkinType.Default}.xaml")
                };
            }
        }

        /// <summary>
        ///     get HandyControl skin
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static ResourceDictionary GetSkin(SkinType skin) => new()
        {
            Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skin}.xaml")
        };

        /// <summary>
        ///     get HandyControl theme
        /// </summary>
        public static ResourceDictionary GetTheme() => new()
        {
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        };
    }
}

using System;
using System.Collections.Generic;
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
        ///     获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key) => Application.Current.TryFindResource(key) as string;

        /// <summary>
        ///     获取字符串
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="keyArr"></param>
        /// <returns></returns>
        public static string GetString(string separator = ";", params string[] keyArr) =>
            string.Join(separator, keyArr.Select(key => Application.Current.TryFindResource(key) as string).ToList());

        /// <summary>
        ///     获取字符串
        /// </summary>
        /// <param name="keyArr"></param>
        /// <returns></returns>
        public static List<string> GetStringList(params string[] keyArr) => keyArr.Select(key => Application.Current.TryFindResource(key) as string).ToList();

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

        /// <summary>
        ///     获取HandyControl皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static ResourceDictionary GetSkin(SkinType skin) => new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skin.ToString()}.xaml")
        };

        /// <summary>
        ///     获取皮肤
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="themePath"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static ResourceDictionary GetSkin(Assembly assembly, string themePath, SkinType skin)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{skin.ToString()}.xaml");
                return new ResourceDictionary
                {
                    Source = uri
                };
            }
            catch
            {
                return new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/{themePath}/Skin{SkinType.Default.ToString()}.xaml")
                };
            }        
        }

        public static Theme GetTheme(string name, ResourceDictionary resourceDictionary)
        {
            if (string.IsNullOrEmpty(name) || resourceDictionary == null) return null;
            return resourceDictionary.MergedDictionaries.OfType<Theme>()
                .FirstOrDefault(item => Equals(item.Name, name));
        }
    }
}
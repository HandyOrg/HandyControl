using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
        public static T GetResource<T>(string key) where T : class => Application.Current.TryFindResource(key) as T;
    }
}
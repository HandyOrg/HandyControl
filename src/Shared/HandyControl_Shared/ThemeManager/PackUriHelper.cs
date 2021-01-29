using System;

namespace HandyControl.ThemeManager
{
    public static class PackUriHelper
    {
        public static Uri GetAbsoluteUri(string Path)
        {
            return new Uri($"pack://application:,,,/HandyControl;component/{Path}");
        }

        public static Uri GetAbsoluteUri(string Namespace, string Path)
        {
            return new Uri($"pack://application:,,,/{Namespace};component/{Path}");
        }
    }
}

using System;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControlDemo.Data
{
    internal class AppConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

        public string Lang { get; set; } = "zh-cn";

        public ApplicationTheme Theme { get; set; } = ApplicationTheme.Dark;
    }
}

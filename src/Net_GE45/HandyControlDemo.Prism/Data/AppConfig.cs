using HandyControl.Data;
using System;

namespace HandyControlDemo.Data
{
    internal class AppConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

        public string Lang { get; set; } = "en";

        public SkinType Skin { get; set; }
    }
}
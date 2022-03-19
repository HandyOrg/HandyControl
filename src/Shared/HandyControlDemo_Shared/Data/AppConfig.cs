using System;
using HandyControl.Data;

namespace HandyControlDemo.Data;

internal class AppConfig
{
    public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

    public string Lang { get; set; } = "zh-cn";

    public SkinType Skin { get; set; }
}

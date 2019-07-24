using System.IO;
#if !Core
using Newtonsoft.Json;
#endif
namespace HandyControlDemo.Data
{
    internal class GlobalData
    {
        public static void Init()
        {
            if (File.Exists(AppConfig.SavePath))
            {
                try
                {
                    var json = File.ReadAllText(AppConfig.SavePath);
#if !Core
                    Config = JsonConvert.DeserializeObject<AppConfig>(json);
#else
                    Config = System.Text.Json.JsonSerializer.Deserialize<AppConfig>(json);
#endif
                }
                catch
                {
                    Config = new AppConfig();
                }
            }
            else
            {
                Config = new AppConfig();
            }
        }

        public static void Save()
        {
#if !Core
            var json = JsonConvert.SerializeObject(Config);
#else
            var json = System.Text.Json.JsonSerializer.Serialize<AppConfig>(Config);
#endif
            File.WriteAllText(AppConfig.SavePath, json);
        }

        public static AppConfig Config { get; set; }

        public static bool NotifyIconIsShow { get; set; }
    }
}
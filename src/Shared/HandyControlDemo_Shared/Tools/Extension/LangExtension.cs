using HandyControlDemo.Properties.Langs;

namespace HandyControlDemo.Tools.Extension
{
    public class LangExtension : HandyControl.Tools.Extension.LangExtension
    {
        public LangExtension()
        {
            Source = LangProvider.Instance;
        }
    }
}

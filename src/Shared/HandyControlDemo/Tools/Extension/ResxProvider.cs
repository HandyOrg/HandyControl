using HandyControlDemo.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HandyControlDemo.Tools.Extension
{
    public class ResxProvider : ILocalizationProvider
    {
        public IEnumerable<CultureInfo> Cultures => throw new NotImplementedException();

        public object Localize(string key)
        {
            return Lang.ResourceManager.GetObject(key);
        }
    }
}

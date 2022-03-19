using System;
using System.Globalization;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class Long2FileSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return Properties.Langs.Lang.UnknownSize;
        if (value is long intValue)
        {
            if (intValue < 0)
            {
                return Properties.Langs.Lang.UnknownSize;
            }
            if (intValue < 1024)
            {
                return $"{intValue} B";
            }
            if (intValue < 1048576)
            {
                return $"{intValue / 1024.0:0.00} KB";
            }
            if (intValue < 1073741824)
            {
                return $"{intValue / 1048576.0:0.00} MB";
            }
            if (intValue < 1099511627776)
            {
                return $"{intValue / 1073741824.0:0.00} GB";
            }
            if (intValue < 1125899906842624)
            {
                return $"{intValue / 1099511627776.0:0.00} TB";
            }
            if (intValue < 1152921504606847000)
            {
                return $"{intValue / 1125899906842624.0:0.00} PB";
            }
            return Properties.Langs.Lang.TooLarge;
        }
        return Properties.Langs.Lang.Unknown;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

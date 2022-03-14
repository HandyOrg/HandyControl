using System;
using System.Globalization;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class Boolean2StringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            if (parameter is string str)
            {
                var arr = str.Split(';');
                if (arr.Length > 1)
                {
                    return boolValue ? arr[1] : arr[0];
                }
                return "";
            }
            return "";
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

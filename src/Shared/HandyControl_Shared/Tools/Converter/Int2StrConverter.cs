using System;
using System.Globalization;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class Int2StringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            if (int.TryParse(value.ToString(), out var intValue))
            {
                if (parameter is string str)
                {
                    var arr = str.Split(';');
                    if (arr.Length > intValue)
                    {
                        return arr[intValue];
                    }

                    return intValue;
                }

                return intValue;
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

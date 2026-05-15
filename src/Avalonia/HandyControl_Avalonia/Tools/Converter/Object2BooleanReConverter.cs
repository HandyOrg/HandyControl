using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

public class Object2BooleanReConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

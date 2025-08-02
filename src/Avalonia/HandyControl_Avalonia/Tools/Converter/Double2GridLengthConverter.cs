using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

public class Double2GridLengthConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double doubleValue)
        {
            return new GridLength(0);
        }

        if (double.IsNaN(doubleValue) || double.IsInfinity(doubleValue))
        {
            return new GridLength(1.0, GridUnitType.Star);
        }

        return new GridLength(doubleValue);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

internal class DoubleExpandConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double originalValue)
        {
            return value;
        }

        if (!double.TryParse(parameter as string, out double expandValue))
        {
            return value;
        }

        return originalValue + expandValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

public class BorderCircularConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Rect rect)
        {
            return AvaloniaProperty.UnsetValue;
        }

        if (rect.Width < double.Epsilon || rect.Height < double.Epsilon)
        {
            return new CornerRadius();
        }

        double min = Math.Min(rect.Width, rect.Height);
        return new CornerRadius(min / 2);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

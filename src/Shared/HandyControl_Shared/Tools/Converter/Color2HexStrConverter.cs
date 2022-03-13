using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HandyControl.Tools.Converter;

public class Color2HexStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            var color = brush.Color;
            return color.A < 255 ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}" : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            try
            {
                if (ColorConverter.ConvertFromString(str) is Color color)
                {
                    return new SolidColorBrush(color);
                }
            }
            catch
            {
                return new SolidColorBrush(default);
            }
            return new SolidColorBrush(default);
        }
        return new SolidColorBrush(default);
    }
}

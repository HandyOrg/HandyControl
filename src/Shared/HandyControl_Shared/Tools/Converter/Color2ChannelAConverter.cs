using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HandyControl.Tools.Converter;

public class Color2ChannelAConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color.A;
        }
        return 0;
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
                return parameter;
            }
            return parameter;
        }
        return parameter;
    }
}

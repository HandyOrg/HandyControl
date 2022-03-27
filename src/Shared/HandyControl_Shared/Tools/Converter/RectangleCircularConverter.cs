using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class RectangleCircularConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is double width && values[1] is double height)
        {
            if (width < double.Epsilon || height < double.Epsilon)
            {
                return .0;
            }

            var min = Math.Min(width, height);
            return min / 2;
        }

        return DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

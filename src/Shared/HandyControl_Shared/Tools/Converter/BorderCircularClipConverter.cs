//referenced from https://stackoverflow.com/a/5650367/9639378

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HandyControl.Tools.Converter;

public class BorderCircularClipConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 3 && values[0] is double width && values[1] is double height && values[2] is CornerRadius radius)
        {
            if (width < double.Epsilon || height < double.Epsilon)
            {
                return Geometry.Empty;
            }

            var clip = new RectangleGeometry(new Rect(0, 0, width, height), radius.TopLeft, radius.TopLeft);
            clip.Freeze();

            return clip;
        }

        return DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

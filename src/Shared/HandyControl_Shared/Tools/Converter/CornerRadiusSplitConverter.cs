using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class CornerRadiusSplitConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not CornerRadius cornerRadius)
        {
            return value;
        }

        if (parameter is not string str)
        {
            return value;
        }

        var arr = str.Split(',');
        if (arr.Length != 4) return cornerRadius;

        return new CornerRadius(
            arr[0].Trim().Equals("1") ? cornerRadius.TopLeft : 0,
            arr[1].Trim().Equals("1") ? cornerRadius.TopRight : 0,
            arr[2].Trim().Equals("1") ? cornerRadius.BottomRight : 0,
            arr[3].Trim().Equals("1") ? cornerRadius.BottomLeft : 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

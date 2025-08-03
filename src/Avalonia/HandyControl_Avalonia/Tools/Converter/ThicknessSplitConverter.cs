using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

public class ThicknessSplitConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Thickness thickness || parameter is not string str)
        {
            return value;
        }

        string[] arr = str.Split(',');

        if (arr.Length != 4)
        {
            return thickness;
        }

        return new Thickness(
            left: double.TryParse(arr[0], out double leftTimes) ? leftTimes * thickness.Left : thickness.Left,
            top: double.TryParse(arr[1], out double topTimes) ? topTimes * thickness.Top : thickness.Top,
            right: double.TryParse(arr[2], out double rightTimes) ? rightTimes * thickness.Right : thickness.Right,
            bottom: double.TryParse(arr[3], out double bottomTimes) ? bottomTimes * thickness.Bottom : thickness.Bottom
        );
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

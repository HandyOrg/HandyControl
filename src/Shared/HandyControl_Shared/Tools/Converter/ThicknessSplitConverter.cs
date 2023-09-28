using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

public class ThicknessSplitConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Thickness thickness)
        {
            if (parameter is string str)
            {
                var arr = str.Split(',');

                if (arr.Length != 4)
                {
                    return thickness;
                }

                var result = new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);

                if (double.TryParse(arr[0], out double leftTimes))
                {
                    result.Left = leftTimes * thickness.Left;
                }

                if (double.TryParse(arr[1], out double topTimes))
                {
                    result.Top = topTimes * thickness.Top;
                }

                if (double.TryParse(arr[2], out double rightTimes))
                {
                    result.Right = rightTimes * thickness.Right;
                }

                if (double.TryParse(arr[3], out double bottomTimes))
                {
                    result.Bottom = bottomTimes * thickness.Bottom;
                }

                return result;
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

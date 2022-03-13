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
                if (arr.Length != 4) return thickness;

                return new Thickness(
                    arr[0].Equals("1") ? thickness.Left : 0,
                    arr[1].Equals("1") ? thickness.Top : 0,
                    arr[2].Equals("1") ? thickness.Right : 0,
                    arr[3].Equals("1") ? thickness.Bottom : 0);
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

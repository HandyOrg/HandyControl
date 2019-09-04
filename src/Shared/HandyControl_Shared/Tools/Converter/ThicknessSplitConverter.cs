using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class ThicknessSplitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness cornerRadius)
            {
                if (parameter is string str)
                {
                    var arr = str.Split(',');
                    if (arr.Length != 4) return cornerRadius;

                    return new CornerRadius(arr[0].Equals("1") ? cornerRadius.Left : 0,
                        arr[1].Equals("1") ? cornerRadius.Top : 0,
                        arr[1].Equals("1") ? cornerRadius.Right : 0,
                        arr[1].Equals("1") ? cornerRadius.Bottom : 0);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

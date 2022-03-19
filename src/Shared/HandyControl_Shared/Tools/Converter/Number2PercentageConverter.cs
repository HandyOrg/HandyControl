using System;
using System.Globalization;
using System.Windows.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools.Extension;

namespace HandyControl.Tools.Converter;

public class Number2PercentageConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length != 2) return .0;

        var obj1 = values[0];
        var obj2 = values[1];

        if (obj1 == null || obj2 == null) return .0;

        var str1 = values[0].ToString();
        var str2 = values[1].ToString();

        var v1 = str1.Value<double>();
        var v2 = str2.Value<double>();

        if (MathHelper.IsVerySmall(v2)) return 100.0;

        return v1 / v2 * 100;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

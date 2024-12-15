using System;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;

namespace HandyControlDemo.Tools.Converter;

public class StringRepeatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string strValue)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        int num;

        switch (parameter)
        {
            case string numStr:
                {
                    if (!int.TryParse(numStr, out num))
                    {
                        return strValue;
                    }

                    break;
                }
            case int intValue:
                num = intValue;
                break;
            default:
                return strValue;
        }

        for (int i = 0; i < num; i++)
        {
            builder.Append(strValue);
        }

        return builder.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

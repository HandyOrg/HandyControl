using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace HandyControl.Tools.Converter;

public class BooleanArr22BooleanConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values?.All(x => x is bool && (bool)x == true) == true;
    }
}

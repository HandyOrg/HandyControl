//referenced from https://stackoverflow.com/a/5650367/9639378

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace HandyControl.Tools.Converter;

public class BorderCircularClipConverter : IMultiValueConverter
{
    private static readonly Geometry Empty = new StreamGeometry();

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [Rect rect, CornerRadius radius])
        {
            return AvaloniaProperty.UnsetValue;
        }

        if (rect.Width < double.Epsilon || rect.Height < double.Epsilon)
        {
            return Empty;
        }

        return new RectangleGeometry(new Rect(0, 0, rect.Width, rect.Height), radius.TopLeft, radius.TopLeft);
    }
}

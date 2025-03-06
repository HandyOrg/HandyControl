using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace HandyControl.Tools.Converter;

public class BorderClipConverter : IMultiValueConverter
{
    private static readonly Geometry Empty = new StreamGeometry();

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is not [double width, double height, CornerRadius radius])
        {
            return AvaloniaProperty.UnsetValue;
        }

        if (width < double.Epsilon || height < double.Epsilon)
        {
            return Empty;
        }

        return new PathGeometry
        {
            Figures = new PathFigures
            {
                new()
                {
                    StartPoint = new Point(radius.TopLeft, 0),
                    Segments = CreateSegments(width, height, radius),
                },
            }
        };
    }

    private static PathSegments CreateSegments(double width, double height, CornerRadius radius)
    {
        return new PathSegments
        {
            new LineSegment
            {
                Point = new Point(width - radius.TopRight, 0),
            },
            new ArcSegment
            {
                Point = new Point(width, radius.TopRight),
                Size = new Size(radius.TopRight, radius.TopRight),
                RotationAngle = 90,
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise,
            },
            new LineSegment
            {
                Point = new Point(width, height - radius.BottomRight),
            },
            new ArcSegment
            {
                Point = new Point(width - radius.BottomRight, height),
                Size = new Size(radius.BottomRight, radius.BottomRight),
                RotationAngle = 90,
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise,
            },
            new LineSegment
            {
                Point = new Point(radius.BottomLeft, height),
            },
            new ArcSegment
            {
                Point = new Point(0, height - radius.BottomLeft),
                Size = new Size(radius.BottomLeft, radius.BottomLeft),
                RotationAngle = 90,
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise,
            },
            new LineSegment
            {
                Point = new Point(0, radius.TopLeft),
            },
            new ArcSegment
            {
                Point = new Point(radius.TopLeft, 0),
                Size = new Size(radius.TopLeft, radius.TopLeft),
                RotationAngle = 90,
                IsLargeArc = false,
                SweepDirection = SweepDirection.Clockwise,
            },
        };
    }
}

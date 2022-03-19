using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Expression.Drawing;

internal class SimpleSegment
{
    public enum SegmentType
    {
        Line,

        CubicBeizer
    }


    private SimpleSegment()
    {
    }

    public Point[] Points { get; private set; }

    public SegmentType Type { get; private set; }

    public static SimpleSegment Create(Point point0, Point point1)
    {
        var segment = new SimpleSegment
        {
            Type = SegmentType.Line,
            Points = new[] { point0, point1 }
        };
        return segment;
    }

    public static SimpleSegment Create(Point point0, Point point1, Point point2)
    {
        var point = GeometryHelper.Lerp(point0, point1, 0.66666666666666663);
        var point3 = GeometryHelper.Lerp(point1, point2, 0.33333333333333331);
        var segment = new SimpleSegment
        {
            Type = SegmentType.CubicBeizer,
            Points = new[] { point0, point, point3, point2 }
        };
        return segment;
    }

    public static SimpleSegment Create(Point point0, Point point1, Point point2, Point point3)
    {
        var segment = new SimpleSegment
        {
            Type = SegmentType.CubicBeizer,
            Points = new[] { point0, point1, point2, point3 }
        };
        return segment;
    }

    public void Flatten(IList<Point> resultPolyline, double tolerance, IList<double> resultParameters)
    {
        switch (Type)
        {
            case SegmentType.Line:
                resultPolyline.Add(Points[1]);
                if (resultParameters == null) break;
                resultParameters.Add(1.0);
                return;

            case SegmentType.CubicBeizer:
                BezierCurveFlattener.FlattenCubic(Points, tolerance, resultPolyline, true,
                    resultParameters);
                break;

            default:
                return;
        }
    }
}

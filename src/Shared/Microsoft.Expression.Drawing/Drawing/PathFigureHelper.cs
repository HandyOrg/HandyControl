using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Expression.Drawing;

internal static class PathFigureHelper
{
    public static IEnumerable<PathSegmentData> AllSegments(this PathFigure figure)
    {
        if (figure != null && figure.Segments.Count > 0)
        {
            var startPoint = figure.StartPoint;
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = figure.Segments.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var lastPoint = current.GetLastPoint();
                yield return new PathSegmentData(startPoint, current);
                startPoint = lastPoint;
            }
        }
    }

    internal static void ApplyTransform(this PathFigure figure, GeneralTransform transform)
    {
        figure.StartPoint = transform.Transform(figure.StartPoint);
        for (var i = 0; i < figure.Segments.Count; i++)
        {
            var segment =
                PathSegmentHelper.ApplyTransform(figure.Segments[i], figure.StartPoint, transform);
            if (!Equals(figure.Segments[i], segment)) figure.Segments[i] = segment;
        }
    }

    internal static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance,
        bool removeRepeat)
    {
        if (figure == null) throw new ArgumentNullException(nameof(figure));
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (tolerance < 0.0) throw new ArgumentOutOfRangeException(nameof(tolerance));
        var list = removeRepeat ? new List<Point>() : points;
        list.Add(figure.StartPoint);
        foreach (var data in figure.AllSegments())
            data.PathSegment.FlattenSegment(list, data.StartPoint, tolerance);
        if (figure.IsClosed) list.Add(figure.StartPoint);
        if (removeRepeat && list.Count > 0)
        {
            points.Add(list[0]);
            for (var i = 1; i < list.Count; i++)
                if (!MathHelper.IsVerySmall(GeometryHelper.SquaredDistance(points.Last(), list[i])))
                    points.Add(list[i]);
        }
    }

    internal static bool SyncEllipseFigure(PathFigure figure, Rect bounds, SweepDirection sweepDirection,
        bool isFilled = true)
    {
        var flag = false;
        var pointArray = new Point[2];
        var size = new Size(bounds.Width / 2.0, bounds.Height / 2.0);
        var point = bounds.Center();
        if (size.Width > size.Height)
        {
            pointArray[0] = new Point(bounds.Left, point.Y);
            pointArray[1] = new Point(bounds.Right, point.Y);
        }
        else
        {
            pointArray[0] = new Point(point.X, bounds.Top);
            pointArray[1] = new Point(point.X, bounds.Bottom);
        }

        flag |= figure.SetIfDifferent(PathFigure.IsClosedProperty, true);
        flag |= figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
        flag |= figure.SetIfDifferent(PathFigure.StartPointProperty, pointArray[0]);
        flag |= figure.Segments.EnsureListCount(2, () => new ArcSegment());
        flag |= GeometryHelper.EnsureSegmentType(out var segment, figure.Segments, 0, () => new ArcSegment());
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty, pointArray[1]);
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty, size);
        flag |= segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
        flag |= segment.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
        flag |= GeometryHelper.EnsureSegmentType(out segment, figure.Segments, 1, () => new ArcSegment());
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty, pointArray[0]);
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty, size);
        flag |= segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
        return flag | segment.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
    }

    internal static bool SyncPolylineFigure(PathFigure figure, IList<Point> points, bool isClosed,
        bool isFilled = true)
    {
        if (figure == null) throw new ArgumentNullException(nameof(figure));
        var flag = false;
        if (points == null || points.Count == 0)
        {
            flag |= figure.ClearIfSet(PathFigure.StartPointProperty);
            flag |= figure.Segments.EnsureListCount(0);
        }
        else
        {
            flag |= figure.SetIfDifferent(PathFigure.StartPointProperty, points[0]);
            flag |= figure.Segments.EnsureListCount(1, () => new PolyLineSegment());
            flag |= PathSegmentHelper.SyncPolylineSegment(figure.Segments, 0, points, 1,
                points.Count - 1);
        }

        flag |= figure.SetIfDifferent(PathFigure.IsClosedProperty, isClosed);
        return flag | figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
    }
}

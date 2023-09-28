using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using HandyControl.Expression.Drawing;

namespace HandyControl.Expression.Media;

internal class ArcGeometrySource : GeometrySource<IArcGeometrySourceParameters>
{
    private double _absoluteThickness;

    private double _relativeThickness;

    private static bool AreCloseEnough(double angleA, double angleB)
    {
        return Math.Abs(angleA - angleB) < 0.001;
    }

    internal static double[] ComputeAngleRanges(double radiusX, double radiusY, double intersect,
        double start, double end)
    {
        var values = new List<double>
        {
            start,
            end,
            intersect,
            180.0 - intersect,
            180.0 + intersect,
            360.0 - intersect,
            360.0 + intersect,
            540.0 - intersect,
            540.0 + intersect,
            720.0 - intersect
        };
        values.Sort();
        var index = values.IndexOf(start);
        var num2 = values.IndexOf(end);
        if (num2 == index)
        {
            num2++;
        }
        else if (start < end)
        {
            IncreaseDuplicatedIndex(values, ref index);
            DecreaseDuplicatedIndex(values, ref num2);
        }
        else if (start > end)
        {
            DecreaseDuplicatedIndex(values, ref index);
            IncreaseDuplicatedIndex(values, ref num2);
        }

        var list = new List<double>();
        if (index < num2)
            for (var i = index; i <= num2; i++)
                list.Add(values[i]);
        else
            for (var j = index; j >= num2; j--)
                list.Add(values[j]);
        var num5 = EnsureFirstQuadrant((list[0] + list[1]) / 2.0);
        if (radiusX < radiusY && num5 < intersect || radiusX > radiusY && num5 > intersect)
            list.RemoveAt(0);
        if (list.Count % 2 == 1) list.RemoveLast();
        if (list.Count == 0)
        {
            var num6 = Math.Min(index, num2) - 1;
            if (num6 < 0) num6 = Math.Max(index, num2) + 1;
            list.Add(values[num6]);
            list.Add(values[num6]);
        }

        return list.ToArray();
    }

    protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
    {
        return GeometryHelper.GetStretchBound(base.ComputeLogicalBounds(layoutBounds, parameters),
            parameters.Stretch, new Size(1.0, 1.0));
    }

    private static IList<Point> ComputeOneInnerCurve(double start, double end, Rect bounds, double offset)
    {
        var num = bounds.Width / 2.0;
        var num2 = bounds.Height / 2.0;
        var point = bounds.Center();
        start = start * 3.1415926535897931 / 180.0;
        end = end * 3.1415926535897931 / 180.0;
        var num3 = 0.17453292519943295;
        var num4 = (int) Math.Ceiling(Math.Abs(end - start) / num3);
        num4 = Math.Max(2, num4);
        var list = new List<Point>(num4);
        var list2 = new List<Vector>(num4);
        var lhs = new Point();
        var item = new Point();
        var vector = new Vector();
        var vector2 = new Vector();
        var vector3 = new Vector();
        var vector4 = new Vector();
        for (var i = 0; i < num4; i++)
        {
            var a = MathHelper.Lerp(start, end, i / (double) (num4 - 1));
            var num10 = Math.Sin(a);
            var num11 = Math.Cos(a);
            lhs.X = point.X + num * num10;
            lhs.Y = point.Y - num2 * num11;
            vector.X = num * num11;
            vector.Y = num2 * num10;
            vector2.X = -num2 * num10;
            vector2.Y = num * num11;
            var d = num2 * num2 * num10 * num10 + num * num * num11 * num11;
            var num6 = Math.Sqrt(d);
            var num7 = 2.0 * num10 * num11 * (num2 * num2 - num * num);
            vector3.X = -num2 * num11;
            vector3.Y = -num * num10;
            item.X = lhs.X + offset * vector2.X / num6;
            item.Y = lhs.Y + offset * vector2.Y / num6;
            vector4.X = vector.X + offset / num6 * (vector3.X - 0.5 * vector2.X / d * num7);
            vector4.Y = vector.Y + offset / num6 * (vector3.Y - 0.5 * vector2.Y / d * num7);
            list.Add(item);
            list2.Add(-vector4.Normalized());
        }

        var list3 = new List<Point>(num4 * 3 + 1)
        {
            list[0]
        };
        for (var j = 1; j < num4; j++)
        {
            lhs = list[j - 1];
            item = list[j];
            var num13 = GeometryHelper.Distance(lhs, item) / 3.0;
            list3.Add(lhs + list2[j - 1] * num13);
            list3.Add(item - list2[j] * num13);
            list3.Add(item);
        }

        return list3;
    }

    private static void DecreaseDuplicatedIndex(IList<double> values, ref int index)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        while (index > 0 && values[index] == values[index - 1]) index--;
    }

    internal static double EnsureFirstQuadrant(double angle)
    {
        angle = Math.Abs(angle % 180.0);
        if (angle <= 90.0) return angle;
        return 180.0 - angle;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Size GetArcSize(Rect bound)
    {
        return new(bound.Width / 2.0, bound.Height / 2.0);
    }

    private static void IncreaseDuplicatedIndex(IList<double> values, ref int index)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        while (index < values.Count - 1 && values[index] == values[index + 1]) index++;
    }

    internal static double InnerCurveSelfIntersect(double radiusX, double radiusY, double thickness)
    {
        var angleA = 0.0;
        var angleB = 1.5707963267948966;
        var flag = radiusX <= radiusY;
        var vector = new Vector();
        while (!AreCloseEnough(angleA, angleB))
        {
            var d = (angleA + angleB) / 2.0;
            var num4 = Math.Cos(d);
            var num5 = Math.Sin(d);
            vector.X = radiusY * num5;
            vector.Y = radiusX * num4;
            vector.Normalize();
            if (flag)
            {
                var num6 = radiusX * num5 - vector.X * thickness;
                if (num6 > 0.0)
                    angleB = d;
                else if (num6 < 0.0) angleA = d;
            }
            else
            {
                var num7 = radiusY * num4 - vector.Y * thickness;
                if (num7 < 0.0)
                    angleB = d;
                else if (num7 > 0.0) angleA = d;
            }
        }

        var num8 = (angleA + angleB) / 2.0;
        if (AreCloseEnough(num8, 0.0)) return 0.0;
        if (!AreCloseEnough(num8, 1.5707963267948966)) return num8 * 180.0 / 3.1415926535897931;
        return 90.0;
    }

    private static double NormalizeAngle(double degree)
    {
        if (degree < 0.0 || degree > 360.0)
        {
            degree = degree % 360.0;
            if (degree < 0.0) degree += 360.0;
        }

        return degree;
    }

    private void NormalizeThickness(IArcGeometrySourceParameters parameters)
    {
        var num = LogicalBounds.Width / 2.0;
        var num2 = LogicalBounds.Height / 2.0;
        var rhs = Math.Min(num, num2);
        var arcThickness = parameters.ArcThickness;
        if (parameters.ArcThicknessUnit == UnitType.Pixel)
            arcThickness = MathHelper.SafeDivide(arcThickness, rhs, 0.0);
        _relativeThickness = MathHelper.EnsureRange(arcThickness, 0.0, 1.0);
        _absoluteThickness = rhs * _relativeThickness;
    }

    private bool SyncPieceWiseInnerCurves(PathFigure figure, int index, ref Point firstPoint,
        params double[] angles)
    {
        var flag = false;
        var length = angles.Length;
        var logicalBounds = LogicalBounds;
        var absoluteThickness = _absoluteThickness;
        flag |= figure.Segments.EnsureListCount(index + length / 2, () => new PolyBezierSegment());
        for (var i = 0; i < length / 2; i++)
        {
            var points = ComputeOneInnerCurve(angles[i * 2], angles[i * 2 + 1], logicalBounds,
                absoluteThickness);
            if (i == 0) firstPoint = points[0];
            flag |= PathSegmentHelper.SyncPolyBezierSegment(figure.Segments, index + i, points, 1,
                points.Count - 1);
        }

        return flag;
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    protected override bool UpdateCachedGeometry(IArcGeometrySourceParameters parameters)
    {
        var flag = false;
        NormalizeThickness(parameters);
        var relativeMode = parameters.ArcThicknessUnit == UnitType.Percent;
        var flag3 = MathHelper.AreClose(parameters.StartAngle, parameters.EndAngle);
        var angle = NormalizeAngle(parameters.StartAngle);
        var end = NormalizeAngle(parameters.EndAngle);
        if (end < angle) end += 360.0;
        var isFilled = _relativeThickness == 1.0;
        var flag5 = _relativeThickness == 0.0;
        if (flag3) return flag | UpdateZeroAngleGeometry(relativeMode, angle);
        if (MathHelper.IsVerySmall((end - angle) % 360.0))
        {
            if (flag5 || isFilled) return flag | UpdateEllipseGeometry(isFilled);
            return flag | UpdateFullRingGeometry(relativeMode);
        }

        if (isFilled) return flag | UpdatePieGeometry(angle, end);
        if (flag5) return flag | UpdateOpenArcGeometry(angle, end);
        return flag | UpdateRingArcGeometry(relativeMode, angle, end);
    }

    private bool UpdateEllipseGeometry(bool isFilled)
    {
        var flag = false;
        var y = MathHelper.Lerp(LogicalBounds.Top, LogicalBounds.Bottom, 0.5);
        var point = new Point(LogicalBounds.Left, y);
        var point2 = new Point(LogicalBounds.Right, y);
        flag |= GeometryHelper.EnsureGeometryType(out var geometry, ref CachedGeometry,
            () => new PathGeometry());
        flag |= geometry.Figures.EnsureListCount(1, () => new PathFigure());
        var dependencyObject = geometry.Figures[0];
        flag |= dependencyObject.SetIfDifferent(PathFigure.IsClosedProperty, true);
        flag |= dependencyObject.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
        flag |= dependencyObject.Segments.EnsureListCount(2, () => new ArcSegment());
        flag |= dependencyObject.SetIfDifferent(PathFigure.StartPointProperty, point);
        flag |= GeometryHelper.EnsureSegmentType(out var segment, dependencyObject.Segments, 0,
            () => new ArcSegment());
        flag |= GeometryHelper.EnsureSegmentType(out var segment2, dependencyObject.Segments, 1,
            () => new ArcSegment());
        var size = new Size(LogicalBounds.Width / 2.0, LogicalBounds.Height / 2.0);
        flag |= segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty, size);
        flag |= segment.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty, point2);
        flag |= segment2.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
        flag |= segment2.SetIfDifferent(ArcSegment.SizeProperty, size);
        flag |= segment2.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
        return flag | segment2.SetIfDifferent(ArcSegment.PointProperty, point);
    }

    private bool UpdateFullRingGeometry(bool relativeMode)
    {
        var flag = false;
        flag |= GeometryHelper.EnsureGeometryType(out var geometry, ref CachedGeometry,
            () => new PathGeometry());
        flag |= geometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.EvenOdd);
        flag |= geometry.Figures.EnsureListCount(2, () => new PathFigure());
        flag |= PathFigureHelper.SyncEllipseFigure(geometry.Figures[0], LogicalBounds,
            SweepDirection.Clockwise);
        var logicalBounds = LogicalBounds;
        var num = logicalBounds.Width / 2.0;
        var num2 = logicalBounds.Height / 2.0;
        if (relativeMode || MathHelper.AreClose(num, num2))
        {
            var bounds = LogicalBounds.Resize(1.0 - _relativeThickness);
            return flag | PathFigureHelper.SyncEllipseFigure(geometry.Figures[1], bounds,
                SweepDirection.Counterclockwise);
        }

        flag |= geometry.Figures[1].SetIfDifferent(PathFigure.IsClosedProperty, true);
        flag |= geometry.Figures[1].SetIfDifferent(PathFigure.IsFilledProperty, true);
        var firstPoint = new Point();
        var intersect = InnerCurveSelfIntersect(num, num2, _absoluteThickness);
        var angles = ComputeAngleRanges(num, num2, intersect, 360.0, 0.0);
        flag |= SyncPieceWiseInnerCurves(geometry.Figures[1], 0, ref firstPoint, angles);
        return flag | geometry.Figures[1].SetIfDifferent(PathFigure.StartPointProperty, firstPoint);
    }

    private bool UpdateOpenArcGeometry(double start, double end)
    {
        PathFigure figure;
        ArcSegment segment;
        var flag = false;
        var cachedGeometry = CachedGeometry as PathGeometry;
        if (cachedGeometry == null || cachedGeometry.Figures.Count != 1 ||
            (figure = cachedGeometry.Figures[0]).Segments.Count != 1 ||
            (segment = figure.Segments[0] as ArcSegment) == null)
        {
            figure = new PathFigure();
            CachedGeometry = new PathGeometry
            {
                Figures = { figure }
            };
            figure.Segments.Add(segment = new ArcSegment());
            figure.IsClosed = false;
            segment.SweepDirection = SweepDirection.Clockwise;
            flag = true;
        }

        flag |= figure.SetIfDifferent(PathFigure.StartPointProperty,
            GeometryHelper.GetArcPoint(start, LogicalBounds));
        flag |= figure.SetIfDifferent(PathFigure.IsFilledProperty, false);
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty,
            GeometryHelper.GetArcPoint(end, LogicalBounds));
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty, GetArcSize(LogicalBounds));
        return flag | segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
    }

    private bool UpdatePieGeometry(double start, double end)
    {
        PathFigure figure;
        ArcSegment segment;
        LineSegment segment2;
        var flag = false;
        var cachedGeometry = CachedGeometry as PathGeometry;
        if (cachedGeometry == null || cachedGeometry.Figures.Count != 1 ||
            (figure = cachedGeometry.Figures[0]).Segments.Count != 2 ||
            (segment = figure.Segments[0] as ArcSegment) == null ||
            (segment2 = figure.Segments[1] as LineSegment) == null)
        {
            figure = new PathFigure();
            CachedGeometry = new PathGeometry
            {
                Figures = { figure }
            };
            figure.Segments.Add(segment = new ArcSegment());
            figure.Segments.Add(segment2 = new LineSegment());
            figure.IsClosed = true;
            segment.SweepDirection = SweepDirection.Clockwise;
            flag = true;
        }

        flag |= figure.SetIfDifferent(PathFigure.StartPointProperty,
            GeometryHelper.GetArcPoint(start, LogicalBounds));
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty,
            GeometryHelper.GetArcPoint(end, LogicalBounds));
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty, GetArcSize(LogicalBounds));
        flag |= segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
        return flag | segment2.SetIfDifferent(LineSegment.PointProperty, LogicalBounds.Center());
    }

    private bool UpdateRingArcGeometry(bool relativeMode, double start, double end)
    {
        var flag = false;
        flag |= GeometryHelper.EnsureGeometryType(out var geometry, ref CachedGeometry,
            () => new PathGeometry());
        flag |= geometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.Nonzero);
        flag |= geometry.Figures.EnsureListCount(1, () => new PathFigure());
        var dependencyObject = geometry.Figures[0];
        flag |= dependencyObject.SetIfDifferent(PathFigure.IsClosedProperty, true);
        flag |= dependencyObject.SetIfDifferent(PathFigure.IsFilledProperty, true);
        flag |= dependencyObject.SetIfDifferent(PathFigure.StartPointProperty,
            GeometryHelper.GetArcPoint(start, LogicalBounds));
        flag |= dependencyObject.Segments.EnsureListCountAtLeast(3, () => new ArcSegment());
        flag |= GeometryHelper.EnsureSegmentType(out var segment, dependencyObject.Segments, 0,
            () => new ArcSegment());
        flag |= segment.SetIfDifferent(ArcSegment.PointProperty,
            GeometryHelper.GetArcPoint(end, LogicalBounds));
        flag |= segment.SetIfDifferent(ArcSegment.SizeProperty,
            new Size(LogicalBounds.Width / 2.0, LogicalBounds.Height / 2.0));
        flag |= segment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
        flag |= segment.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
        flag |= GeometryHelper.EnsureSegmentType(out var segment2, dependencyObject.Segments, 1,
            () => new LineSegment());
        var logicalBounds = LogicalBounds;
        var num = logicalBounds.Width / 2.0;
        var num2 = logicalBounds.Height / 2.0;
        if (relativeMode || MathHelper.AreClose(num, num2))
        {
            var bound = LogicalBounds.Resize(1.0 - _relativeThickness);
            flag |= segment2.SetIfDifferent(LineSegment.PointProperty,
                GeometryHelper.GetArcPoint(end, bound));
            flag |= dependencyObject.Segments.EnsureListCount(3, () => new ArcSegment());
            flag |= GeometryHelper.EnsureSegmentType(out var segment3, dependencyObject.Segments, 2,
                () => new ArcSegment());
            flag |= segment3.SetIfDifferent(ArcSegment.PointProperty,
                GeometryHelper.GetArcPoint(start, bound));
            flag |= segment3.SetIfDifferent(ArcSegment.SizeProperty, GetArcSize(bound));
            flag |= segment3.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180.0);
            return flag | segment3.SetIfDifferent(ArcSegment.SweepDirectionProperty,
                SweepDirection.Counterclockwise);
        }

        var firstPoint = new Point();
        var intersect = InnerCurveSelfIntersect(num, num2, _absoluteThickness);
        var angles = ComputeAngleRanges(num, num2, intersect, end, start);
        flag |= SyncPieceWiseInnerCurves(dependencyObject, 2, ref firstPoint, angles);
        return flag | segment2.SetIfDifferent(LineSegment.PointProperty, firstPoint);
    }

    private bool UpdateZeroAngleGeometry(bool relativeMode, double angle)
    {
        Point point2;
        var flag = false;
        var arcPoint = GeometryHelper.GetArcPoint(angle, LogicalBounds);
        var logicalBounds = LogicalBounds;
        var num = logicalBounds.Width / 2.0;
        var num2 = logicalBounds.Height / 2.0;
        if (relativeMode || MathHelper.AreClose(num, num2))
        {
            var bound = LogicalBounds.Resize(1.0 - _relativeThickness);
            point2 = GeometryHelper.GetArcPoint(angle, bound);
        }
        else
        {
            var intersect = InnerCurveSelfIntersect(num, num2, _absoluteThickness);
            var numArray = ComputeAngleRanges(num, num2, intersect, angle, angle);
            var a = numArray[0] * 3.1415926535897931 / 180.0;
            var vector = new Vector(num2 * Math.Sin(a), -num * Math.Cos(a));
            point2 = GeometryHelper.GetArcPoint(numArray[0], LogicalBounds) -
                     vector.Normalized() * _absoluteThickness;
        }

        flag |= GeometryHelper.EnsureGeometryType(out var geometry, ref CachedGeometry,
            () => new LineGeometry());
        flag |= geometry.SetIfDifferent(LineGeometry.StartPointProperty, arcPoint);
        return flag | geometry.SetIfDifferent(LineGeometry.EndPointProperty, point2);
    }
}

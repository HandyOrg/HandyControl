using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Expression.Drawing;

internal static class PathSegmentHelper
{
    public static PathSegment ApplyTransform(PathSegment segment, Point start, GeneralTransform transform)
    {
        return PathSegmentImplementation.Create(segment, start).ApplyTransform(transform);
    }

    public static PathSegment ArcToBezierSegments(ArcSegment arcSegment, Point startPoint)
    {
        var isStroked = arcSegment.IsStroked();
        ArcToBezierHelper.ArcToBezier(startPoint.X, startPoint.Y, arcSegment.Size.Width,
            arcSegment.Size.Height, arcSegment.RotationAngle, arcSegment.IsLargeArc,
            arcSegment.SweepDirection == SweepDirection.Clockwise, arcSegment.Point.X, arcSegment.Point.Y,
            out var pointArray, out var num);
        return num switch
        {
            -1 => null,
            0 => CreateLineSegment(arcSegment.Point, isStroked),
            1 => CreateBezierSegment(pointArray[0], pointArray[1], pointArray[2], isStroked),
            _ => CreatePolyBezierSegment(pointArray, 0, num * 3, isStroked)
        };
    }

    public static ArcSegment CreateArcSegment(Point point, Size size, bool isLargeArc, bool clockwise,
        double rotationAngle = 0.0, bool isStroked = true)
    {
        var dependencyObject = new ArcSegment();
        dependencyObject.SetIfDifferent(ArcSegment.PointProperty, point);
        dependencyObject.SetIfDifferent(ArcSegment.SizeProperty, size);
        dependencyObject.SetIfDifferent(ArcSegment.IsLargeArcProperty, isLargeArc);
        dependencyObject.SetIfDifferent(ArcSegment.SweepDirectionProperty,
            clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise);
        dependencyObject.SetIfDifferent(ArcSegment.RotationAngleProperty, rotationAngle);
        dependencyObject.SetIsStroked(isStroked);
        return dependencyObject;
    }

    public static BezierSegment CreateBezierSegment(Point point1, Point point2, Point point3,
        bool isStroked = true)
    {
        var segment = new BezierSegment
        {
            Point1 = point1,
            Point2 = point2,
            Point3 = point3
        };
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static LineSegment CreateLineSegment(Point point, bool isStroked = true)
    {
        var segment = new LineSegment
        {
            Point = point
        };
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static PolyBezierSegment CreatePolyBezierSegment(IList<Point> points, int start, int count,
        bool isStroked = true)
    {
        if (points == null) throw new ArgumentNullException(nameof(points));
        count = count / 3 * 3;
        if (count < 0 || points.Count < start + count) throw new ArgumentOutOfRangeException(nameof(count));
        var segment = new PolyBezierSegment
        {
            Points = new PointCollection()
        };
        for (var i = 0; i < count; i++) segment.Points.Add(points[start + i]);
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static PolyLineSegment CreatePolylineSegment(IList<Point> points, int start, int count,
        bool isStroked = true)
    {
        if (count < 0 || points.Count < start + count) throw new ArgumentOutOfRangeException(nameof(count));
        var segment = new PolyLineSegment
        {
            Points = new PointCollection()
        };
        for (var i = 0; i < count; i++) segment.Points.Add(points[start + i]);
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static PolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(IList<Point> points,
        int start, int count, bool isStroked = true)
    {
        if (points == null) throw new ArgumentNullException(nameof(points));
        count = count / 2 * 2;
        if (count < 0 || points.Count < start + count) throw new ArgumentOutOfRangeException(nameof(count));
        var segment = new PolyQuadraticBezierSegment
        {
            Points = new PointCollection()
        };
        for (var i = 0; i < count; i++) segment.Points.Add(points[start + i]);
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static QuadraticBezierSegment CreateQuadraticBezierSegment(Point point1, Point point2,
        bool isStroked = true)
    {
        var segment = new QuadraticBezierSegment
        {
            Point1 = point1,
            Point2 = point2
        };
        segment.SetIsStroked(isStroked);
        return segment;
    }

    public static void FlattenSegment(this PathSegment segment, IList<Point> points, Point start,
        double tolerance)
    {
        PathSegmentImplementation.Create(segment, start).Flatten(points, tolerance);
    }

    public static Point GetLastPoint(this PathSegment segment)
    {
        return segment.GetPoint(-1);
    }

    public static Point GetPoint(this PathSegment segment, int index)
    {
        return PathSegmentImplementation.Create(segment).GetPoint(index);
    }

    public static int GetPointCount(this PathSegment segment)
    {
        if (segment is ArcSegment) return 1;
        if (segment is LineSegment) return 1;
        if (segment is QuadraticBezierSegment) return 2;
        if (segment is BezierSegment) return 3;
        if (segment is PolyLineSegment segment2) return segment2.Points.Count;
        if (segment is PolyQuadraticBezierSegment segment3) return segment3.Points.Count / 2 * 2;
        if (segment is PolyBezierSegment segment4) return segment4.Points.Count / 3 * 3;
        return 0;
    }

    public static IEnumerable<SimpleSegment> GetSimpleSegments(this PathSegment segment, Point start)
    {
        return PathSegmentImplementation.Create(segment, start).GetSimpleSegments();
    }

    public static bool IsEmpty(this PathSegment segment)
    {
        return segment.GetPointCount() == 0;
    }

    private static void SetIsStroked(this PathSegment segment, bool isStroked)
    {
        if (segment.IsStroked != isStroked) segment.IsStroked = isStroked;
    }

    public static bool SyncPolyBezierSegment(PathSegmentCollection collection, int index,
        IList<Point> points, int start, int count)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (index < 0 || index >= collection.Count) throw new ArgumentOutOfRangeException(nameof(index));
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (points.Count < start + count) throw new ArgumentOutOfRangeException(nameof(count));
        var flag = false;
        count = count / 3 * 3;
        var segment = collection[index] as PolyBezierSegment;
        if (segment == null)
        {
            collection[index] = segment = new PolyBezierSegment();
            flag = true;
        }

        segment.Points.EnsureListCount(count);
        for (var i = 0; i < count; i++)
            if (segment.Points[i] != points[i + start])
            {
                segment.Points[i] = points[i + start];
                flag = true;
            }

        return flag;
    }

    public static bool SyncPolylineSegment(PathSegmentCollection collection, int index,
        IList<Point> points, int start, int count)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (index < 0 || index >= collection.Count) throw new ArgumentOutOfRangeException(nameof(index));
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (points.Count < start + count) throw new ArgumentOutOfRangeException(nameof(count));
        var flag = false;
        var segment = collection[index] as PolyLineSegment;
        if (segment == null)
        {
            collection[index] = segment = new PolyLineSegment();
            flag = true;
        }

        flag |= segment.Points.EnsureListCount(count);
        for (var i = 0; i < count; i++)
            if (segment.Points[i] != points[i + start])
            {
                segment.Points[i] = points[i + start];
                flag = true;
            }

        return flag;
    }


    private class ArcSegmentImplementation : PathSegmentImplementation
    {
        private ArcSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            var segment = ArcToBezierSegments(_segment, Start);
            if (segment != null) return PathSegmentHelper.ApplyTransform(segment, Start, transform);
            _segment.Point = transform.Transform(_segment.Point);
            return _segment;
        }

        public static PathSegmentImplementation Create(ArcSegment source)
        {
            if (source != null) return new ArcSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            var segment = ArcToBezierSegments(_segment, Start);
            segment?.FlattenSegment(points, Start, tolerance);
        }

        public override Point GetPoint(int index)
        {
            if (index < -1 || index > 0) throw new ArgumentOutOfRangeException(nameof(index));
            return _segment.Point;
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            var segment = ArcToBezierSegments(_segment, Start);
            if (segment != null) return segment.GetSimpleSegments(Start);
            return Enumerable.Empty<SimpleSegment>();
        }
    }


    private static class ArcToBezierHelper
    {
        private static bool AcceptRadius(double rHalfChord2, double rFuzz2, ref double rRadius)
        {
            var flag = rRadius * rRadius > rHalfChord2 * rFuzz2;
            if (flag && rRadius < 0.0) rRadius = -rRadius;
            return flag;
        }

        public static void ArcToBezier(double xStart, double yStart, double xRadius, double yRadius,
            double rRotation, bool fLargeArc, bool fSweepUp, double xEnd, double yEnd, out Point[] pPt,
            out int cPieces)
        {
            var num = 1E-06;
            pPt = new Point[12];
            var num12 = num * num;
            var flag = false;
            cPieces = -1;
            var num2 = 0.5 * (xEnd - xStart);
            var num3 = 0.5 * (yEnd - yStart);
            var num4 = num2 * num2 + num3 * num3;
            if (num4 >= num12)
            {
                if (!AcceptRadius(num4, num12, ref xRadius) || !AcceptRadius(num4, num12, ref yRadius))
                {
                    cPieces = 0;
                }
                else
                {
                    double num5;
                    double num6;
                    // ReSharper disable once InlineOutVariableDeclaration
                    double num7;
                    // ReSharper disable once InlineOutVariableDeclaration
                    double num8;
                    double num9;
                    double num10;
                    Point point4;
                    if (Math.Abs(rRotation) < num)
                    {
                        num5 = 1.0;
                        num6 = 0.0;
                    }
                    else
                    {
                        rRotation = -rRotation * 3.1415926535897931 / 180.0;
                        num5 = Math.Cos(rRotation);
                        num6 = Math.Sin(rRotation);
                        var num15 = num2 * num5 - num3 * num6;
                        num3 = num2 * num6 + num3 * num5;
                        num2 = num15;
                    }

                    num2 /= xRadius;
                    num3 /= yRadius;
                    num4 = num2 * num2 + num3 * num3;
                    if (num4 > 1.0)
                    {
                        var num16 = Math.Sqrt(num4);
                        xRadius *= num16;
                        yRadius *= num16;
                        num9 = num10 = 0.0;
                        flag = true;
                        num2 /= num16;
                        num3 /= num16;
                    }
                    else
                    {
                        var num17 = Math.Sqrt((1.0 - num4) / num4);
                        if (fLargeArc != fSweepUp)
                        {
                            num9 = -num17 * num3;
                            num10 = num17 * num2;
                        }
                        else
                        {
                            num9 = num17 * num3;
                            num10 = -num17 * num2;
                        }
                    }

                    var ptStart = new Point(-num2 - num9, -num3 - num10);
                    var ptEnd = new Point(num2 - num9, num3 - num10);
                    var matrix = new Matrix(num5 * xRadius, -num6 * xRadius, num6 * yRadius,
                        num5 * yRadius, 0.5 * (xEnd + xStart), 0.5 * (yEnd + yStart));
                    if (!flag)
                    {
                        matrix.OffsetX += matrix.M11 * num9 + matrix.M21 * num10;
                        matrix.OffsetY += matrix.M12 * num9 + matrix.M22 * num10;
                    }

                    GetArcAngle(ptStart, ptEnd, fLargeArc, fSweepUp, out num7, out num8, out cPieces);
                    var bezierDistance = GetBezierDistance(num7);
                    if (!fSweepUp) bezierDistance = -bezierDistance;
                    var rhs = new Point(-bezierDistance * ptStart.Y, bezierDistance * ptStart.X);
                    var index = 0;
                    pPt = new Point[cPieces * 3];
                    for (var i = 1; i < cPieces; i++)
                    {
                        var lhs = new Point(ptStart.X * num7 - ptStart.Y * num8,
                            ptStart.X * num8 + ptStart.Y * num7);
                        point4 = new Point(-bezierDistance * lhs.Y, bezierDistance * lhs.X);
                        pPt[index++] = matrix.Transform(ptStart.Plus(rhs));
                        pPt[index++] = matrix.Transform(lhs.Minus(point4));
                        pPt[index++] = matrix.Transform(lhs);
                        ptStart = lhs;
                        rhs = point4;
                    }

                    point4 = new Point(-bezierDistance * ptEnd.Y, bezierDistance * ptEnd.X);
                    pPt[index++] = matrix.Transform(ptStart.Plus(rhs));
                    pPt[index++] = matrix.Transform(ptEnd.Minus(point4));
                    pPt[index] = new Point(xEnd, yEnd);
                }
            }
        }

        private static void GetArcAngle(Point ptStart, Point ptEnd, bool fLargeArc, bool fSweepUp,
            out double rCosArcAngle, out double rSinArcAngle, out int cPieces)
        {
            rCosArcAngle = GeometryHelper.Dot(ptStart, ptEnd);
            rSinArcAngle = GeometryHelper.Determinant(ptStart, ptEnd);
            if (rCosArcAngle >= 0.0)
            {
                if (!fLargeArc)
                {
                    cPieces = 1;
                    return;
                }

                cPieces = 4;
            }
            else if (fLargeArc)
            {
                cPieces = 3;
            }
            else
            {
                cPieces = 2;
            }

            var d = Math.Atan2(rSinArcAngle, rCosArcAngle);
            if (fSweepUp)
            {
                if (d < 0.0) d += 6.2831853071795862;
            }
            else if (d > 0.0)
            {
                d -= 6.2831853071795862;
            }

            d /= cPieces;
            rCosArcAngle = Math.Cos(d);
            rSinArcAngle = Math.Sin(d);
        }

        private static double GetBezierDistance(double rDot, double rRadius = 1.0)
        {
            var num = rRadius * rRadius;
            var num5 = 0.0;
            var d = 0.5 * (num + rDot);
            if (d < 0.0) return num5;
            var num3 = num - d;
            if (num3 <= 0.0) return num5;
            var num4 = Math.Sqrt(num3);
            var num2 = 4.0 * (rRadius - Math.Sqrt(d)) / 3.0;
            if (num2 <= num4 * 1E-06) return 0.0;
            return num2 / num4;
        }
    }


    private class BezierSegmentImplementation : PathSegmentImplementation
    {
        private BezierSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Point1 = transform.Transform(_segment.Point1);
            _segment.Point2 = transform.Transform(_segment.Point2);
            _segment.Point3 = transform.Transform(_segment.Point3);
            return _segment;
        }

        public static PathSegmentImplementation Create(BezierSegment source)
        {
            if (source != null) return new BezierSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            Point[] controlPoints = { Start, _segment.Point1, _segment.Point2, _segment.Point3 };
            var resultPolyline = new List<Point>();
            BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, resultPolyline, true);
            points.AddRange(resultPolyline);
        }

        public override Point GetPoint(int index)
        {
            if (index < -1 || index > 2) throw new ArgumentOutOfRangeException(nameof(index));
            if (index == 0) return _segment.Point1;
            if (index == 1) return _segment.Point2;
            return _segment.Point3;
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            yield return SimpleSegment.Create(Start, _segment.Point1, _segment.Point2, _segment.Point3);
        }
    }


    private class LineSegmentImplementation : PathSegmentImplementation
    {
        private LineSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Point = transform.Transform(_segment.Point);
            return _segment;
        }

        public static PathSegmentImplementation Create(LineSegment source)
        {
            if (source != null) return new LineSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            points.Add(_segment.Point);
        }

        public override Point GetPoint(int index)
        {
            if (index < -1 || index > 0) throw new ArgumentOutOfRangeException(nameof(index));
            return _segment.Point;
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            yield return SimpleSegment.Create(Start, _segment.Point);
        }
    }


    private abstract class PathSegmentImplementation
    {
        protected Point Start { get; private set; }

        public abstract PathSegment ApplyTransform(GeneralTransform transform);

        public static PathSegmentImplementation Create(PathSegment segment)
        {
            PathSegmentImplementation implementation;
            if ((implementation = BezierSegmentImplementation.Create(segment as BezierSegment)) == null &&
                (implementation = LineSegmentImplementation.Create(segment as LineSegment)) == null &&
                (implementation = ArcSegmentImplementation.Create(segment as ArcSegment)) == null &&
                (implementation = PolyLineSegmentImplementation.Create(segment as PolyLineSegment)) ==
                null &&
                (implementation = PolyBezierSegmentImplementation.Create(segment as PolyBezierSegment)) ==
                null &&
                (implementation =
                    QuadraticBezierSegmentImplementation.Create(segment as QuadraticBezierSegment)) ==
                null &&
                (implementation =
                    PolyQuadraticBezierSegmentImplementation.Create(segment as PolyQuadraticBezierSegment)
                ) == null)
                throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.TypeNotSupported, new object[] { segment.GetType().FullName }));
            return implementation;
        }

        public static PathSegmentImplementation Create(PathSegment segment, Point start)
        {
            var implementation = Create(segment);
            implementation.Start = start;
            return implementation;
        }

        public abstract void Flatten(IList<Point> points, double tolerance);

        public abstract Point GetPoint(int index);

        public abstract IEnumerable<SimpleSegment> GetSimpleSegments();
    }


    private class PolyBezierSegmentImplementation : PathSegmentImplementation
    {
        private PolyBezierSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Points.ApplyTransform(transform);
            return _segment;
        }

        public static PathSegmentImplementation Create(PolyBezierSegment source)
        {
            if (source != null) return new PolyBezierSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            var start = Start;
            var num = _segment.Points.Count / 3 * 3;
            for (var i = 0; i < num; i += 3)
            {
                Point[] controlPoints = { start, _segment.Points[i], _segment.Points[i + 1], _segment.Points[i + 2] };
                var resultPolyline = new List<Point>();
                BezierCurveFlattener.FlattenCubic(controlPoints, tolerance, resultPolyline, true);
                points.AddRange(resultPolyline);
                start = _segment.Points[i + 2];
            }
        }

        public override Point GetPoint(int index)
        {
            var num = _segment.Points.Count / 3 * 3;
            if (index < -1 || index > num - 1) throw new ArgumentOutOfRangeException(nameof(index));
            if (index != -1) return _segment.Points[index];
            return _segment.Points[num - 1];
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            var start = Start;
            IList<Point> points = _segment.Points;
            var iteratorVariable2 = _segment.Points.Count / 3;
            var iteratorVariable3 = 0;
            while (true)
            {
                if (iteratorVariable3 >= iteratorVariable2) yield break;
                var iteratorVariable4 = iteratorVariable3 * 3;
                yield return SimpleSegment.Create(start, points[iteratorVariable4],
                    points[iteratorVariable4 + 1], points[iteratorVariable4 + 2]);
                start = points[iteratorVariable4 + 2];
                iteratorVariable3++;
            }
        }
    }


    private class PolyLineSegmentImplementation : PathSegmentImplementation
    {
        private PolyLineSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Points.ApplyTransform(transform);
            return _segment;
        }

        public static PathSegmentImplementation Create(PolyLineSegment source)
        {
            if (source != null) return new PolyLineSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            points.AddRange(_segment.Points);
        }

        public override Point GetPoint(int index)
        {
            if (index < -1 || index > _segment.Points.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index != -1) return _segment.Points[index];
            return _segment.Points.Last();
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            var start = Start;
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = _segment.Points.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                yield return SimpleSegment.Create(start, current);
                start = current;
            }
        }
    }


    private class PolyQuadraticBezierSegmentImplementation : PathSegmentImplementation
    {
        private PolyQuadraticBezierSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Points.ApplyTransform(transform);
            return _segment;
        }

        public static PathSegmentImplementation Create(PolyQuadraticBezierSegment source)
        {
            if (source != null) return new PolyQuadraticBezierSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            var start = Start;
            var num = _segment.Points.Count / 2 * 2;
            for (var i = 0; i < num; i += 2)
            {
                Point[] controlPoints = { start, _segment.Points[i], _segment.Points[i + 1] };
                var resultPolyline = new List<Point>();
                BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, resultPolyline, true);
                points.AddRange(resultPolyline);
                start = _segment.Points[i + 1];
            }
        }

        public override Point GetPoint(int index)
        {
            var num = _segment.Points.Count / 2 * 2;
            if (index < -1 || index > num - 1) throw new ArgumentOutOfRangeException(nameof(index));
            if (index != -1) return _segment.Points[index];
            return _segment.Points[num - 1];
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            var start = Start;
            IList<Point> points = _segment.Points;
            var iteratorVariable2 = _segment.Points.Count / 2;
            var iteratorVariable3 = 0;
            while (true)
            {
                if (iteratorVariable3 >= iteratorVariable2) yield break;
                var iteratorVariable4 = iteratorVariable3 * 2;
                yield return SimpleSegment.Create(start, points[iteratorVariable4],
                    points[iteratorVariable4 + 1]);
                start = points[iteratorVariable4 + 1];
                iteratorVariable3++;
            }
        }
    }


    private class QuadraticBezierSegmentImplementation : PathSegmentImplementation
    {
        private QuadraticBezierSegment _segment;

        public override PathSegment ApplyTransform(GeneralTransform transform)
        {
            _segment.Point1 = transform.Transform(_segment.Point1);
            _segment.Point2 = transform.Transform(_segment.Point2);
            return _segment;
        }

        public static PathSegmentImplementation Create(QuadraticBezierSegment source)
        {
            if (source != null) return new QuadraticBezierSegmentImplementation { _segment = source };
            return null;
        }

        public override void Flatten(IList<Point> points, double tolerance)
        {
            Point[] controlPoints = { Start, _segment.Point1, _segment.Point2 };
            var resultPolyline = new List<Point>();
            BezierCurveFlattener.FlattenQuadratic(controlPoints, tolerance, resultPolyline, true);
            points.AddRange(resultPolyline);
        }

        public override Point GetPoint(int index)
        {
            if (index < -1 || index > 1) throw new ArgumentOutOfRangeException(nameof(index));
            return index == 0 ? _segment.Point1 : _segment.Point2;
        }

        public override IEnumerable<SimpleSegment> GetSimpleSegments()
        {
            yield return SimpleSegment.Create(Start, _segment.Point1, _segment.Point2);
        }
    }
}

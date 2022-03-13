using System;
using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Expression.Drawing;

internal static class BezierCurveFlattener
{
    public const double StandardFlatteningTolerance = 0.25;

    private static void DoCubicForwardDifferencing(Point[] controlPoints, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
    {
        double num14;
        var num = controlPoints[1].X - controlPoints[0].X;
        var num2 = controlPoints[1].Y - controlPoints[0].Y;
        var num3 = controlPoints[2].X - controlPoints[1].X;
        var num4 = controlPoints[2].Y - controlPoints[1].Y;
        var num5 = controlPoints[3].X - controlPoints[2].X;
        var num6 = controlPoints[3].Y - controlPoints[2].Y;
        var num7 = num3 - num;
        var num8 = num4 - num2;
        var num9 = num5 - num3;
        var num10 = num6 - num4;
        var num11 = num9 - num7;
        var num12 = num10 - num8;
        var vector = controlPoints[3].Subtract(controlPoints[0]);
        var length = vector.Length;
        if (!MathHelper.IsVerySmall(length))
            num14 = Math.Max(0.0,
                Math.Max(Math.Abs((num7 * vector.Y - num8 * vector.X) / length),
                    Math.Abs((num9 * vector.Y - num10 * vector.X) / length)));
        else
            num14 = Math.Max(0.0,
                Math.Max(GeometryHelper.Distance(controlPoints[1], controlPoints[0]),
                    GeometryHelper.Distance(controlPoints[2], controlPoints[0])));
        uint num15 = 0;
        if (num14 > 0.0)
        {
            var d = num14 * inverseErrorTolerance;
            num15 = d < 2147483647.0 ? Log4UnsignedInt32((uint) (d + 0.5)) : Log4Double(d);
        }

        var exp = (int) -num15;
        var num18 = exp + exp;
        var num19 = num18 + exp;
        var num20 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num7, num18);
        var num21 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num8, num18);
        var num22 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num11, num19);
        var num23 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num12, num19);
        var num24 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num, exp) + num20 +
                    0.16666666666666666 * num22;
        var num25 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num2, exp) + num21 +
                    0.16666666666666666 * num23;
        var num26 = 2.0 * num20 + num22;
        var num27 = 2.0 * num21 + num23;
        var x = controlPoints[0].X;
        var y = controlPoints[0].Y;
        var item = new Point(0.0, 0.0);
        var num30 = 1 << (int) num15;
        var num31 = num30 > 0 ? (rightParameter - leftParameter) / num30 : 0.0;
        var num32 = leftParameter;
        for (var i = 1; i < num30; i++)
        {
            x += num24;
            y += num25;
            item.X = x;
            item.Y = y;
            resultPolyline.Add(item);
            num32 += num31;
            resultParameters?.Add(num32);
            num24 += num26;
            num25 += num27;
            num26 += num22;
            num27 += num23;
        }
    }

    private static void DoCubicMidpointSubdivision(Point[] controlPoints, uint depth, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
    {
        Point[] pointArray = { controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3] };
        var pointArray2 = new Point[4];
        pointArray2[3] = pointArray[3];
        pointArray[3] = GeometryHelper.Midpoint(pointArray[3], pointArray[2]);
        pointArray[2] = GeometryHelper.Midpoint(pointArray[2], pointArray[1]);
        pointArray[1] = GeometryHelper.Midpoint(pointArray[1], pointArray[0]);
        pointArray2[2] = pointArray[3];
        pointArray[3] = GeometryHelper.Midpoint(pointArray[3], pointArray[2]);
        pointArray[2] = GeometryHelper.Midpoint(pointArray[2], pointArray[1]);
        pointArray2[1] = pointArray[3];
        pointArray[3] = GeometryHelper.Midpoint(pointArray[3], pointArray[2]);
        pointArray2[0] = pointArray[3];
        depth--;
        var num = (leftParameter + rightParameter) * 0.5;
        if (depth > 0)
        {
            DoCubicMidpointSubdivision(pointArray, depth, leftParameter, num, inverseErrorTolerance,
                resultPolyline, resultParameters);
            resultPolyline.Add(pointArray2[0]);
            resultParameters?.Add(num);
            DoCubicMidpointSubdivision(pointArray2, depth, num, rightParameter, inverseErrorTolerance,
                resultPolyline, resultParameters);
        }
        else
        {
            DoCubicForwardDifferencing(pointArray, leftParameter, num, inverseErrorTolerance,
                resultPolyline, resultParameters);
            resultPolyline.Add(pointArray2[0]);
            resultParameters?.Add(num);
            DoCubicForwardDifferencing(pointArray2, num, rightParameter, inverseErrorTolerance,
                resultPolyline, resultParameters);
        }
    }

    private static void EnsureErrorTolerance(ref double errorTolerance)
    {
        if (errorTolerance <= 0.0)
        {
            errorTolerance = 0.25;
        }
    }

    public static void FlattenCubic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
    {
        if (resultPolyline == null) throw new ArgumentNullException(nameof(resultPolyline));
        if (controlPoints == null) throw new ArgumentNullException(nameof(controlPoints));
        if (controlPoints.Length != 4) throw new ArgumentOutOfRangeException(nameof(controlPoints));
        EnsureErrorTolerance(ref errorTolerance);
        if (!skipFirstPoint)
        {
            resultPolyline.Add(controlPoints[0]);
            resultParameters?.Add(0.0);
        }

        if (IsCubicChordMonotone(controlPoints, errorTolerance * errorTolerance))
        {
            var flattener = new AdaptiveForwardDifferencingCubicFlattener(controlPoints, errorTolerance,
                errorTolerance, true);
            var p = new Point();
            var u = 0.0;
            while (flattener.Next(ref p, ref u))
            {
                resultPolyline.Add(p);
                resultParameters?.Add(u);
            }
        }
        else
        {
            var x = controlPoints[3].X - controlPoints[2].X + controlPoints[1].X - controlPoints[0].X;
            var y = controlPoints[3].Y - controlPoints[2].Y + controlPoints[1].Y - controlPoints[0].Y;
            var num4 = 1.0 / errorTolerance;
            var depth = Log8UnsignedInt32((uint) (MathHelper.Hypotenuse(x, y) * num4 + 0.5));
            if (depth > 0) depth--;
            if (depth > 0)
                DoCubicMidpointSubdivision(controlPoints, depth, 0.0, 1.0, 0.75 * num4, resultPolyline,
                    resultParameters);
            else
                DoCubicForwardDifferencing(controlPoints, 0.0, 1.0, 0.75 * num4, resultPolyline,
                    resultParameters);
        }

        resultPolyline.Add(controlPoints[3]);
        resultParameters?.Add(1.0);
    }

    public static void FlattenQuadratic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
    {
        if (resultPolyline == null)
        {
            throw new ArgumentNullException(nameof(resultPolyline));
        }
        if (controlPoints == null)
        {
            throw new ArgumentNullException(nameof(controlPoints));
        }
        if (controlPoints.Length != 3)
        {
            throw new ArgumentOutOfRangeException(nameof(controlPoints));
        }
        EnsureErrorTolerance(ref errorTolerance);
        Point[] pointArray = { controlPoints[0], GeometryHelper.Lerp(controlPoints[0], controlPoints[1], 0.66666666666666663), GeometryHelper.Lerp(controlPoints[1], controlPoints[2], 0.33333333333333331), controlPoints[2] };
        FlattenCubic(pointArray, errorTolerance, resultPolyline, skipFirstPoint, resultParameters);
    }

    private static bool IsCubicChordMonotone(Point[] controlPoints, double squaredTolerance)
    {
        double num = GeometryHelper.SquaredDistance(controlPoints[0], controlPoints[3]);
        if (num <= squaredTolerance)
        {
            return false;
        }
        Vector lhs = controlPoints[3].Subtract(controlPoints[0]);
        Vector rhs = controlPoints[1].Subtract(controlPoints[0]);
        double num2 = GeometryHelper.Dot(lhs, rhs);
        if ((num2 < 0.0) || (num2 > num))
        {
            return false;
        }
        Vector vector3 = controlPoints[2].Subtract(controlPoints[0]);
        double num3 = GeometryHelper.Dot(lhs, vector3);
        if ((num3 < 0.0) || (num3 > num))
        {
            return false;
        }
        if (num2 > num3)
        {
            return false;
        }
        return true;
    }

    private static uint Log4Double(double d)
    {
        uint num = 0;
        while (d > 1.0)
        {
            d *= 0.25;
            num++;
        }
        return num;
    }

    private static uint Log4UnsignedInt32(uint i)
    {
        uint num = 0;
        while (i > 0)
        {
            i = i >> 2;
            num++;
        }
        return num;
    }

    private static uint Log8UnsignedInt32(uint i)
    {
        uint num = 0;
        while (i > 0)
        {
            i = i >> 3;
            num++;
        }
        return num;
    }

    private class AdaptiveForwardDifferencingCubicFlattener
    {
        private double _aX;

        private double _aY;

        private double _bX;

        private double _bY;

        private double _cX;

        private double _cY;

        private readonly double _distanceTolerance;

        private readonly bool _doParameters;

        private double _dParameter;

        private double _dX;

        private double _dY;

        private readonly double _flatnessTolerance;

        private int _numSteps;

        private double _parameter;

        internal AdaptiveForwardDifferencingCubicFlattener(Point[] controlPoints,
            double flatnessTolerance, double distanceTolerance, bool doParameters)
        {
            _numSteps = 1;
            _dParameter = 1.0;
            _flatnessTolerance = 3.0 * flatnessTolerance;
            _distanceTolerance = distanceTolerance;
            _doParameters = doParameters;
            _aX = -controlPoints[0].X + 3.0 * (controlPoints[1].X - controlPoints[2].X) +
                  controlPoints[3].X;
            _aY = -controlPoints[0].Y + 3.0 * (controlPoints[1].Y - controlPoints[2].Y) +
                  controlPoints[3].Y;
            _bX = 3.0 * (controlPoints[0].X - 2.0 * controlPoints[1].X + controlPoints[2].X);
            _bY = 3.0 * (controlPoints[0].Y - 2.0 * controlPoints[1].Y + controlPoints[2].Y);
            _cX = 3.0 * (-controlPoints[0].X + controlPoints[1].X);
            _cY = 3.0 * (-controlPoints[0].Y + controlPoints[1].Y);
            _dX = controlPoints[0].X;
            _dY = controlPoints[0].Y;
        }

        private void DoubleStepSize()
        {
            _aX *= 8.0;
            _aY *= 8.0;
            _bX *= 4.0;
            _bY *= 4.0;
            _cX += _cX;
            _cY += _cY;
            if (_doParameters) _dParameter *= 2.0;
            _numSteps = _numSteps >> 1;
        }

        private void HalveStepSize()
        {
            _aX *= 0.125;
            _aY *= 0.125;
            _bX *= 0.25;
            _bY *= 0.25;
            _cX *= 0.5;
            _cY *= 0.5;
            if (_doParameters) _dParameter *= 0.5;
            _numSteps = _numSteps << 1;
        }

        private void IncrementDifferencesAndParameter()
        {
            _dX = _aX + _bX + _cX + _dX;
            _dY = _aY + _bY + _cY + _dY;
            _cX = _aX + _aX + _aX + _bX + _bX + _cX;
            _cY = _aY + _aY + _aY + _bY + _bY + _cY;
            _bX = _aX + _aX + _aX + _bX;
            _bY = _aY + _aY + _aY + _bY;
            _numSteps--;
            _parameter += _dParameter;
        }

        private bool MustSubdivide(double flatnessTolerance)
        {
            var num = -(_aY + _bY + _cY);
            var num2 = _aX + _bX + _cX;
            var num3 = Math.Abs(num) + Math.Abs(num2);
            if (num3 <= _distanceTolerance) return false;
            num3 *= flatnessTolerance;
            if (Math.Abs(_cX * num + _cY * num2) <= num3 &&
                Math.Abs((_bX + _cX + _cX) * num + (_bY + _cY + _cY) * num2) <= num3) return false;
            return true;
        }

        // ReSharper disable once RedundantAssignment
        internal bool Next(ref Point p, ref double u)
        {
            while (MustSubdivide(_flatnessTolerance)) HalveStepSize();
            if ((_numSteps & 1) == 0)
                while (_numSteps > 1 && !MustSubdivide(_flatnessTolerance * 0.25))
                    DoubleStepSize();
            IncrementDifferencesAndParameter();
            p.X = _dX;
            p.Y = _dY;
            u = _parameter;
            return _numSteps != 0;
        }
    }
}

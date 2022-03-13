using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HandyControl.Expression.Drawing;

internal class PolylineData
{
    private IList<double> _accumulates;

    private IList<double> _angles;

    private IList<double> _lengths;

    private IList<Vector> _normals;

    private double? _totalLength;

    public PolylineData(IList<Point> points)
    {
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (points.Count <= 1) throw new ArgumentOutOfRangeException(nameof(points));
        Points = points;
    }

    public IList<double> AccumulatedLength =>
        _accumulates ?? ComputeAccumulatedLength();

    public IList<double> Angles =>
        _angles ?? ComputeAngles();

    public int Count =>
        Points.Count;

    public bool IsClosed =>
        Points[0] == Points.Last();

    public IList<double> Lengths =>
        _lengths ?? ComputeLengths();

    public IList<Vector> Normals =>
        _normals ?? ComputeNormals();

    public IList<Point> Points { get; }

    public double TotalLength
    {
        get
        {
            var length = _totalLength;
            return length ?? ComputeTotalLength();
        }
    }

    private IList<double> ComputeAccumulatedLength()
    {
        _accumulates = new double[Count];
        _accumulates[0] = 0.0;
        for (var i = 1; i < Count; i++) _accumulates[i] = _accumulates[i - 1] + Lengths[i - 1];
        _totalLength = _accumulates.Last();
        return _accumulates;
    }

    private IList<double> ComputeAngles()
    {
        _angles = new double[Count];
        for (var i = 1; i < Count - 1; i++) _angles[i] = -GeometryHelper.Dot(Normals[i - 1], Normals[i]);
        if (IsClosed)
        {
            var num2 = -GeometryHelper.Dot(Normals[0], Normals[Count - 2]);
            _angles[0] = _angles[Count - 1] = num2;
        }
        else
        {
            _angles[0] = _angles[Count - 1] = 1.0;
        }

        return _angles;
    }

    private IList<double> ComputeLengths()
    {
        _lengths = new double[Count];
        for (var i = 0; i < Count; i++) _lengths[i] = Difference(i).Length;
        return _lengths;
    }

    private IList<Vector> ComputeNormals()
    {
        _normals = new Vector[Points.Count];
        for (var i = 0; i < Count - 1; i++) _normals[i] = GeometryHelper.Normal(Points[i], Points[i + 1]);
        _normals[Count - 1] = _normals[Count - 2];
        return _normals;
    }

    private double ComputeTotalLength()
    {
        ComputeAccumulatedLength();
        // ReSharper disable once PossibleInvalidOperationException
        return _totalLength.Value;
    }

    public Vector Difference(int index)
    {
        var num = (index + 1) % Count;
        return Points[num].Subtract(Points[index]);
    }

    public Vector SmoothNormal(int index, double fraction, double cornerRadius)
    {
        if (cornerRadius > 0.0)
        {
            var num = Lengths[index];
            if (MathHelper.IsVerySmall(num))
            {
                var num2 = index - 1;
                if (num2 < 0 && IsClosed) num2 = Count - 1;
                var num3 = index + 1;
                if (IsClosed && num3 >= Count - 1) num3 = 0;
                if (num2 >= 0 && num3 < Count)
                    return GeometryHelper.Lerp(Normals[num3], Normals[num2], 0.5).Normalized();
                return Normals[index];
            }

            var num4 = Math.Min(cornerRadius / num, 0.5);
            if (fraction <= num4)
            {
                var num5 = index - 1;
                if (IsClosed && num5 == -1) num5 = Count - 1;
                if (num5 >= 0)
                {
                    var alpha = (num4 - fraction) / (2.0 * num4);
                    return GeometryHelper.Lerp(Normals[index], Normals[num5], alpha).Normalized();
                }
            }
            else if (fraction >= 1.0 - num4)
            {
                var num7 = index + 1;
                if (IsClosed && num7 >= Count - 1) num7 = 0;
                if (num7 < Count)
                {
                    var num8 = (fraction + num4 - 1.0) / (2.0 * num4);
                    return GeometryHelper.Lerp(Normals[index], Normals[num7], num8).Normalized();
                }
            }
        }

        return Normals[index];
    }
}

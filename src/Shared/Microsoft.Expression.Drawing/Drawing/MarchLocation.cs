using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Expression.Drawing;

internal class MarchLocation
{
    public double After { get; private set; }

    public double Before { get; private set; }

    public int Index { get; private set; }

    public double Ratio { get; private set; }

    public MarchStopReason Reason { get; private set; }

    public double Remain { get; private set; }

    public static MarchLocation Create(MarchStopReason reason, int index, double before, double after,
        double remain)
    {
        var num = before + after;
        return new MarchLocation
        {
            Reason = reason,
            Index = index,
            Remain = remain,
            Before = MathHelper.EnsureRange(before, 0.0, num),
            After = MathHelper.EnsureRange(after, 0.0, num),
            Ratio = MathHelper.EnsureRange(MathHelper.SafeDivide(before, num, 0.0), 0.0, 1.0)
        };
    }

    public double GetArcLength(IList<double> accumulatedLengths)
    {
        return MathHelper.Lerp(accumulatedLengths[Index], accumulatedLengths[Index + 1], Ratio);
    }

    public Vector GetNormal(PolylineData polyline, double cornerRadius = 0.0)
    {
        return polyline.SmoothNormal(Index, Ratio, cornerRadius);
    }

    public Point GetPoint(IList<Point> points)
    {
        return GeometryHelper.Lerp(points[Index], points[Index + 1], Ratio);
    }
}

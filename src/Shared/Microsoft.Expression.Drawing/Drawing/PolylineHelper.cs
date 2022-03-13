using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyControl.Expression.Drawing;

internal static class PolylineHelper
{
    public static IEnumerable<PolylineData> GetWrappedPolylines(IList<PolylineData> lines,
        ref double startArcLength)
    {
        var count = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            count = i;
            startArcLength -= lines[i].TotalLength;
            if (MathHelper.LessThanOrClose(startArcLength, 0.0)) break;
        }

        if (!MathHelper.LessThanOrClose(startArcLength, 0.0))
            throw new ArgumentOutOfRangeException(nameof(startArcLength));
        startArcLength += lines[count].TotalLength;
        return lines.Skip(count).Concat(lines.Take(count + 1));
    }

    public static void PathMarch(PolylineData polyline, double startArcLength, double cornerThreshold,
        Func<MarchLocation, double> stopCallback)
    {
        if (polyline == null) throw new ArgumentNullException(nameof(polyline));
        var count = polyline.Count;
        if (count <= 1) throw new ArgumentOutOfRangeException(nameof(polyline));
        var flag = false;
        var x = startArcLength;
        var before = 0.0;
        var index = 0;
        var num5 = Math.Cos(cornerThreshold * 3.1415926535897931 / 180.0);
        while (true)
        {
            var num6 = polyline.Lengths[index];
            if (!MathHelper.IsFiniteDouble(x)) return;
            if (MathHelper.IsVerySmall(x))
            {
                x = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before,
                    num6 - before, x));
                flag = true;
            }
            else if (MathHelper.GreaterThan(x, 0.0))
            {
                if (MathHelper.LessThanOrClose(x + before, num6))
                {
                    before += x;
                    x = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before,
                        num6 - before, 0.0));
                    flag = true;
                }
                else if (index < count - 2)
                {
                    index++;
                    var num7 = num6 - before;
                    x -= num7;
                    before = 0.0;
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (flag && num5 != 1.0 && polyline.Angles[index] > num5)
                    {
                        num6 = polyline.Lengths[index];
                        x = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, index, before,
                            num6 - before, x));
                    }
                }
                else
                {
                    var num8 = num6 - before;
                    x -= num8;
                    num6 = polyline.Lengths[index];
                    before = polyline.Lengths[index];
                    x = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, index, before,
                        num6 - before, x));
                    flag = true;
                }
            }
            else if (MathHelper.LessThan(x, 0.0))
            {
                if (MathHelper.GreaterThanOrClose(x + before, 0.0))
                {
                    before += x;
                    x = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before,
                        num6 - before, 0.0));
                    flag = true;
                }
                else if (index > 0)
                {
                    index--;
                    x += before;
                    before = polyline.Lengths[index];
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (flag && num5 != 1.0 && polyline.Angles[index + 1] > num5)
                    {
                        num6 = polyline.Lengths[index];
                        x = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, index, before,
                            num6 - before, x));
                    }
                }
                else
                {
                    x += before;
                    num6 = polyline.Lengths[index];
                    before = 0.0;
                    x = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, index, before,
                        num6 - before, x));
                    flag = true;
                }
            }
        }
    }
}

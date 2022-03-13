using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Expression.Drawing;

namespace HandyControl.Expression.Media;

public sealed class SketchGeometryEffect : GeometryEffect
{
    private readonly long _randomSeed = DateTime.Now.Ticks;

    protected override GeometryEffect DeepCopy()
    {
        return new SketchGeometryEffect();
    }

    private static void DisturbPoints(RandomEngine random, double scale, IList<Point> points,
        IList<Vector> normals)
    {
        var count = points.Count;
        for (var i = 1; i < count; i++)
        {
            var num3 = random.NextGaussian(0.0, 1.0 * scale);
            var num4 = random.NextUniform(-0.5, 0.5) * scale;
            var point = points[i];
            var vector = normals[i];
            var vector2 = normals[i];
            var point2 = points[i];
            var vector3 = normals[i];
            var vector4 = normals[i];
            points[i] = new Point(point.X + vector.X * num4 - vector2.Y * num3,
                point2.Y + vector3.X * num3 + vector4.Y * num4);
        }
    }

    public override bool Equals(GeometryEffect geometryEffect)
    {
        return geometryEffect is SketchGeometryEffect;
    }

    private IEnumerable<SimpleSegment> GetEffectiveSegments(PathFigure pathFigure)
    {
        var startPoint = pathFigure.StartPoint;
        foreach (var iteratorVariable1 in pathFigure.AllSegments())
            foreach (var iteratorVariable2 in iteratorVariable1.PathSegment.GetSimpleSegments(
                         iteratorVariable1.StartPoint))
            {
                yield return iteratorVariable2;
                startPoint = iteratorVariable2.Points.Last();
            }

        if (pathFigure.IsClosed) yield return SimpleSegment.Create(startPoint, pathFigure.StartPoint);
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    protected override bool UpdateCachedGeometry(Geometry input)
    {
        var flag = false;
        var inputPath = input.AsPathGeometry();
        if (inputPath != null) return flag | UpdateSketchGeometry(inputPath);
        CachedGeometry = input;
        return flag;
    }

    private bool UpdateSketchGeometry(PathGeometry inputPath)
    {
        var flag = false;
        flag |= GeometryHelper.EnsureGeometryType(out var geometry, ref CachedGeometry,
            () => new PathGeometry());
        flag |= geometry.Figures.EnsureListCount(inputPath.Figures.Count, () => new PathFigure());
        var random = new RandomEngine(_randomSeed);
        for (var i = 0; i < inputPath.Figures.Count; i++)
        {
            var pathFigure = inputPath.Figures[i];
            var isClosed = pathFigure.IsClosed;
            var isFilled = pathFigure.IsFilled;
            if (pathFigure.Segments.Count == 0)
            {
                flag |= geometry.Figures[i]
                    .SetIfDifferent(PathFigure.StartPointProperty, pathFigure.StartPoint);
                flag |= geometry.Figures[i].Segments.EnsureListCount(0);
            }
            else
            {
                var list = new List<Point>(pathFigure.Segments.Count * 3);
                foreach (var segment in GetEffectiveSegments(pathFigure))
                {
                    var resultPolyline = new List<Point>
                    {
                        segment.Points[0]
                    };
                    segment.Flatten(resultPolyline, 0.0, null);
                    var polyline = new PolylineData(resultPolyline);
                    if (resultPolyline.Count > 1 && polyline.TotalLength > 4.0)
                    {
                        var a = polyline.TotalLength / 8.0;
                        var sampleCount = (int) Math.Max(2.0, Math.Ceiling(a));
                        var interval = polyline.TotalLength / sampleCount;
                        var scale = interval / 8.0;
                        var samplePoints = new List<Point>(sampleCount);
                        var sampleNormals = new List<Vector>(sampleCount);
                        var sampleIndex = 0;
                        PolylineHelper.PathMarch(polyline, 0.0, 0.0, delegate (MarchLocation location)
                        {
                            if (location.Reason == MarchStopReason.CompletePolyline) return double.NaN;
                            if (location.Reason != MarchStopReason.CompleteStep) return location.Remain;
                            if (sampleIndex++ == sampleCount) return double.NaN;
                            samplePoints.Add(location.GetPoint(polyline.Points));
                            sampleNormals.Add(location.GetNormal(polyline));
                            return interval;
                        });
                        DisturbPoints(random, scale, samplePoints, sampleNormals);
                        list.AddRange(samplePoints);
                    }
                    else
                    {
                        list.AddRange(resultPolyline);
                        list.RemoveLast();
                    }
                }

                if (!isClosed) list.Add(pathFigure.Segments.Last().GetLastPoint());
                flag |= PathFigureHelper.SyncPolylineFigure(geometry.Figures[i], list, isClosed,
                    isFilled);
            }
        }

        if (flag) CachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(CachedGeometry);
        return flag;
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HandyControl.Controls;

internal class ConfettiOverlay : Control
{
    private static readonly Random Random = new();

    private readonly List<ConfettiCannon.Confetti> _renderList = new();
    private Rect _renderRect;

    public ConfettiOverlay()
    {
        IsHitTestVisible = false;
    }

    public void UpdateConfettis(ConcurrentQueue<ConfettiCannon.Confetti> confettis, Rect renderRect)
    {
        _renderRect = renderRect;
        _renderList.Clear();
        foreach (var confetti in confettis)
        {
            UpdateConfetti(confetti);
            _renderList.Add(confetti);
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        using (context.PushClip(_renderRect))
        {
            foreach (var confetti in _renderList)
            {
                DrawConfetti(context, confetti);
            }
        }
    }

    private static void UpdateConfetti(ConfettiCannon.Confetti confetti)
    {
        confetti.Point = new Point(
            confetti.Point.X + Math.Sin(confetti.Angle2D) * confetti.Velocity + confetti.Drift,
            confetti.Point.Y + Math.Cos(confetti.Angle2D) * confetti.Velocity + confetti.Gravity
        );
        confetti.Velocity *= confetti.Decay;

        if (confetti.Flat)
        {
            confetti.Wobble = 0;
            confetti.WobbleX = confetti.Point.X + 10 * confetti.Scalar;
            confetti.WobbleY = confetti.Point.Y + 10 * confetti.Scalar;

            confetti.TiltSin = 0;
            confetti.TiltCos = 0;
            confetti.RandomValue = 1;
        }
        else
        {
            confetti.Wobble += confetti.WobbleSpeed;
            confetti.WobbleX = confetti.Point.X + 10 * confetti.Scalar * Math.Cos(confetti.Wobble);
            confetti.WobbleY = confetti.Point.Y + 10 * confetti.Scalar * Math.Sin(confetti.Wobble);

            confetti.TiltAngle += 0.1;
            confetti.TiltSin = Math.Sin(confetti.TiltAngle);
            confetti.TiltCos = Math.Cos(confetti.TiltAngle);
            confetti.RandomValue = Random.NextDouble() + 2;
        }

        confetti.Tick++;
    }

    private static void DrawConfetti(DrawingContext context, ConfettiCannon.Confetti confetti)
    {
        double progress = (double) confetti.Tick / confetti.TotalTicks;
        double opacity = Math.Max(0, 1 - progress);
        var brush = confetti.Brush;

        double x1 = confetti.Point.X + confetti.RandomValue * confetti.TiltCos;
        double y1 = confetti.Point.Y + confetti.RandomValue * confetti.TiltSin;
        double x2 = confetti.WobbleX + confetti.RandomValue * confetti.TiltCos;
        double y2 = confetti.WobbleY + confetti.RandomValue * confetti.TiltSin;

        using (context.PushOpacity(opacity))
        {
            if ("circle".Equals(confetti.Shape))
            {
                double radiusX = Math.Abs(x2 - x1) * confetti.OvalScalar;
                double radiusY = Math.Abs(y2 - y1) * confetti.OvalScalar;
                double angle = 18 * confetti.Wobble;

                var rotate = new RotateTransform(angle, confetti.Point.X, confetti.Point.Y);
                using (context.PushTransform(rotate.Value))
                {
                    context.DrawEllipse(brush, null, confetti.Point, radiusX, radiusY);
                }
            }
            else if ("star".Equals(confetti.Shape))
            {
                double rot = Math.PI / 2 * 3;
                double innerRadius = 4 * confetti.Scalar;
                double outerRadius = 8 * confetti.Scalar;
                int spikes = 5;
                double step = Math.PI / spikes;
                bool isFirst = true;

                var geometry = new StreamGeometry();
                using (var sgc = geometry.Open())
                {
                    while (spikes-- > 0)
                    {
                        double x = confetti.Point.X + Math.Cos(rot) * outerRadius;
                        double y = confetti.Point.Y + Math.Sin(rot) * outerRadius;

                        if (isFirst)
                        {
                            sgc.BeginFigure(new Point(x, y), true);
                            isFirst = false;
                        }
                        else
                        {
                            sgc.LineTo(new Point(x, y));
                        }

                        rot += step;

                        x = confetti.Point.X + Math.Cos(rot) * innerRadius;
                        y = confetti.Point.Y + Math.Sin(rot) * innerRadius;
                        sgc.LineTo(new Point(x, y));
                        rot += step;
                    }

                    sgc.EndFigure(true);
                }

                context.DrawGeometry(brush, null, geometry);
            }
            else
            {
                var geometry = new StreamGeometry();
                using (var sgc = geometry.Open())
                {
                    sgc.BeginFigure(new Point(Math.Floor(confetti.Point.X), Math.Floor(confetti.Point.Y)), true);
                    sgc.LineTo(new Point(Math.Floor(confetti.WobbleX), Math.Floor(y1)));
                    sgc.LineTo(new Point(Math.Floor(x2), Math.Floor(y2)));
                    sgc.LineTo(new Point(Math.Floor(x1), Math.Floor(confetti.WobbleY)));
                    sgc.EndFigure(true);
                }

                context.DrawGeometry(brush, null, geometry);
            }
        }
    }
}

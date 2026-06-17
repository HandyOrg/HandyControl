using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Collections;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using HandyControl.Tools;

namespace HandyControl.Controls;

/// <summary>
///     A shape that draws itself along a path over time.
///     Uses <see cref="StrokeDashOffset" /> animation on the <see cref="Data" /> geometry
///     to create a "drawing" or "reveal" effect.
/// </summary>
public class AnimationPath : Shape
{
    private readonly DispatcherTimer _timer;
    private TimeSpan _elapsed;
    private double _pathLength;
    private int _remainingRepeats;
    private bool _isDisposed;

    public AnimationPath()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; // ~60 fps
        _timer.Tick += OnTimerTick;
    }

    ~AnimationPath() => Dispose();

    public void Dispose()
    {
        if (_isDisposed) return;
        _timer.Tick -= OnTimerTick;
        _timer.Stop();
        LayoutUpdated -= OnLayoutUpdated;
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    // ── Properties ──

    /// <summary>
    ///     The geometry path to draw.
    /// </summary>
    public static readonly StyledProperty<Geometry?> DataProperty =
        AvaloniaProperty.Register<AnimationPath, Geometry?>(nameof(Data));

    public Geometry? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Geometry? CreateDefiningGeometry() => Data;

    /// <summary>
    ///     Explicit path length. If ≤ 0, computed automatically from <see cref="Data"/> geometry.
    /// </summary>
    public static readonly StyledProperty<double> PathLengthProperty =
        AvaloniaProperty.Register<AnimationPath, double>(nameof(PathLength));

    public double PathLength
    {
        get => GetValue(PathLengthProperty);
        set => SetValue(PathLengthProperty, value);
    }

    /// <summary>
    ///     Duration of one complete animation cycle.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<AnimationPath, TimeSpan>(nameof(Duration),
            defaultValue: TimeSpan.FromSeconds(2));

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>
    ///     Whether the animation is currently playing.
    /// </summary>
    public static readonly StyledProperty<bool> IsPlayingProperty =
        AvaloniaProperty.Register<AnimationPath, bool>(nameof(IsPlaying),
            defaultValue: true);

    public bool IsPlaying
    {
        get => GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    /// <summary>
    ///     How many times to repeat. 0 = infinite (forever). Default = 0.
    /// </summary>
    public static readonly StyledProperty<int> RepeatCountProperty =
        AvaloniaProperty.Register<AnimationPath, int>(nameof(RepeatCount), defaultValue: 0,
            validate: v => v >= 0);

    public int RepeatCount
    {
        get => GetValue(RepeatCountProperty);
        set => SetValue(RepeatCountProperty, value);
    }

    /// <summary>
    ///     Controls the end value of the animation.
    ///     <see cref="FillMode.None"/> animates from +pathLength to -pathLength (stroke disappears);
    ///     <see cref="FillMode.Forward"/> animates from +pathLength to 0 (stroke stays drawn).
    /// </summary>
    public static readonly StyledProperty<FillMode> FillModeProperty =
        AvaloniaProperty.Register<AnimationPath, FillMode>(nameof(FillMode),
            defaultValue: FillMode.None);

    public FillMode FillMode
    {
        get => GetValue(FillModeProperty);
        set => SetValue(FillModeProperty, value);
    }

    // ── Events ──

    public event EventHandler? Completed;

    // ── Static constructor ──

    static AnimationPath()
    {
        AffectsRender<AnimationPath>(
            DataProperty, PathLengthProperty, DurationProperty,
            IsPlayingProperty, RepeatCountProperty, FillModeProperty);

        DataProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnPropertyChanged());
        PathLengthProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnPropertyChanged());
        DurationProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnPropertyChanged());
        IsPlayingProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnIsPlayingChanged(e));
        RepeatCountProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnPropertyChanged());
        FillModeProperty.Changed.AddClassHandler<AnimationPath>((o, e) => o.OnPropertyChanged());
    }

    // ── Lifecycle ──

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        // Bounds may not be ready at attach time; retry after layout
        LayoutUpdated += OnLayoutUpdated;
        UpdatePath();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        LayoutUpdated -= OnLayoutUpdated;
        _timer.Stop();
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (!MathHelper.IsVerySmall(Bounds.Width) && !MathHelper.IsVerySmall(Bounds.Height))
        {
            LayoutUpdated -= OnLayoutUpdated;
            UpdatePath();
        }
    }

    // ── Property handlers ──

    private void OnPropertyChanged() => UpdatePath();

    private void OnIsPlayingChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.GetNewValue<bool>())
        {
            UpdatePath();
        }
        else
        {
            StopAnimation();
        }
    }

    // ── Animation core ──

    private void UpdatePath()
    {
        _timer.Stop();
        if (!IsPlaying || Data == null) return;

        // Compute path length
        _pathLength = PathLength > 0 ? PathLength : GetScaledTotalLength();
        if (MathHelper.IsVerySmall(_pathLength)) return;

        // Dash array: two dashes each exactly the full path length
        StrokeDashArray = new AvaloniaList<double> { _pathLength, _pathLength };
        StrokeDashOffset = _pathLength;

        // Reset iteration state
        _elapsed = TimeSpan.Zero;
        _remainingRepeats = RepeatCount;

        if (!MathHelper.IsVerySmall(Duration.TotalMilliseconds))
        {
            _timer.Start();
        }
    }

    private void StopAnimation()
    {
        _timer.Stop();
        _elapsed = TimeSpan.Zero;
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        _elapsed += _timer.Interval;

        var duration = Duration;
        if (duration.TotalMilliseconds <= 0) return;

        var progress = Math.Min(_elapsed.TotalMilliseconds / duration.TotalMilliseconds, 1.0);

        // Interpolate StrokeDashOffset linearly
        double startValue = _pathLength;
        double endValue = FillMode == FillMode.None ? -_pathLength : 0;
        StrokeDashOffset = startValue + (endValue - startValue) * progress;

        if (progress >= 1.0)
        {
            // Cycle finished
            Completed?.Invoke(this, EventArgs.Empty);

            bool shouldRepeat;
            if (_remainingRepeats == 0)
            {
                shouldRepeat = true; // Infinite
            }
            else
            {
                _remainingRepeats--;
                shouldRepeat = _remainingRepeats > 0;
            }

            if (shouldRepeat)
            {
                _elapsed = TimeSpan.Zero;
                StrokeDashOffset = startValue; // Reset for next cycle
            }
            else
            {
                _timer.Stop();
            }
        }
    }

    // ── Path length calculation ──

    private double GetScaledTotalLength()
    {
        var geometry = Data;
        if (geometry == null) return 0;

        var renderSize = Bounds.Size;
        if (MathHelper.IsVerySmall(renderSize.Width) || MathHelper.IsVerySmall(renderSize.Height))
            return 0;

        var length = GetGeometryLength(geometry);
        if (MathHelper.IsVerySmall(length)) return 0;

        // Scale from native geometry space to render space (Stretch.Uniform)
        var sw = geometry.Bounds.Width / renderSize.Width;
        var sh = geometry.Bounds.Height / renderSize.Height;
        var min = Math.Min(sw, sh);

        if (MathHelper.IsVerySmall(min) || MathHelper.IsVerySmall(StrokeThickness))
            return 0;

        length /= min;
        return length / StrokeThickness;
    }

    private static double GetGeometryLength(Geometry geometry)
    {
        if (geometry is PathGeometry { Figures: { } figures })
        {
            double totalLength = 0;
            foreach (var figure in figures)
            {
                totalLength += GetFigureLength(figure);
            }
            return totalLength;
        }

        // Fallback: estimate from bounding box diagonal
        var bounds = geometry.Bounds;
        return Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 2;
    }

    private static double PointLen(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double GetFigureLength(PathFigure figure)
    {
        double length = 0;
        var current = figure.StartPoint;

        if (figure.Segments is null) return length;

        foreach (var segment in figure.Segments)
        {
            switch (segment)
            {
                case ArcSegment arc:
                    length += PointLen(arc.Point, current);
                    current = arc.Point;
                    break;

                case BezierSegment bezier:
                    length += ApproximateCubicBezierLength(current, bezier.Point1, bezier.Point2, bezier.Point3);
                    current = bezier.Point3;
                    break;

                case LineSegment line:
                    length += PointLen(line.Point, current);
                    current = line.Point;
                    break;

                case PolyLineSegment { Points: { } polyLinePoints }:
                    foreach (var pt in polyLinePoints)
                    {
                        length += PointLen(pt, current);
                        current = pt;
                    }
                    break;

                case PolyBezierSegment { Points: { } polyBezierPoints }:
                    for (var i = 0; i + 2 < polyBezierPoints.Count; i += 3)
                    {
                        length += ApproximateCubicBezierLength(
                            current,
                            polyBezierPoints[i],
                            polyBezierPoints[i + 1],
                            polyBezierPoints[i + 2]);
                        current = polyBezierPoints[i + 2];
                    }
                    break;

                case QuadraticBezierSegment quad:
                    length += ApproximateQuadraticBezierLength(current, quad.Point1, quad.Point2);
                    current = quad.Point2;
                    break;
            }
        }

        if (figure.IsClosed)
        {
            length += PointLen(figure.StartPoint, current);
        }

        return length;
    }

    /// <summary>
    ///     Approximate cubic bezier length by subdividing into <paramref name="steps"/> line segments.
    /// </summary>
    private static double ApproximateCubicBezierLength(Point p0, Point p1, Point p2, Point p3, int steps = 8)
    {
        double length = 0;
        var prev = p0;

        for (var i = 1; i <= steps; i++)
        {
            var t = i / (double)steps;
            var u = 1.0 - t;
            var uu = u * u;
            var uuu = uu * u;
            var tt = t * t;
            var ttt = tt * t;

            var pt = new Point(
                uuu * p0.X + 3 * uu * t * p1.X + 3 * u * tt * p2.X + ttt * p3.X,
                uuu * p0.Y + 3 * uu * t * p1.Y + 3 * u * tt * p2.Y + ttt * p3.Y
            );

            length += PointLen(pt, prev);
            prev = pt;
        }

        return length;
    }

    /// <summary>
    ///     Approximate quadratic bezier length by subdividing into <paramref name="steps"/> line segments.
    /// </summary>
    private static double ApproximateQuadraticBezierLength(Point p0, Point p1, Point p2, int steps = 8)
    {
        double length = 0;
        var prev = p0;

        for (var i = 1; i <= steps; i++)
        {
            var t = i / (double)steps;
            var u = 1.0 - t;

            var pt = new Point(
                u * u * p0.X + 2 * u * t * p1.X + t * t * p2.X,
                u * u * p0.Y + 2 * u * t * p1.Y + t * t * p2.Y
            );

            length += PointLen(pt, prev);
            prev = pt;
        }

        return length;
    }
}

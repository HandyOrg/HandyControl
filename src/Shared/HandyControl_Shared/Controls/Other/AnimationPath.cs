using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class AnimationPath : Shape
{
    private Storyboard _storyboard;

    private double _pathLength;

    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data),
        typeof(Geometry), typeof(AnimationPath), new FrameworkPropertyMetadata(null,
            OnPropertiesChanged));

    private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AnimationPath path)
        {
            path.UpdatePath();
        }
    }

    public Geometry Data
    {
        get => (Geometry) GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

    public static readonly DependencyProperty PathLengthProperty = DependencyProperty.Register(
        nameof(PathLength), typeof(double), typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnPropertiesChanged));

    public double PathLength
    {
        get => (double) GetValue(PathLengthProperty);
        set => SetValue(PathLengthProperty, value);
    }

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration), typeof(Duration), typeof(AnimationPath), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)),
            OnPropertiesChanged));

    public Duration Duration
    {
        get => (Duration) GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
        nameof(IsPlaying), typeof(bool), typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, (o, args) =>
        {
            var ctl = (AnimationPath) o;
            var v = (bool) args.NewValue;
            if (v)
            {
                ctl.UpdatePath();
            }
            else
            {
                ctl._storyboard?.Pause();
            }
        }));

    public bool IsPlaying
    {
        get => (bool) GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty RepeatBehaviorProperty =
        Timeline.RepeatBehaviorProperty.AddOwner(typeof(AnimationPath),
            new PropertyMetadata(RepeatBehavior.Forever));

    public RepeatBehavior RepeatBehavior
    {
        get => (RepeatBehavior) GetValue(RepeatBehaviorProperty);
        set => SetValue(RepeatBehaviorProperty, value);
    }

    public static readonly DependencyProperty FillBehaviorProperty =
        Timeline.FillBehaviorProperty.AddOwner(typeof(AnimationPath), new PropertyMetadata(FillBehavior.Stop));

    public FillBehavior FillBehavior
    {
        get { return (FillBehavior) GetValue(FillBehaviorProperty); }
        set { SetValue(FillBehaviorProperty, value); }
    }

    static AnimationPath()
    {
        StretchProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(Stretch.Uniform,
            OnPropertiesChanged));

        StrokeThicknessProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double1Box,
            OnPropertiesChanged));
    }

    public AnimationPath() => Loaded += (s, e) => UpdatePath();

    public static readonly RoutedEvent CompletedEvent =
        EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
            typeof(EventHandler), typeof(AnimationPath));

    public event EventHandler Completed
    {
        add => AddHandler(CompletedEvent, value);
        remove => RemoveHandler(CompletedEvent, value);
    }

    private void UpdatePath()
    {
        if (!Duration.HasTimeSpan || !IsPlaying) return;

        _pathLength = PathLength > 0 ? PathLength : Data.GetTotalLength(new Size(ActualWidth, ActualHeight), StrokeThickness);

        if (MathHelper.IsVerySmall(_pathLength)) return;

        StrokeDashOffset = _pathLength;
        StrokeDashArray = new DoubleCollection(new List<double>
        {
            _pathLength,
            _pathLength
        });

        if (_storyboard != null)
        {
            _storyboard.Stop();
            _storyboard.Completed -= Storyboard_Completed;
        }
        _storyboard = new Storyboard
        {
            RepeatBehavior = RepeatBehavior,
            FillBehavior = FillBehavior
        };
        _storyboard.Completed += Storyboard_Completed;

        var frames = new DoubleAnimationUsingKeyFrames();

        var frameIn = new LinearDoubleKeyFrame
        {
            Value = _pathLength,
            KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
        };
        frames.KeyFrames.Add(frameIn);

        var frameOut = new LinearDoubleKeyFrame
        {
            Value = FillBehavior == FillBehavior.Stop ? -_pathLength : 0,
            KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan)
        };
        frames.KeyFrames.Add(frameOut);

        Storyboard.SetTarget(frames, this);
        Storyboard.SetTargetProperty(frames, new PropertyPath(StrokeDashOffsetProperty));
        _storyboard.Children.Add(frames);

        _storyboard.Begin();
    }

    private void Storyboard_Completed(object sender, EventArgs e) => RaiseEvent(new RoutedEventArgs(CompletedEvent));
}

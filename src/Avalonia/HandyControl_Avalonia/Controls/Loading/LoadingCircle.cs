using System;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace HandyControl.Controls;

public class LoadingCircle : LoadingBase
{
    public static readonly StyledProperty<double> DotOffSetProperty =
        AvaloniaProperty.Register<LoadingCircle, double>(nameof(DotOffSet), 20d);

    public double DotOffSet
    {
        get => GetValue(DotOffSetProperty);
        set => SetValue(DotOffSetProperty, value);
    }

    public static readonly StyledProperty<bool> NeedHiddenProperty =
        AvaloniaProperty.Register<LoadingCircle, bool>(nameof(NeedHidden), true);

    public bool NeedHidden
    {
        get => GetValue(NeedHiddenProperty);
        set => SetValue(NeedHiddenProperty, value);
    }

    static LoadingCircle()
    {
        DotSpeedProperty.OverrideDefaultValue<LoadingCircle>(6d);
        DotDelayTimeProperty.OverrideDefaultValue<LoadingCircle>(220d);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == DotOffSetProperty || change.Property == NeedHiddenProperty)
        {
            if (IsRunning)
            {
                StopAnimations();
                UpdateDots();
            }
        }
        base.OnPropertyChanged(change);
    }

    protected sealed override void UpdateDots()
    {
        var dotCount = DotCount;
        var dotInterval = DotInterval;
        var dotSpeed = DotSpeed;
        var dotDelayTime = DotDelayTime;
        var needHidden = NeedHidden;
        var dotOffset = DotOffSet;

        if (dotCount < 1) return;
        PrivateCanvas.Children.Clear();

        for (var i = 0; i < dotCount; i++)
        {
            var rotate = new RotateTransform();
            var border = CreateBorder(i, dotInterval, needHidden, rotate);
            PrivateCanvas.Children.Add(border);

            var subAngle = -dotInterval * i;
            rotate.Angle = subAngle;

            var rotateAnimation = new Animation
            {
                Duration = TimeSpan.FromSeconds(dotSpeed),
                Delay = TimeSpan.FromMilliseconds(dotDelayTime * i),
                IterationCount = IterationCount.Infinite,
                FillMode = FillMode.Forward,
                Easing = new LinearEasing(),
                Children =
                {
                    BuildAngleFrame(0d / 7d, 0 + subAngle, null),
                    BuildAngleFrame(0.75d / 7d, 180 + subAngle, EaseOutSpline),
                    BuildAngleFrame(2.75d / 7d, 180 + dotOffset + subAngle, null),
                    BuildAngleFrame(3.5d / 7d, 360 + subAngle, EaseInSpline),
                    BuildAngleFrame(4.25d / 7d, 540 + subAngle, EaseOutSpline),
                    BuildAngleFrame(6.25d / 7d, 540 + dotOffset + subAngle, null),
                    BuildAngleFrame(1d, 720 + subAngle, EaseInSpline),
                }
            };

            var rotateCts = new CancellationTokenSource();
            AnimationTokens.Add(rotateCts);
            _ = rotateAnimation.RunAsync(border, rotateCts.Token);

            if (needHidden)
            {
                var totalSeconds = dotSpeed + 0.4d;
                var visibleCue = dotSpeed / totalSeconds;

                var opacityAnimation = new Animation
                {
                    Duration = TimeSpan.FromSeconds(totalSeconds),
                    Delay = TimeSpan.FromMilliseconds(dotDelayTime * i),
                    IterationCount = IterationCount.Infinite,
                    FillMode = FillMode.Forward,
                    Easing = new LinearEasing(),
                    Children =
                    {
                        new KeyFrame
                        {
                            Cue = new Cue(0d),
                            Setters = { new Setter(Visual.OpacityProperty, 1d) }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(Math.Max(0d, visibleCue - 0.0001d)),
                            Setters = { new Setter(Visual.OpacityProperty, 1d) }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(visibleCue),
                            Setters = { new Setter(Visual.OpacityProperty, 0d) }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(1d),
                            Setters = { new Setter(Visual.OpacityProperty, 0d) }
                        }
                    }
                };

                var opacityCts = new CancellationTokenSource();
                AnimationTokens.Add(opacityCts);
                _ = opacityAnimation.RunAsync(border, opacityCts.Token);
            }
        }
    }

    private static readonly KeySpline EaseOutSpline = new(0d, 0d, 0.5d, 1d);
    private static readonly KeySpline EaseInSpline = new(0.5d, 0d, 1d, 1d);

    private static KeyFrame BuildAngleFrame(double cue, double angle, KeySpline? spline)
    {
        var frame = new KeyFrame
        {
            Cue = new Cue(Math.Min(1d, Math.Max(0d, cue))),
            Setters = { new Setter(RotateTransform.AngleProperty, angle) }
        };
        if (spline != null)
        {
            frame.KeySpline = spline;
        }
        return frame;
    }

    private Border CreateBorder(int index, double dotInterval, bool needHidden, RotateTransform rotate)
    {
        var ellipse = CreateEllipse(index);
        ellipse.HorizontalAlignment = HorizontalAlignment.Center;
        ellipse.VerticalAlignment = VerticalAlignment.Bottom;

        var border = new Border
        {
            Child = ellipse,
            RenderTransformOrigin = RelativePoint.Center,
            RenderTransform = rotate,
            Opacity = needHidden ? 0d : 1d
        };
        border.Bind(WidthProperty, new Binding(nameof(Width)) { Source = this });
        border.Bind(HeightProperty, new Binding(nameof(Height)) { Source = this });

        return border;
    }
}

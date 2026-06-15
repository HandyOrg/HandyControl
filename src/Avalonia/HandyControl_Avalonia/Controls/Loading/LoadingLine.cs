using System;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Styling;

namespace HandyControl.Controls;

public class LoadingLine : LoadingBase
{
    private const double MoveLength = 80;
    private const double UniformScale = 0.6;

    private static readonly KeySpline EaseOutSpline = new(0d, 0d, 0.5d, 1d);
    private static readonly KeySpline EaseInSpline = new(0.5d, 0d, 1d, 1d);

    public LoadingLine()
    {
        this.Bind(HeightProperty, new Binding(nameof(DotDiameter)) { Source = this });
    }

    protected sealed override void UpdateDots()
    {
        var dotCount = DotCount;
        var dotInterval = DotInterval;
        var dotDiameter = DotDiameter;
        var dotSpeed = DotSpeed;
        var dotDelayTime = DotDelayTime;

        if (dotCount < 1) return;
        PrivateCanvas.Children.Clear();

        var actualWidth = Bounds.Width;
        if (double.IsNaN(actualWidth) || actualWidth <= 0)
        {
            // Bounds 还没准备好；OnSizeChanged 之后会再触发一次
            return;
        }

        var centerWidth = dotDiameter * dotCount + dotInterval * (dotCount - 1) + MoveLength;
        var speedDownLength = (actualWidth - MoveLength) / 2;
        var speedUniformLength = centerWidth / 2;

        for (var i = 0; i < dotCount; i++)
        {
            var ellipse = CreateLineEllipse(i, dotInterval, dotDiameter);
            PrivateCanvas.Children.Add(ellipse);

            var startLeft = ellipse.Margin.Left;

            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(dotSpeed),
                Delay = TimeSpan.FromMilliseconds(dotDelayTime * i),
                IterationCount = IterationCount.Infinite,
                FillMode = FillMode.Forward,
                // WPF 用 PowerEase EaseOut/EaseIn + Linear，Avalonia 单缓动近似为 LinearEasing，
                // 关键帧自身位置已经表达了"快/匀/慢"的节奏。
                Easing = new LinearEasing(),
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0d),
                        Setters = { new Setter(MarginProperty, new Thickness(startLeft, 0, 0, 0)) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue((1 - UniformScale) / 2),
                        KeySpline = EaseOutSpline,
                        Setters = { new Setter(MarginProperty, new Thickness(speedDownLength + startLeft, 0, 0, 0)) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue((1 + UniformScale) / 2),
                        Setters = { new Setter(MarginProperty, new Thickness(speedDownLength + speedUniformLength + startLeft, 0, 0, 0)) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1d),
                        KeySpline = EaseInSpline,
                        Setters = { new Setter(MarginProperty, new Thickness(actualWidth + startLeft + speedUniformLength, 0, 0, 0)) }
                    }
                }
            };

            var cts = new CancellationTokenSource();
            AnimationTokens.Add(cts);
            _ = animation.RunAsync(ellipse, cts.Token);
        }
    }

    private Ellipse CreateLineEllipse(int index, double dotInterval, double dotDiameter)
    {
        var ellipse = base.CreateEllipse(index);
        ellipse.HorizontalAlignment = HorizontalAlignment.Left;
        ellipse.VerticalAlignment = VerticalAlignment.Top;
        ellipse.Margin = new Thickness(-(dotInterval + dotDiameter) * index, 0, 0, 0);
        return ellipse;
    }
}

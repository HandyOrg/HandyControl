using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace HandyControl.Controls;

public class LoadingLine : LoadingBase
{
    private const double MoveLength = 80;

    private const double UniformScale = .6;

    public LoadingLine()
    {
        SetBinding(HeightProperty, new Binding("DotDiameter") { Source = this });
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

        //计算相关尺寸
        var centerWidth = dotDiameter * dotCount + dotInterval * (dotCount - 1) + MoveLength;
        var speedDownLength = (ActualWidth - MoveLength) / 2;
        var speedUniformLength = centerWidth / 2;

        //定义动画
        Storyboard = new Storyboard
        {
            RepeatBehavior = RepeatBehavior.Forever
        };

        //创建圆点
        for (var i = 0; i < dotCount; i++)
        {
            var ellipse = CreateEllipse(i, dotInterval, dotDiameter);

            var frames = new ThicknessAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(dotDelayTime * i)
            };
            //开始位置
            var frame0 = new LinearThicknessKeyFrame
            {
                Value = new Thickness(ellipse.Margin.Left, 0, 0, 0),
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
            };

            //开始位置到匀速开始
            var frame1 = new EasingThicknessKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                Value = new Thickness(speedDownLength + ellipse.Margin.Left, 0, 0, 0),
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (1 - UniformScale) / 2))
            };

            //匀速开始到匀速结束
            var frame2 = new LinearThicknessKeyFrame
            {
                Value = new Thickness(speedDownLength + speedUniformLength + ellipse.Margin.Left, 0, 0, 0),
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (1 + UniformScale) / 2))
            };

            //匀速结束到匀加速结束
            var frame3 = new EasingThicknessKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseIn
                },
                Value = new Thickness(ActualWidth + ellipse.Margin.Left + speedUniformLength, 0, 0, 0),
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed))
            };

            frames.KeyFrames.Add(frame0);
            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);
            frames.KeyFrames.Add(frame3);

            Storyboard.SetTarget(frames, ellipse);
            Storyboard.SetTargetProperty(frames, new PropertyPath(MarginProperty));
            Storyboard.Children.Add(frames);

            PrivateCanvas.Children.Add(ellipse);
        }

        Storyboard.Begin();
        if (!IsRunning)
        {
            Storyboard.Pause();
        }
    }

    private Ellipse CreateEllipse(int index, double dotInterval, double dotDiameter)
    {
        var ellipse = base.CreateEllipse(index);
        ellipse.HorizontalAlignment = HorizontalAlignment.Left;
        ellipse.VerticalAlignment = VerticalAlignment.Top;
        ellipse.Margin = new Thickness(-(dotInterval + dotDiameter) * index, 0, 0, 0);
        return ellipse;
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;


namespace HandyControl.Controls;

public class LoadingCircle : LoadingBase
{
    public static readonly DependencyProperty DotOffSetProperty = DependencyProperty.Register(
        nameof(DotOffSet), typeof(double), typeof(LoadingCircle),
        new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double DotOffSet
    {
        get => (double) GetValue(DotOffSetProperty);
        set => SetValue(DotOffSetProperty, value);
    }

    public static readonly DependencyProperty NeedHiddenProperty = DependencyProperty.Register(
        nameof(NeedHidden), typeof(bool), typeof(LoadingCircle),
        new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public bool NeedHidden
    {
        get => (bool) GetValue(NeedHiddenProperty);
        set => SetValue(NeedHiddenProperty, ValueBoxes.BooleanBox(value));
    }

    static LoadingCircle()
    {
        DotSpeedProperty.OverrideMetadata(typeof(LoadingCircle),
            new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender));
        DotDelayTimeProperty.OverrideMetadata(typeof(LoadingCircle),
            new FrameworkPropertyMetadata(220.0, FrameworkPropertyMetadataOptions.AffectsRender));
    }

    protected sealed override void UpdateDots()
    {
        var dotCount = DotCount;
        var dotInterval = DotInterval;
        var dotSpeed = DotSpeed;
        var dotDelayTime = DotDelayTime;
        var needHidden = NeedHidden;

        if (dotCount < 1) return;
        PrivateCanvas.Children.Clear();

        //定义动画
        Storyboard = new Storyboard
        {
            RepeatBehavior = RepeatBehavior.Forever
        };

        //创建圆点
        for (var i = 0; i < dotCount; i++)
        {
            var border = CreateBorder(i, dotInterval, needHidden);

            var framesMove = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(dotDelayTime * i)
            };

            var subAngle = -dotInterval * i;

            //开始位置
            var frame0 = new LinearDoubleKeyFrame
            {
                Value = 0 + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
            };

            //开始位置到第一次匀速开始
            var frame1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                Value = 180 + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (0.75 / 7)))
            };

            //第一次匀速开始到第一次匀速结束
            var frame2 = new LinearDoubleKeyFrame
            {
                Value = 180 + DotOffSet + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (2.75 / 7)))
            };

            //第一次匀速结束到匀加速结束
            var frame3 = new EasingDoubleKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseIn
                },
                Value = 360 + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (3.5 / 7)))
            };

            //匀加速结束到匀减速结束
            var frame4 = new EasingDoubleKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                Value = 540 + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (4.25 / 7)))
            };

            //匀减速结束到第二次匀速结束
            var frame5 = new LinearDoubleKeyFrame
            {
                Value = 540 + DotOffSet + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (6.25 / 7)))
            };

            //第二次匀速结束到匀加速结束
            var frame6 = new EasingDoubleKeyFrame
            {
                EasingFunction = new PowerEase
                {
                    EasingMode = EasingMode.EaseIn
                },
                Value = 720 + subAngle,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed))
            };

            framesMove.KeyFrames.Add(frame0);
            framesMove.KeyFrames.Add(frame1);
            framesMove.KeyFrames.Add(frame2);
            framesMove.KeyFrames.Add(frame3);
            framesMove.KeyFrames.Add(frame4);
            framesMove.KeyFrames.Add(frame5);
            framesMove.KeyFrames.Add(frame6);

            Storyboard.SetTarget(framesMove, border);
            Storyboard.SetTargetProperty(framesMove, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            Storyboard.Children.Add(framesMove);

            if (NeedHidden)
            {
                //隐藏一段时间
                var frame7 = new DiscreteObjectKeyFrame
                {
                    Value = Visibility.Collapsed,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed))
                };

                var frame8 = new DiscreteObjectKeyFrame
                {
                    Value = Visibility.Collapsed,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed + 0.4))
                };

                var frame9 = new DiscreteObjectKeyFrame
                {
                    Value = Visibility.Visible,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
                };
                var framesVisibility = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(dotDelayTime * i)
                };
                framesVisibility.KeyFrames.Add(frame9);
                framesVisibility.KeyFrames.Add(frame7);
                framesVisibility.KeyFrames.Add(frame8);
                Storyboard.SetTarget(framesVisibility, border);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                Storyboard.Children.Add(framesVisibility);
            }
            PrivateCanvas.Children.Add(border);
        }

        Storyboard.Begin();
        if (!IsRunning)
        {
            Storyboard.Pause();
        }
    }

    private Border CreateBorder(int index, double dotInterval, bool needHidden)
    {
        var ellipse = CreateEllipse(index);
        ellipse.HorizontalAlignment = HorizontalAlignment.Center;
        ellipse.VerticalAlignment = VerticalAlignment.Bottom;
        var rt = new RotateTransform
        {
            Angle = -dotInterval * index
        };
        var myTransGroup = new TransformGroup();
        myTransGroup.Children.Add(rt);
        var border = new Border
        {
            RenderTransformOrigin = new Point(0.5, 0.5),
            RenderTransform = myTransGroup,
            Child = ellipse,
            Visibility = needHidden ? Visibility.Collapsed : Visibility.Visible
        };
        border.SetBinding(WidthProperty, new Binding("Width") { Source = this });
        border.SetBinding(HeightProperty, new Binding("Height") { Source = this });

        return border;
    }
}

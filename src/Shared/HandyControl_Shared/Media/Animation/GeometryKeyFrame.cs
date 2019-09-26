using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;

public abstract class GeometryKeyFrame : Freezable, IKeyFrame
{
    protected GeometryKeyFrame()
    {
        
    }

    protected GeometryKeyFrame(Geometry value)
    {
        AnimationHelper.DecomposeGeometryStr(value.ToString(), out var arr);
        Value = arr;
    }

    protected GeometryKeyFrame(Geometry value, KeyTime keyTime) : this(value)
    {
        KeyTime = keyTime;
    }

    public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(
        "KeyTime", typeof(KeyTime), typeof(GeometryKeyFrame), new PropertyMetadata(KeyTime.Uniform));

    public KeyTime KeyTime
    {
        get => (KeyTime) GetValue(KeyTimeProperty);
        set => SetValue(KeyTimeProperty, value);
    }

    object IKeyFrame.Value
    {
        get => Value;
        set => Value = (double[])value;
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(Geometry), typeof(GeometryKeyFrame), new PropertyMetadata(default(double[])));

    public double[] Value
    {
        get => (double[]) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected string[] Strings { get; set; }

    public Geometry InterpolateValue(Geometry baseValue, double keyFrameProgress)
    {
        if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));
        }

        return InterpolateValueCore(baseValue, keyFrameProgress);
    }

    protected abstract Geometry InterpolateValueCore(Geometry baseValue, double keyFrameProgress);
}
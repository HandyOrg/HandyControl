using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

public abstract class GeometryKeyFrame : Freezable, IKeyFrame
{
    protected GeometryKeyFrame()
    {
        
    }

    protected GeometryKeyFrame(Geometry value)
    {
        Value = value;
    }

    protected GeometryKeyFrame(Geometry value, KeyTime keyTime)
    {
        Value = value;
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
        set => Value = (Geometry)value;
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(Geometry), typeof(GeometryKeyFrame), new PropertyMetadata(default(Geometry)));

    public Geometry Value
    {
        get => (Geometry) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

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
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;

public abstract class GeometryKeyFrame : Freezable, IKeyFrame
{
    internal double[] Numbers;

    protected GeometryKeyFrame()
    {

    }

    protected GeometryKeyFrame(Geometry value)
    {
        AnimationHelper.DecomposeGeometryStr(value.ToString(CultureInfo.InvariantCulture), out var arr);
        Numbers = arr;
        Value = value;
    }

    protected GeometryKeyFrame(Geometry value, KeyTime keyTime) : this(value) => KeyTime = keyTime;

    public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(
        nameof(KeyTime), typeof(KeyTime), typeof(GeometryKeyFrame), new PropertyMetadata(KeyTime.Uniform));

    public KeyTime KeyTime
    {
        get => (KeyTime) GetValue(KeyTimeProperty);
        set => SetValue(KeyTimeProperty, value);
    }

    object IKeyFrame.Value
    {
        get => Value;
        set => Value = (Geometry) value;
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(Geometry), typeof(GeometryKeyFrame), new PropertyMetadata(default(Geometry), OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var obj = (GeometryKeyFrame) d;
        var v = (Geometry) e.NewValue;
        AnimationHelper.DecomposeGeometryStr(v.ToString(CultureInfo.InvariantCulture), out var arr);
        obj.Numbers = arr;
    }

    public Geometry Value
    {
        get => (Geometry) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double[] InterpolateValue(double[] baseValue, double keyFrameProgress)
    {
        if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));
        }

        return InterpolateValueCore(baseValue, keyFrameProgress);
    }

    protected abstract double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress);
}

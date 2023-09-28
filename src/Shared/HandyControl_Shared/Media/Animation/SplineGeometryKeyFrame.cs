using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation;

public class SplineGeometryKeyFrame : GeometryKeyFrame
{
    public SplineGeometryKeyFrame()
    {

    }

    public SplineGeometryKeyFrame(Geometry value) : base(value)
    {

    }

    public SplineGeometryKeyFrame(Geometry value, KeyTime keyTime) : base(value, keyTime)
    {

    }

    public SplineGeometryKeyFrame(Geometry value, KeyTime keyTime, KeySpline keySpline) : base(value, keyTime)
    {
        KeySpline = keySpline ?? throw new ArgumentNullException(nameof(keySpline));
    }

    protected override Freezable CreateInstanceCore() => new SplineGeometryKeyFrame();

    protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
    {
        if (MathHelper.IsVerySmall(keyFrameProgress))
        {
            return baseValue;
        }

        if (MathHelper.AreClose(keyFrameProgress, 1))
        {
            return Numbers;
        }

        var splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);
        return AnimationHelper.InterpolateGeometryValue(baseValue, Numbers, splineProgress);
    }

    public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register(
        nameof(KeySpline), typeof(KeySpline), typeof(SplineGeometryKeyFrame), new PropertyMetadata(new KeySpline()));

    public KeySpline KeySpline
    {
        get => (KeySpline) GetValue(KeySplineProperty);
        set => SetValue(KeySplineProperty, value);
    }
}

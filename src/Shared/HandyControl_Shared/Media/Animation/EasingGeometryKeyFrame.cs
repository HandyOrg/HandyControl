using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation;

public class EasingGeometryKeyFrame : GeometryKeyFrame
{
    public EasingGeometryKeyFrame()
    {

    }

    public EasingGeometryKeyFrame(Geometry value) : base(value)
    {

    }

    public EasingGeometryKeyFrame(Geometry value, KeyTime keyTime) : base(value, keyTime)
    {

    }

    public EasingGeometryKeyFrame(Geometry value, KeyTime keyTime, IEasingFunction easingFunction) : base(value, keyTime)
    {
        EasingFunction = easingFunction;
    }

    protected override Freezable CreateInstanceCore() => new EasingGeometryKeyFrame();

    protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
    {
        var easingFunction = EasingFunction;
        if (easingFunction != null)
        {
            keyFrameProgress = easingFunction.Ease(keyFrameProgress);
        }

        if (MathHelper.IsVerySmall(keyFrameProgress))
        {
            return baseValue;
        }

        if (MathHelper.AreClose(keyFrameProgress, 1))
        {
            return Numbers;
        }

        return AnimationHelper.InterpolateGeometryValue(baseValue, Numbers, keyFrameProgress);
    }

    public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
        nameof(EasingFunction), typeof(IEasingFunction), typeof(EasingGeometryKeyFrame), new PropertyMetadata(default(IEasingFunction)));

    public IEasingFunction EasingFunction
    {
        get => (IEasingFunction) GetValue(EasingFunctionProperty);
        set => SetValue(EasingFunctionProperty, value);
    }
}

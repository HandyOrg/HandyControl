using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation;

public class LinearGeometryKeyFrame : GeometryKeyFrame
{
    public LinearGeometryKeyFrame()
    {

    }

    public LinearGeometryKeyFrame(Geometry value) : base(value)
    {

    }

    public LinearGeometryKeyFrame(Geometry value, KeyTime keyTime) : base(value, keyTime)
    {

    }

    protected override Freezable CreateInstanceCore() => new LinearGeometryKeyFrame();

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

        return AnimationHelper.InterpolateGeometryValue(baseValue, Numbers, keyFrameProgress);
    }
}

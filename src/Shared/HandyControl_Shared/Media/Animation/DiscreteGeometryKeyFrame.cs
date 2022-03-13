using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HandyControl.Media.Animation;

public class DiscreteGeometryKeyFrame : GeometryKeyFrame
{
    public DiscreteGeometryKeyFrame()
    {
    }

    public DiscreteGeometryKeyFrame(Geometry value) : base(value)
    {
    }

    public DiscreteGeometryKeyFrame(Geometry value, KeyTime keyTime) : base(value, keyTime)
    {
    }

    protected override Freezable CreateInstanceCore() => new DiscreteGeometryKeyFrame();

    protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress) => keyFrameProgress < 1.0 ? baseValue : Numbers;
}

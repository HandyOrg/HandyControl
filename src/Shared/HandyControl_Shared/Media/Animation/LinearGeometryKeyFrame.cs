using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation
{
    public class LinearGeometryKeyFrame : GeometryKeyFrame
    {
        private double[] _baseValues;

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

        protected override Geometry InterpolateValueCore(Geometry baseValue, double keyFrameProgress)
        {
            if (_baseValues == null)
            {
                AnimationHelper.DecomposeGeometryStr(baseValue.ToString(), out _baseValues);
            }

            if (MathHelper.IsVerySmall(keyFrameProgress))
            {
                return baseValue;
            }

            if (MathHelper.AreClose(keyFrameProgress, 1))
            {
                return AnimationHelper.ComposeGeometry(Strings, Value);
            }

            return AnimationHelper.InterpolateGeometry(_baseValues, Value, keyFrameProgress, Strings);
        }
    }
}

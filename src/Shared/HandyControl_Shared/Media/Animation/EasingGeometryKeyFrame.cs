using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation
{
    public class EasingGeometryKeyFrame : GeometryKeyFrame
    {
        private double[] _baseValues;

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

        protected override Geometry InterpolateValueCore(Geometry baseValue, double keyFrameProgress)
        {
            if (_baseValues == null)
            {
                AnimationHelper.DecomposeGeometryStr(baseValue.ToString(), out _baseValues);
            }

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
                return AnimationHelper.ComposeGeometry(Strings, Value);
            }

            return AnimationHelper.InterpolateGeometry(_baseValues, Value, keyFrameProgress, Strings);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            "EasingFunction", typeof(IEasingFunction), typeof(EasingGeometryKeyFrame), new PropertyMetadata(default(IEasingFunction)));

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction) GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }
    }
}

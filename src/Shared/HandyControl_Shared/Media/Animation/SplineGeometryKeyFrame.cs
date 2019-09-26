using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Media.Animation
{
    public class SplineGeometryKeyFrame : GeometryKeyFrame
    {
        private double[] _baseValues;

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

            var splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);
            return AnimationHelper.InterpolateGeometry(_baseValues, Value, splineProgress, Strings);
        }

        public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register(
            "KeySpline", typeof(KeySpline), typeof(SplineGeometryKeyFrame), new PropertyMetadata(new KeySpline()));

        public KeySpline KeySpline
        {
            get => (KeySpline) GetValue(KeySplineProperty);
            set => SetValue(KeySplineProperty, value);
        }
    }
}

using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools.Extension;
// ReSharper disable PossibleInvalidOperationException

namespace HandyControl.Media.Animation
{
    public class GeometryAnimation : GeometryAnimationBase
    {
        private const string Rgx = @"[+-]?\d*\.?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?";

        private string[] _strings;

        private double[] _numbersFrom;

        private double[] _numbersTo;

        private double[] _numbersAccumulator;

        private double[][] _keyValues;

        private bool _isAnimationFunctionValid;

        public GeometryAnimation()
        {

        }

        public GeometryAnimation(string fromValue, string toValue) : this()
        {
            From = Geometry.Parse(fromValue);
            To = Geometry.Parse(toValue);
        }

        private void UpdateValue()
        {
            if (_numbersFrom == null || _numbersTo == null || _numbersFrom.Length != _numbersTo.Length) return;

            _numbersAccumulator = new double[_numbersFrom.Length];
            for (var i = 0; i < _numbersFrom.Length; i++)
            {
                _numbersAccumulator[i] = _numbersTo[i] - _numbersFrom[i];
            }
        }

        private static void DecomposeValue(string geometryStr, out double[] arr)
        {
            var collection = Regex.Matches(geometryStr, Rgx);
            arr = new double[collection.Count];
            for (var i = 0; i < collection.Count; i++)
            {
                arr[i] = collection[i].Value.Value<double>();
            }
        }

        public GeometryAnimation(Geometry fromValue, Geometry toValue) : this()
        {
            From = fromValue;
            To = toValue;
        }

        public GeometryAnimation(Geometry fromValue, Geometry toValue, Duration duration) : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }

        public GeometryAnimation(Geometry fromValue, Geometry toValue, Duration duration, FillBehavior fillBehavior) : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        public new GeometryAnimation Clone() => (GeometryAnimation)base.Clone();

        protected override Freezable CreateInstanceCore() => new GeometryAnimation();

        private Geometry ConvertToGeometry(double[] arr)
        {
            var builder = new StringBuilder(_strings[0]);
            for (var i = 0; i < _numbersAccumulator.Length; i++)
            {
                var s = _strings[i + 1];
                var n = arr[i];
                if (!double.IsNaN(n))
                {
                    builder.Append(n).Append(s);
                }
            }

            return Geometry.Parse(builder.ToString());
        }

        protected override Geometry GetCurrentValueCore(Geometry defaultOriginValue, Geometry defaultDestinationValue, AnimationClock animationClock)
        {
            if (!_isAnimationFunctionValid)
            {
                ValidateAnimationFunction();
            }

            var progress = animationClock.CurrentProgress.Value;

            var easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }

            var accumulated = new double[_numbersAccumulator.Length];
            var foundation = new double[_numbersAccumulator.Length];

            var from = _keyValues[0];

            if (IsAdditive)
            {
                foundation = new double[_numbersAccumulator.Length];
            }


            if (IsCumulative)
            {
                var currentRepeat = (double)(animationClock.CurrentIteration - 1);

                if (currentRepeat > 0.0)
                {
                    accumulated = new double[_numbersAccumulator.Length];
                    for (var i = 0; i < _numbersAccumulator.Length; i++)
                    {
                        accumulated[i] = _numbersAccumulator[i] * currentRepeat;
                    }
                }
            }

            var numbers = new double[_numbersAccumulator.Length];

            for (var i = 0; i < _numbersAccumulator.Length; i++)
            {
                numbers[i] = foundation[i] + accumulated[i] + from[i] + _numbersAccumulator[i] * progress;
            }

            return ConvertToGeometry(numbers);
        }

        private void ValidateAnimationFunction()
        {
            _keyValues = null;

            _keyValues = new double[2][];
            _keyValues[0] = _numbersFrom;
            _keyValues[1] = _numbersTo;

            _isAnimationFunctionValid = true;
        }

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", typeof(Geometry), typeof(GeometryAnimation), new PropertyMetadata(default(Geometry), OnFromChanged));

        private static void OnFromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (GeometryAnimation) d;
            if (e.NewValue is Geometry geometry)
            {
                DecomposeValue(geometry.ToString(), out obj._numbersFrom);
                obj.UpdateValue();
            }
        }

        public Geometry From
        {
            get => (Geometry)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(Geometry), typeof(GeometryAnimation), new PropertyMetadata(default(Geometry), OnToChanged));

        private static void OnToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (GeometryAnimation)d;
            if (e.NewValue is Geometry geometry)
            {
                var geometryStr = geometry.ToString();
                DecomposeValue(geometryStr, out obj._numbersTo);
                obj._strings = Regex.Split(geometryStr, Rgx);
                obj.UpdateValue();
            }
        }

        public Geometry To
        {
            get => (Geometry)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            "EasingFunction", typeof(IEasingFunction), typeof(GeometryAnimation), new PropertyMetadata(default(IEasingFunction)));

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public bool IsAdditive
        {
            get => (bool)GetValue(IsAdditiveProperty);
            set => SetValue(IsAdditiveProperty, value);
        }

        public bool IsCumulative
        {
            get => (bool)GetValue(IsCumulativeProperty);
            set => SetValue(IsCumulativeProperty, value);
        }
    }
}
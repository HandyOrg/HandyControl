using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Tools;

// ReSharper disable PossibleInvalidOperationException
namespace HandyControl.Media.Animation;

public class GeometryAnimation : GeometryAnimationBase
{
    private string[] _strings;

    private double[] _numbersFrom;

    private double[] _numbersTo;

    private double[] _numbersAccumulator;

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

    public new GeometryAnimation Clone() => (GeometryAnimation) base.Clone();

    protected override Freezable CreateInstanceCore() => new GeometryAnimation();

    protected override Geometry GetCurrentValueCore(Geometry defaultOriginValue, Geometry defaultDestinationValue, AnimationClock animationClock)
    {
        if (_numbersAccumulator == null)
        {
            if (_numbersFrom == null)
            {
                var geometryStr = defaultOriginValue.ToString(CultureInfo.InvariantCulture);
                AnimationHelper.DecomposeGeometryStr(geometryStr, out _numbersFrom);
            }

            if (_numbersTo == null)
            {
                var geometryStr = defaultDestinationValue.ToString(CultureInfo.InvariantCulture);
                AnimationHelper.DecomposeGeometryStr(geometryStr, out _numbersTo);
                _strings = Regex.Split(geometryStr, RegexPatterns.DigitsPattern);
            }

            UpdateValue();
        }

        if (_numbersAccumulator == null) return defaultOriginValue;

        var progress = animationClock.CurrentProgress.Value;

        var easingFunction = EasingFunction;
        if (easingFunction != null)
        {
            progress = easingFunction.Ease(progress);
        }

        var accumulated = new double[_numbersAccumulator.Length];

        if (IsCumulative)
        {
            var currentRepeat = (double) (animationClock.CurrentIteration - 1);

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
            numbers[i] = accumulated[i] + _numbersFrom[i] + _numbersAccumulator[i] * progress;
        }

        return AnimationHelper.ComposeGeometry(_strings, numbers);
    }

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
        nameof(From), typeof(Geometry), typeof(GeometryAnimation), new PropertyMetadata(default(Geometry), OnFromChanged));

    private static void OnFromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var obj = (GeometryAnimation) d;
        if (e.NewValue is Geometry geometry)
        {
            AnimationHelper.DecomposeGeometryStr(geometry.ToString(CultureInfo.InvariantCulture), out obj._numbersFrom);
            obj.UpdateValue();
        }
    }

    public Geometry From
    {
        get => (Geometry) GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
        nameof(To), typeof(Geometry), typeof(GeometryAnimation), new PropertyMetadata(default(Geometry), OnToChanged));

    private static void OnToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var obj = (GeometryAnimation) d;
        if (e.NewValue is Geometry geometry)
        {
            var geometryStr = geometry.ToString(CultureInfo.InvariantCulture);
            AnimationHelper.DecomposeGeometryStr(geometryStr, out obj._numbersTo);
            obj._strings = Regex.Split(geometryStr, RegexPatterns.DigitsPattern);
            obj.UpdateValue();
        }
    }

    public Geometry To
    {
        get => (Geometry) GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
        nameof(EasingFunction), typeof(IEasingFunction), typeof(GeometryAnimation), new PropertyMetadata(default(IEasingFunction)));

    public IEasingFunction EasingFunction
    {
        get => (IEasingFunction) GetValue(EasingFunctionProperty);
        set => SetValue(EasingFunctionProperty, value);
    }

    public bool IsCumulative
    {
        get => (bool) GetValue(IsCumulativeProperty);
        set => SetValue(IsCumulativeProperty, ValueBoxes.BooleanBox(value));
    }
}

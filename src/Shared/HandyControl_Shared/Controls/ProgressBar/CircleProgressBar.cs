using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControl.Expression.Shapes;

namespace HandyControl.Controls;

[TemplatePart(Name = IndicatorTemplateName, Type = typeof(Arc))]
public class CircleProgressBar : RangeBase
{
    private const string IndicatorTemplateName = "PART_Indicator";

    public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register(
        nameof(ArcThickness), typeof(double), typeof(CircleProgressBar), new PropertyMetadata(ValueBoxes.Double0Box));

    public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
        nameof(ShowText), typeof(bool), typeof(CircleProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(CircleProgressBar), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty IsIndeterminateProperty =
        ProgressBar.IsIndeterminateProperty.AddOwner(typeof(CircleProgressBar),
            new FrameworkPropertyMetadata(ValueBoxes.FalseBox));

    private Arc _indicator;

    static CircleProgressBar()
    {
        FocusableProperty.OverrideMetadata(typeof(CircleProgressBar),
            new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
        MaximumProperty.OverrideMetadata(typeof(CircleProgressBar),
            new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
    }

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowText
    {
        get => (bool) GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, ValueBoxes.BooleanBox(value));
    }

    public double ArcThickness
    {
        get => (double) GetValue(ArcThicknessProperty);
        set => SetValue(ArcThicknessProperty, value);
    }

    public bool IsIndeterminate
    {
        get => (bool) GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, ValueBoxes.BooleanBox(value));
    }

    private void SetProgressBarIndicatorAngle()
    {
        if (_indicator == null) return;
        var minimum = Minimum;
        var maximum = Maximum;
        var num = Value;
        _indicator.EndAngle = (maximum <= minimum ? 0 : (num - minimum) / (maximum - minimum)) * 360;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _indicator = GetTemplateChild(IndicatorTemplateName) as Arc;
        if (_indicator != null)
        {
            _indicator.StartAngle = 0;
            _indicator.EndAngle = 0;
        }

        SetProgressBarIndicatorAngle();
    }

    protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
        base.OnMinimumChanged(oldMinimum, newMinimum);
        SetProgressBarIndicatorAngle();
    }

    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
        base.OnMaximumChanged(oldMaximum, newMaximum);
        SetProgressBarIndicatorAngle();
    }

    protected override void OnValueChanged(double oldValue, double newValue)
    {
        base.OnValueChanged(oldValue, newValue);
        SetProgressBarIndicatorAngle();
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

namespace HandyControl.Controls;

[TemplatePart(IndicatorTemplateName, typeof(Arc))]
public class CircleProgressBar : RangeBase
{
    private const string IndicatorTemplateName = "PART_Indicator";

    public static readonly StyledProperty<double> ArcThicknessProperty =
        AvaloniaProperty.Register<CircleProgressBar, double>(nameof(ArcThickness));

    public static readonly StyledProperty<bool> ShowTextProperty =
        AvaloniaProperty.Register<CircleProgressBar, bool>(nameof(ShowText), true);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<CircleProgressBar, string?>(nameof(Text));

    public static readonly StyledProperty<bool> IsIndeterminateProperty =
        AvaloniaProperty.Register<CircleProgressBar, bool>(nameof(IsIndeterminate));

    private Arc? _indicator;

    static CircleProgressBar()
    {
        FocusableProperty.OverrideDefaultValue<CircleProgressBar>(false);
        MaximumProperty.OverrideDefaultValue<CircleProgressBar>(100d);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public double ArcThickness
    {
        get => GetValue(ArcThicknessProperty);
        set => SetValue(ArcThicknessProperty, value);
    }

    public bool IsIndeterminate
    {
        get => GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }

    private void SetProgressBarIndicatorAngle()
    {
        if (_indicator == null) return;
        var minimum = Minimum;
        var maximum = Maximum;
        var num = Value;
        _indicator.StartAngle = 0;
        _indicator.SweepAngle = (maximum <= minimum ? 0 : (num - minimum) / (maximum - minimum)) * 360;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _indicator = e.NameScope.Find<Arc>(IndicatorTemplateName);
        SetProgressBarIndicatorAngle();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MinimumProperty
            || change.Property == MaximumProperty
            || change.Property == ValueProperty)
        {
            SetProgressBarIndicatorAngle();
        }
    }
}

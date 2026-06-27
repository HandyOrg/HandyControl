using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace HandyControl.Controls;

/// <summary>
///     波浪进度条
/// </summary>
[TemplatePart(ElementWave, typeof(Control))]
[TemplatePart(ElementClip, typeof(Control))]
public class WaveProgressBar : RangeBase
{
    private const string ElementWave = "PART_Wave";

    private const string ElementClip = "PART_Clip";

    private const double TranslateTransformMinY = -20;

    private Control? _waveElement;

    private double _translateTransformYRange;

    private TranslateTransform? _yTransform;

    static WaveProgressBar()
    {
        FocusableProperty.OverrideDefaultValue<WaveProgressBar>(false);
        MaximumProperty.OverrideDefaultValue<WaveProgressBar>(100d);
    }

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<WaveProgressBar, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<bool> ShowTextProperty =
        AvaloniaProperty.Register<WaveProgressBar, bool>(nameof(ShowText), true);

    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public static readonly StyledProperty<IBrush?> WaveFillProperty =
        AvaloniaProperty.Register<WaveProgressBar, IBrush?>(nameof(WaveFill));

    public IBrush? WaveFill
    {
        get => GetValue(WaveFillProperty);
        set => SetValue(WaveFillProperty, value);
    }

    public static readonly StyledProperty<double> WaveThicknessProperty =
        AvaloniaProperty.Register<WaveProgressBar, double>(nameof(WaveThickness));

    public double WaveThickness
    {
        get => GetValue(WaveThicknessProperty);
        set => SetValue(WaveThicknessProperty, value);
    }

    public static readonly StyledProperty<IBrush?> WaveStrokeProperty =
        AvaloniaProperty.Register<WaveProgressBar, IBrush?>(nameof(WaveStroke));

    public IBrush? WaveStroke
    {
        get => GetValue(WaveStrokeProperty);
        set => SetValue(WaveStrokeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _waveElement = e.NameScope.Find<Control>(ElementWave);
        var clipElement = e.NameScope.Find<Control>(ElementClip);

        if (_waveElement != null && clipElement != null)
        {
            var clipElementHeight = clipElement.Height;

            _yTransform = new TranslateTransform { Y = clipElementHeight };
            _translateTransformYRange = clipElementHeight - TranslateTransformMinY;
            _waveElement.RenderTransform = _yTransform;

            UpdateWave(Value);
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty
            || change.Property == MinimumProperty
            || change.Property == MaximumProperty)
        {
            UpdateWave(Value);
        }
    }

    private void UpdateWave(double value)
    {
        if (_yTransform == null) return;
        var maximum = Maximum;
        if (Math.Abs(maximum) < 1e-6) return;
        var scale = 1 - value / maximum;
        var y = _translateTransformYRange * scale + TranslateTransformMinY;
        _yTransform.Y = y;
    }
}

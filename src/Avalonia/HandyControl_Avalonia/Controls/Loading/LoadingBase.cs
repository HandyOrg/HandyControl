using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Media;

namespace HandyControl.Controls;

public abstract class LoadingBase : ContentControl
{
    protected readonly List<CancellationTokenSource> AnimationTokens = new();

    public static readonly StyledProperty<bool> IsRunningProperty =
        AvaloniaProperty.Register<LoadingBase, bool>(nameof(IsRunning), true);

    public static readonly StyledProperty<int> DotCountProperty =
        AvaloniaProperty.Register<LoadingBase, int>(nameof(DotCount), 5);

    public static readonly StyledProperty<double> DotIntervalProperty =
        AvaloniaProperty.Register<LoadingBase, double>(nameof(DotInterval), 10d);

    public static readonly StyledProperty<IBrush?> DotBorderBrushProperty =
        AvaloniaProperty.Register<LoadingBase, IBrush?>(nameof(DotBorderBrush));

    public static readonly StyledProperty<double> DotBorderThicknessProperty =
        AvaloniaProperty.Register<LoadingBase, double>(nameof(DotBorderThickness), 0d);

    public static readonly StyledProperty<double> DotDiameterProperty =
        AvaloniaProperty.Register<LoadingBase, double>(nameof(DotDiameter), 6d);

    public static readonly StyledProperty<double> DotSpeedProperty =
        AvaloniaProperty.Register<LoadingBase, double>(nameof(DotSpeed), 4d);

    public static readonly StyledProperty<double> DotDelayTimeProperty =
        AvaloniaProperty.Register<LoadingBase, double>(nameof(DotDelayTime), 80d);

    protected readonly Canvas PrivateCanvas = new()
    {
        ClipToBounds = true
    };

    protected LoadingBase()
    {
        Content = PrivateCanvas;
    }

    public bool IsRunning
    {
        get => GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    public int DotCount
    {
        get => GetValue(DotCountProperty);
        set => SetValue(DotCountProperty, value);
    }

    public double DotInterval
    {
        get => GetValue(DotIntervalProperty);
        set => SetValue(DotIntervalProperty, value);
    }

    public IBrush? DotBorderBrush
    {
        get => GetValue(DotBorderBrushProperty);
        set => SetValue(DotBorderBrushProperty, value);
    }

    public double DotBorderThickness
    {
        get => GetValue(DotBorderThicknessProperty);
        set => SetValue(DotBorderThicknessProperty, value);
    }

    public double DotDiameter
    {
        get => GetValue(DotDiameterProperty);
        set => SetValue(DotDiameterProperty, value);
    }

    public double DotSpeed
    {
        get => GetValue(DotSpeedProperty);
        set => SetValue(DotSpeedProperty, value);
    }

    public double DotDelayTime
    {
        get => GetValue(DotDelayTimeProperty);
        set => SetValue(DotDelayTimeProperty, value);
    }

    protected abstract void UpdateDots();

    protected void StopAnimations()
    {
        foreach (var cts in AnimationTokens)
        {
            try { cts.Cancel(); cts.Dispose(); } catch { }
        }
        AnimationTokens.Clear();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        if (IsRunning)
        {
            StopAnimations();
            UpdateDots();
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsRunningProperty)
        {
            if (IsRunning)
            {
                UpdateDots();
            }
            else
            {
                StopAnimations();
            }
            return;
        }

        if (change.Property == DotCountProperty
            || change.Property == DotIntervalProperty
            || change.Property == DotBorderBrushProperty
            || change.Property == DotBorderThicknessProperty
            || change.Property == DotDiameterProperty
            || change.Property == DotSpeedProperty
            || change.Property == DotDelayTimeProperty
            || change.Property == ForegroundProperty)
        {
            if (IsRunning)
            {
                StopAnimations();
                UpdateDots();
            }
        }
    }

    protected override void OnLoaded(Avalonia.Interactivity.RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (IsRunning)
        {
            StopAnimations();
            UpdateDots();
        }
    }

    protected override void OnUnloaded(Avalonia.Interactivity.RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        StopAnimations();
    }

    protected virtual Ellipse CreateEllipse(int index)
    {
        var ellipse = new Ellipse();
        ellipse.Bind(WidthProperty, new Binding(nameof(DotDiameter)) { Source = this });
        ellipse.Bind(HeightProperty, new Binding(nameof(DotDiameter)) { Source = this });
        ellipse.Bind(Shape.FillProperty, new Binding(nameof(Foreground)) { Source = this });
        ellipse.Bind(Shape.StrokeThicknessProperty, new Binding(nameof(DotBorderThickness)) { Source = this });
        ellipse.Bind(Shape.StrokeProperty, new Binding(nameof(DotBorderBrush)) { Source = this });
        return ellipse;
    }
}

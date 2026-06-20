using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace HandyControl.Controls;

public class ProgressButton : ToggleButton
{
    public static readonly StyledProperty<ControlTheme?> ProgressThemeProperty =
        AvaloniaProperty.Register<ProgressButton, ControlTheme?>(nameof(ProgressTheme));

    public static readonly StyledProperty<double> ProgressProperty =
        AvaloniaProperty.Register<ProgressButton, double>(nameof(Progress));

    public ControlTheme? ProgressTheme
    {
        get => GetValue(ProgressThemeProperty);
        set => SetValue(ProgressThemeProperty, value);
    }

    public double Progress
    {
        get => GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }
}
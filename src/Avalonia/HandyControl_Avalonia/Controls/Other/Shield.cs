using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;

namespace HandyControl.Controls;

/// <summary>
///     徽章
/// </summary>
public class Shield : Button
{
    public static readonly StyledProperty<string?> SubjectProperty =
        AvaloniaProperty.Register<Shield, string?>(nameof(Subject));

    public string? Subject
    {
        get => GetValue(SubjectProperty);
        set => SetValue(SubjectProperty, value);
    }

    public static readonly StyledProperty<object?> StatusProperty =
        AvaloniaProperty.Register<Shield, object?>(nameof(Status));

    [Content]
    public object? Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ColorProperty =
        AvaloniaProperty.Register<Shield, IBrush?>(nameof(Color));

    public IBrush? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
}

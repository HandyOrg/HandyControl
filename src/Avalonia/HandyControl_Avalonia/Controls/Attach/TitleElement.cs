using Avalonia;
using Avalonia.Media;

namespace HandyControl.Controls;

public class TitleElement
{
    public static readonly AttachedProperty<IBrush?> BorderBrushProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("BorderBrush", inherits: true);

    public static void SetBorderBrush(AvaloniaObject element, IBrush? value) =>
        element.SetValue(BorderBrushProperty, value);

    public static IBrush? GetBorderBrush(AvaloniaObject element) => element.GetValue(BorderBrushProperty);

    public static readonly AttachedProperty<IBrush?> BackgroundProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("Background", inherits: true);

    public static void SetBackground(AvaloniaObject element, IBrush? value) =>
        element.SetValue(BackgroundProperty, value);

    public static IBrush? GetBackground(AvaloniaObject element) => element.GetValue(BackgroundProperty);

    public static readonly AttachedProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.RegisterAttached<TitleElement, AvaloniaObject, IBrush?>("Foreground", inherits: true);

    public static void SetForeground(AvaloniaObject element, IBrush? value) =>
        element.SetValue(ForegroundProperty, value);

    public static IBrush? GetForeground(AvaloniaObject element) => element.GetValue(ForegroundProperty);
}

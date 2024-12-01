using Avalonia;
using Avalonia.Media;

namespace HandyControl.Controls;

public class VisualElement
{
    public static readonly AttachedProperty<IBrush> HighlightBrushProperty =
        AvaloniaProperty.RegisterAttached<VisualElement, AvaloniaObject, IBrush>("HighlightBrush", inherits: true);

    public static void SetHighlightBrush(AvaloniaObject element, IBrush value) =>
        element.SetValue(HighlightBrushProperty, value);

    public static IBrush GetHighlightBrush(AvaloniaObject element) => element.GetValue(HighlightBrushProperty);

    public static readonly AttachedProperty<IBrush> HighlightBackgroundProperty =
        AvaloniaProperty.RegisterAttached<VisualElement, AvaloniaObject, IBrush>("HighlightBackground", inherits: true);

    public static void SetHighlightBackground(AvaloniaObject element, IBrush value) =>
        element.SetValue(HighlightBackgroundProperty, value);

    public static IBrush GetHighlightBackground(AvaloniaObject element) =>
        element.GetValue(HighlightBackgroundProperty);

    public static readonly AttachedProperty<IBrush> HighlightBorderBrushProperty =
        AvaloniaProperty.RegisterAttached<VisualElement, AvaloniaObject, IBrush>("HighlightBorderBrush",
            inherits: true);

    public static void SetHighlightBorderBrush(AvaloniaObject element, IBrush value) =>
        element.SetValue(HighlightBorderBrushProperty, value);

    public static IBrush GetHighlightBorderBrush(AvaloniaObject element) =>
        element.GetValue(HighlightBorderBrushProperty);

    public static readonly AttachedProperty<IBrush> HighlightForegroundProperty =
        AvaloniaProperty.RegisterAttached<VisualElement, AvaloniaObject, IBrush>("HighlightForeground", inherits: true);

    public static void SetHighlightForeground(AvaloniaObject element, IBrush value) =>
        element.SetValue(HighlightForegroundProperty, value);

    public static IBrush GetHighlightForeground(AvaloniaObject element) =>
        element.GetValue(HighlightForegroundProperty);

    public static readonly AttachedProperty<string> TextProperty =
        AvaloniaProperty.RegisterAttached<VisualElement, AvaloniaObject, string>("Text");

    public static void SetText(AvaloniaObject element, string value) => element.SetValue(TextProperty, value);

    public static string GetText(AvaloniaObject element) => element.GetValue(TextProperty);
}

using Avalonia;
using Avalonia.Media;

namespace HandyControl.Controls;

public class BackgroundSwitchElement
{
    public static readonly AttachedProperty<IBrush> MouseHoverBackgroundProperty =
        AvaloniaProperty.RegisterAttached<BackgroundSwitchElement, AvaloniaObject, IBrush>("MouseHoverBackground",
            defaultValue: Brushes.Transparent, inherits: true);

    public static void SetMouseHoverBackground(AvaloniaObject element, IBrush value) =>
        element.SetValue(MouseHoverBackgroundProperty, value);

    public static IBrush GetMouseHoverBackground(AvaloniaObject element) =>
        element.GetValue(MouseHoverBackgroundProperty);

    public static readonly AttachedProperty<IBrush> MouseHoverForegroundProperty =
        AvaloniaProperty.RegisterAttached<BackgroundSwitchElement, AvaloniaObject, IBrush>("MouseHoverForeground",
            defaultValue: Brushes.Transparent, inherits: true);

    public static void SetMouseHoverForeground(AvaloniaObject element, IBrush value) =>
        element.SetValue(MouseHoverForegroundProperty, value);

    public static IBrush GetMouseHoverForeground(AvaloniaObject element) =>
        element.GetValue(MouseHoverForegroundProperty);

    public static readonly AttachedProperty<IBrush> MouseDownBackgroundProperty =
        AvaloniaProperty.RegisterAttached<BackgroundSwitchElement, AvaloniaObject, IBrush>("MouseDownBackground",
            defaultValue: Brushes.Transparent, inherits: true);

    public static void SetMouseDownBackground(AvaloniaObject element, IBrush value) =>
        element.SetValue(MouseDownBackgroundProperty, value);

    public static IBrush GetMouseDownBackground(AvaloniaObject element) =>
        element.GetValue(MouseDownBackgroundProperty);
}

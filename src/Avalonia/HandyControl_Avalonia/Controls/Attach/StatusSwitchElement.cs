using Avalonia;

namespace HandyControl.Controls;

public class StatusSwitchElement
{
    public static readonly AttachedProperty<object> CheckedElementProperty =
        AvaloniaProperty.RegisterAttached<StatusSwitchElement, AvaloniaObject, object>("CheckedElement");

    public static void SetCheckedElement(AvaloniaObject element, object value) =>
        element.SetValue(CheckedElementProperty, value);

    public static object GetCheckedElement(AvaloniaObject element) => element.GetValue(CheckedElementProperty);

    public static readonly AttachedProperty<object> HideUncheckedElementProperty =
        AvaloniaProperty.RegisterAttached<StatusSwitchElement, AvaloniaObject, object>("HideUncheckedElement");

    public static void SetHideUncheckedElement(AvaloniaObject element, object value) =>
        element.SetValue(HideUncheckedElementProperty, value);

    public static object GetHideUncheckedElement(AvaloniaObject element) =>
        element.GetValue(HideUncheckedElementProperty);
}

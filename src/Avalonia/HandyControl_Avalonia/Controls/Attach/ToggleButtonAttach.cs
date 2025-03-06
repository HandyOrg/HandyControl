using Avalonia;

namespace HandyControl.Controls;

public class ToggleButtonAttach
{
    public static readonly AttachedProperty<bool> ShowLabelProperty =
        AvaloniaProperty.RegisterAttached<ToggleButtonAttach, AvaloniaObject, bool>("ShowLabel", inherits: true);

    public static void SetShowLabel(AvaloniaObject element, bool value) => element.SetValue(ShowLabelProperty, value);

    public static bool GetShowLabel(AvaloniaObject element) => element.GetValue(ShowLabelProperty);
}

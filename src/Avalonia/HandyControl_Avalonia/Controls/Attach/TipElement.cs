using Avalonia;
using HandyControl.Data;

namespace HandyControl.Controls;

public class TipElement
{
    public static readonly AttachedProperty<bool> IsVisibleProperty =
        AvaloniaProperty.RegisterAttached<TipElement, AvaloniaObject, bool>("IsVisible");

    public static void SetIsVisible(AvaloniaObject element, bool value) => element.SetValue(IsVisibleProperty, value);

    public static bool GetIsVisible(AvaloniaObject element) => element.GetValue(IsVisibleProperty);

    public static readonly AttachedProperty<PlacementType> PlacementProperty =
        AvaloniaProperty.RegisterAttached<TipElement, AvaloniaObject, PlacementType>("Placement");

    public static void SetPlacement(AvaloniaObject element, PlacementType value) =>
        element.SetValue(PlacementProperty, value);

    public static PlacementType GetPlacement(AvaloniaObject element) => element.GetValue(PlacementProperty);

    public static readonly AttachedProperty<string> StringFormatProperty =
        AvaloniaProperty.RegisterAttached<TipElement, AvaloniaObject, string>("StringFormat", defaultValue: "N1");

    public static void SetStringFormat(AvaloniaObject element, string value) =>
        element.SetValue(StringFormatProperty, value);

    public static string GetStringFormat(AvaloniaObject element) => element.GetValue(StringFormatProperty);
}

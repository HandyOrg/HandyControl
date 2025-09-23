using Avalonia;

namespace HandyControl.Controls;

public class MenuAttach
{
    public static readonly AttachedProperty<double> PopupVerticalOffsetProperty =
        AvaloniaProperty.RegisterAttached<MenuAttach, AvaloniaObject, double>("PopupVerticalOffset", inherits: true);

    public static void SetPopupVerticalOffset(AvaloniaObject element, double value) =>
        element.SetValue(PopupVerticalOffsetProperty, value);

    public static double GetPopupVerticalOffset(AvaloniaObject element) =>
        element.GetValue(PopupVerticalOffsetProperty);

    public static readonly AttachedProperty<double> PopupHorizontalOffsetProperty =
        AvaloniaProperty.RegisterAttached<MenuAttach, AvaloniaObject, double>("PopupHorizontalOffset", inherits: true);

    public static void SetPopupHorizontalOffset(AvaloniaObject element, double value) =>
        element.SetValue(PopupHorizontalOffsetProperty, value);

    public static double GetPopupHorizontalOffset(AvaloniaObject element) =>
        element.GetValue(PopupHorizontalOffsetProperty);

    public static readonly AttachedProperty<Thickness> ItemPaddingProperty =
        AvaloniaProperty.RegisterAttached<MenuAttach, AvaloniaObject, Thickness>("ItemPadding", inherits: true);

    public static void SetItemPadding(AvaloniaObject element, Thickness value) =>
        element.SetValue(ItemPaddingProperty, value);

    public static Thickness GetItemPadding(AvaloniaObject element) => element.GetValue(ItemPaddingProperty);

    public static readonly AttachedProperty<double> ItemMinHeightProperty =
        AvaloniaProperty.RegisterAttached<MenuAttach, AvaloniaObject, double>("ItemMinHeight", inherits: true);

    public static void SetItemMinHeight(AvaloniaObject element, double value) =>
        element.SetValue(ItemMinHeightProperty, value);

    public static double GetItemMinHeight(AvaloniaObject element) => element.GetValue(ItemMinHeightProperty);
}

using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class MenuAttach
{
    public static readonly DependencyProperty PopupVerticalOffsetProperty = DependencyProperty.RegisterAttached(
        "PopupVerticalOffset", typeof(double), typeof(MenuAttach), new PropertyMetadata(ValueBoxes.Double0Box));

    public static void SetPopupVerticalOffset(DependencyObject element, double value)
        => element.SetValue(PopupVerticalOffsetProperty, value);

    public static double GetPopupVerticalOffset(DependencyObject element)
        => (double) element.GetValue(PopupVerticalOffsetProperty);

    public static readonly DependencyProperty PopupHorizontalOffsetProperty = DependencyProperty.RegisterAttached(
        "PopupHorizontalOffset", typeof(double), typeof(MenuAttach), new PropertyMetadata(ValueBoxes.Double0Box));

    public static void SetPopupHorizontalOffset(DependencyObject element, double value)
        => element.SetValue(PopupHorizontalOffsetProperty, value);

    public static double GetPopupHorizontalOffset(DependencyObject element)
        => (double) element.GetValue(PopupHorizontalOffsetProperty);

    public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.RegisterAttached(
        "ItemPadding", typeof(Thickness), typeof(MenuAttach), new PropertyMetadata(default(Thickness)));

    public static void SetItemPadding(DependencyObject element, Thickness value)
        => element.SetValue(ItemPaddingProperty, value);

    public static Thickness GetItemPadding(DependencyObject element)
        => (Thickness) element.GetValue(ItemPaddingProperty);
}

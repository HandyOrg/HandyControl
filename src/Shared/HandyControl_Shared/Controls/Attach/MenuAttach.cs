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

    public static readonly DependencyProperty ItemMinWidthProperty = DependencyProperty.RegisterAttached(
        "ItemMinWidth", typeof(double), typeof(MenuAttach), new PropertyMetadata(ValueBoxes.Double240Box));

    public static void SetItemMinWidth(DependencyObject obj, double value) => obj.SetValue(ItemMinWidthProperty, value);

    public static double GetItemMinWidth(DependencyObject obj) => (double) obj.GetValue(ItemMinWidthProperty);

    public static readonly DependencyProperty TopLevelMinWidthProperty = DependencyProperty.RegisterAttached(
        "TopLevelMinWidth", typeof(double), typeof(MenuAttach), new PropertyMetadata(ValueBoxes.Double44Box));

    public static void SetTopLevelMinWidth(DependencyObject obj, double value) => obj.SetValue(TopLevelMinWidthProperty, value);

    public static double GetTopLevelMinWidth(DependencyObject obj) => (double) obj.GetValue(TopLevelMinWidthProperty);
}

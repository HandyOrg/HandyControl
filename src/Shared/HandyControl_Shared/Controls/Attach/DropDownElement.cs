using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class DropDownElement
{
    public static readonly DependencyProperty ConsistentWidthProperty = DependencyProperty.RegisterAttached(
        "ConsistentWidth", typeof(bool), typeof(DropDownElement),
        new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetConsistentWidth(DependencyObject element, bool value)
        => element.SetValue(ConsistentWidthProperty, ValueBoxes.BooleanBox(value));

    public static bool GetConsistentWidth(DependencyObject element)
        => (bool) element.GetValue(ConsistentWidthProperty);

    public static readonly DependencyProperty AutoWidthProperty = DependencyProperty.RegisterAttached(
        "AutoWidth", typeof(bool), typeof(DropDownElement),
        new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetAutoWidth(DependencyObject element, bool value)
        => element.SetValue(AutoWidthProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAutoWidth(DependencyObject element)
        => (bool) element.GetValue(AutoWidthProperty);
}

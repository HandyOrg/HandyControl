using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls;

public class BackgroundSwitchElement
{
    public static readonly DependencyProperty MouseHoverBackgroundProperty = DependencyProperty.RegisterAttached(
        "MouseHoverBackground", typeof(Brush), typeof(BackgroundSwitchElement), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetMouseHoverBackground(DependencyObject element, Brush value) => element.SetValue(MouseHoverBackgroundProperty, value);

    public static Brush GetMouseHoverBackground(DependencyObject element) => (Brush) element.GetValue(MouseHoverBackgroundProperty);

    public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached(
        "MouseDownBackground", typeof(Brush), typeof(BackgroundSwitchElement), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetMouseDownBackground(DependencyObject element, Brush value) => element.SetValue(MouseDownBackgroundProperty, value);

    public static Brush GetMouseDownBackground(DependencyObject element) => (Brush) element.GetValue(MouseDownBackgroundProperty);
}

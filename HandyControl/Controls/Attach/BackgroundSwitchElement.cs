using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class BackgroundSwitchElement : DependencyObject
    {
        public static readonly DependencyProperty MouseHoverBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseHoverBackground", typeof(Brush), typeof(BackgroundSwitchElement), new PropertyMetadata(Brushes.Transparent));

        public static void SetMouseHoverBackground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseHoverBackgroundProperty, value);
        }

        public static Brush GetMouseHoverBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseHoverBackgroundProperty);
        }

        public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseDownBackground", typeof(Brush), typeof(BackgroundSwitchElement), new PropertyMetadata(Brushes.Transparent));

        public static void SetMouseDownBackground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseDownBackgroundProperty, value);
        }

        public static Brush GetMouseDownBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseDownBackgroundProperty);
        }
    }
}
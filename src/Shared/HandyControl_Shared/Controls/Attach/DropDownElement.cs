using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class DropDownElement
    {
        public static readonly DependencyProperty ConsistentWidthProperty = DependencyProperty.RegisterAttached(
            "ConsistentWidth", typeof(bool), typeof(DropDownElement),
            new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetConsistentWidth(DependencyObject element, bool value)
            => element.SetValue(ConsistentWidthProperty, value);

        public static bool GetConsistentWidth(DependencyObject element)
            => (bool) element.GetValue(ConsistentWidthProperty);
    }
}

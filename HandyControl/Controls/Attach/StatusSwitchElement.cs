using System.Windows;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class StatusSwitchElement
    {
        public static readonly DependencyProperty CheckedElementProperty = DependencyProperty.RegisterAttached(
            "CheckedElement", typeof(object), typeof(StatusSwitchElement), new PropertyMetadata(default(object)));

        public static void SetCheckedElement(DependencyObject element, object value) => element.SetValue(CheckedElementProperty, value);

        public static object GetCheckedElement(DependencyObject element) => element.GetValue(CheckedElementProperty);
    }
}
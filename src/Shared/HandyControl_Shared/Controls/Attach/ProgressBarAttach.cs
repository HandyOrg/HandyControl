using HandyControl.Data;
using System.Windows;

namespace HandyControl.Controls
{
    public class ProgressBarAttach
    {
        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.RegisterAttached(
            "ShowText", typeof(bool), typeof(ProgressBarAttach), new PropertyMetadata(ValueBoxes.TrueBox));

        public static void SetShowText(DependencyObject element, bool value)
            => element.SetValue(ShowTextProperty, ValueBoxes.BooleanBox(value));

        public static bool GetShowText(DependencyObject element)
            => (bool) element.GetValue(ShowTextProperty);
    }
}

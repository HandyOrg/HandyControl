using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class VisualElement
    {
        public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.RegisterAttached(
            "HighlightBrush", typeof(Brush), typeof(VisualElement), new PropertyMetadata(default(Brush)));

        public static void SetHighlightBrush(DependencyObject element, Brush value)
            => element.SetValue(HighlightBrushProperty, value);

        public static Brush GetHighlightBrush(DependencyObject element)
            => (Brush) element.GetValue(HighlightBrushProperty);

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof(string), typeof(VisualElement), new PropertyMetadata(default(string)));

        public static void SetText(DependencyObject element, string value)
            => element.SetValue(TextProperty, value);

        public static string GetText(DependencyObject element)
            => (string) element.GetValue(TextProperty);
    }
}
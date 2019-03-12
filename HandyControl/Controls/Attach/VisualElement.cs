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
    }
}
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Controls;

public class RectangleAttach
{
    public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached(
        "Circular", typeof(bool), typeof(RectangleAttach), new PropertyMetadata(ValueBoxes.FalseBox, OnCircularChanged));

    private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Rectangle rectangle)
        {
            if ((bool) e.NewValue)
            {
                var binding = new MultiBinding
                {
                    Converter = new RectangleCircularConverter()
                };
                binding.Bindings.Add(new Binding(FrameworkElement.ActualWidthProperty.Name) { Source = rectangle });
                binding.Bindings.Add(new Binding(FrameworkElement.ActualHeightProperty.Name) { Source = rectangle });
                rectangle.SetBinding(Rectangle.RadiusXProperty, binding);
                rectangle.SetBinding(Rectangle.RadiusYProperty, binding);
            }
            else
            {
                BindingOperations.ClearBinding(rectangle, FrameworkElement.ActualWidthProperty);
                BindingOperations.ClearBinding(rectangle, FrameworkElement.ActualHeightProperty);
                BindingOperations.ClearBinding(rectangle, Rectangle.RadiusXProperty);
                BindingOperations.ClearBinding(rectangle, Rectangle.RadiusYProperty);
            }
        }
    }

    public static void SetCircular(DependencyObject element, bool value)
        => element.SetValue(CircularProperty, ValueBoxes.BooleanBox(value));

    public static bool GetCircular(DependencyObject element)
        => (bool) element.GetValue(CircularProperty);
}

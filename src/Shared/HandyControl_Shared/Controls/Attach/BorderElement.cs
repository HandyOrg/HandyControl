using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HandyControl.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Controls;

public class BorderElement
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
        "CornerRadius", typeof(CornerRadius), typeof(BorderElement), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

    public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

    public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius) element.GetValue(CornerRadiusProperty);

    public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached(
        "Circular", typeof(bool), typeof(BorderElement), new PropertyMetadata(ValueBoxes.FalseBox, OnCircularChanged));

    private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Border border)
        {
            return;
        }

        if ((bool) e.NewValue)
        {
            var binding = new MultiBinding
            {
                Converter = new BorderCircularConverter()
            };
            binding.Bindings.Add(new Binding(FrameworkElement.ActualWidthProperty.Name) { Source = border });
            binding.Bindings.Add(new Binding(FrameworkElement.ActualHeightProperty.Name) { Source = border });
            border.SetBinding(Border.CornerRadiusProperty, binding);
        }
        else
        {
            BindingOperations.ClearBinding(border, Border.CornerRadiusProperty);
        }
    }

    public static void SetCircular(DependencyObject element, bool value)
        => element.SetValue(CircularProperty, ValueBoxes.BooleanBox(value));

    public static bool GetCircular(DependencyObject element)
        => (bool) element.GetValue(CircularProperty);
}

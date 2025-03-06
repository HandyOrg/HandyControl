using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Controls;

public class BorderElement
{
    public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.RegisterAttached<BorderElement, AvaloniaObject, CornerRadius>("CornerRadius", inherits: true);

    public static void SetCornerRadius(AvaloniaObject element, CornerRadius value) =>
        element.SetValue(CornerRadiusProperty, value);

    public static CornerRadius GetCornerRadius(AvaloniaObject element) => element.GetValue(CornerRadiusProperty);

    public static readonly AttachedProperty<bool> CircularProperty =
        AvaloniaProperty.RegisterAttached<BorderElement, AvaloniaObject, bool>("Circular");

    public static void SetCircular(AvaloniaObject element, bool value) => element.SetValue(CircularProperty, value);

    public static bool GetCircular(AvaloniaObject element) => element.GetValue(CircularProperty);

    static BorderElement()
    {
        CircularProperty.Changed.AddClassHandler<AvaloniaObject>(OnCircularChanged);
    }

    private static void OnCircularChanged(AvaloniaObject element, AvaloniaPropertyChangedEventArgs e)
    {
        if (element is not Border border)
        {
            return;
        }

        if (e.GetNewValue<bool>())
        {
            var binding = new Binding(Visual.BoundsProperty.Name)
            {
                Converter = new BorderCircularConverter(),
                Source = border,
            };
            border.Bind(Border.CornerRadiusProperty, binding);
        }
        else
        {
            border.ClearValue(Border.CornerRadiusProperty);
        }
    }
}

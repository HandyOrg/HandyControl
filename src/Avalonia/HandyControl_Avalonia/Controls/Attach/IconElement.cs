using Avalonia;
using Avalonia.Media;

namespace HandyControl.Controls;

public class IconElement
{
    public static readonly AttachedProperty<Geometry> GeometryProperty =
        AvaloniaProperty.RegisterAttached<IconElement, AvaloniaObject, Geometry>("Geometry");

    public static void SetGeometry(AvaloniaObject element, Geometry value) => element.SetValue(GeometryProperty, value);

    public static Geometry GetGeometry(AvaloniaObject element) => element.GetValue(GeometryProperty);

    public static readonly AttachedProperty<double> WidthProperty =
        AvaloniaProperty.RegisterAttached<IconElement, AvaloniaObject, double>("Width", defaultValue: double.NaN);

    public static void SetWidth(AvaloniaObject element, double value) => element.SetValue(WidthProperty, value);

    public static double GetWidth(AvaloniaObject element) => element.GetValue(WidthProperty);

    public static readonly AttachedProperty<double> HeightProperty =
        AvaloniaProperty.RegisterAttached<IconElement, AvaloniaObject, double>("Height", defaultValue: double.NaN);

    public static void SetHeight(AvaloniaObject element, double value) => element.SetValue(HeightProperty, value);
    public static double GetHeight(AvaloniaObject element) => element.GetValue(HeightProperty);
}

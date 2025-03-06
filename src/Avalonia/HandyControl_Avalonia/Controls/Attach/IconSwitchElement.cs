using Avalonia;
using Avalonia.Media;

namespace HandyControl.Controls;

public class IconSwitchElement
{
    public static readonly AttachedProperty<Geometry> GeometryProperty =
        AvaloniaProperty.RegisterAttached<IconSwitchElement, AvaloniaObject, Geometry>("Geometry");

    public static void SetGeometry(AvaloniaObject element, Geometry value) => element.SetValue(GeometryProperty, value);

    public static Geometry GetGeometry(AvaloniaObject element) => element.GetValue(GeometryProperty);

    public static readonly AttachedProperty<double> WidthProperty =
        AvaloniaProperty.RegisterAttached<IconSwitchElement, AvaloniaObject, double>("Width", defaultValue: double.NaN);

    public static void SetWidth(AvaloniaObject element, double value) => element.SetValue(WidthProperty, value);

    public static double GetWidth(AvaloniaObject element) => element.GetValue(WidthProperty);

    public static readonly AttachedProperty<double> HeightProperty =
        AvaloniaProperty.RegisterAttached<IconSwitchElement, AvaloniaObject, double>("Height", defaultValue: double.NaN);

    public static void SetHeight(AvaloniaObject element, double value) => element.SetValue(HeightProperty, value);
    public static double GetHeight(AvaloniaObject element) => element.GetValue(HeightProperty);

    public static readonly AttachedProperty<Geometry> GeometrySelectedProperty =
        AvaloniaProperty.RegisterAttached<IconSwitchElement, AvaloniaObject, Geometry>("GeometrySelected");

    public static void SetGeometrySelected(AvaloniaObject element, Geometry value) =>
        element.SetValue(GeometrySelectedProperty, value);

    public static Geometry GetGeometrySelected(AvaloniaObject element) => element.GetValue(GeometrySelectedProperty);
}

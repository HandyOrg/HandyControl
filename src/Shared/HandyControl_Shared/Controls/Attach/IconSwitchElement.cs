using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls;

public class IconSwitchElement : IconElement
{
    public static readonly DependencyProperty GeometrySelectedProperty = DependencyProperty.RegisterAttached(
        "GeometrySelected", typeof(Geometry), typeof(IconSwitchElement), new PropertyMetadata(default(Geometry)));

    public static void SetGeometrySelected(DependencyObject element, Geometry value)
    {
        element.SetValue(GeometrySelectedProperty, value);
    }

    public static Geometry GetGeometrySelected(DependencyObject element)
    {
        return (Geometry) element.GetValue(GeometrySelectedProperty);
    }
}

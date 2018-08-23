using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class IconSwitchElement : DependencyObject
    {
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
            "Geometry", typeof(Geometry), typeof(IconSwitchElement), new PropertyMetadata(default(Geometry)));

        public static void SetGeometry(DependencyObject element, Geometry value)
        {
            element.SetValue(GeometryProperty, value);
        }

        public static Geometry GetGeometry(DependencyObject element)
        {
            return (Geometry)element.GetValue(GeometryProperty);
        }

        public static readonly DependencyProperty GeometrySelectedProperty = DependencyProperty.RegisterAttached(
            "GeometrySelected", typeof(Geometry), typeof(IconSwitchElement), new PropertyMetadata(default(Geometry)));

        public static void SetGeometrySelected(DependencyObject element, Geometry value)
        {
            element.SetValue(GeometrySelectedProperty, value);
        }

        public static Geometry GetGeometrySelected(DependencyObject element)
        {
            return (Geometry)element.GetValue(GeometrySelectedProperty);
        }
    }
}
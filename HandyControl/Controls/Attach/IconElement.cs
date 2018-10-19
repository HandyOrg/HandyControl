using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class IconElement : DependencyObject
    {
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
            "Geometry", typeof(Geometry), typeof(IconElement), new PropertyMetadata(default(Geometry)));

        public static void SetGeometry(DependencyObject element, Geometry value)
        {
            element.SetValue(GeometryProperty, value);
        }

        public static Geometry GetGeometry(DependencyObject element)
        {
            return (Geometry)element.GetValue(GeometryProperty);
        }
    }
}
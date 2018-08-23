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

        public static readonly DependencyProperty AngleProperty = DependencyProperty.RegisterAttached(
            "Angle", typeof(double), typeof(IconElement), new PropertyMetadata(default(double)));

        public static void SetAngle(DependencyObject element, double value)
        {
            element.SetValue(AngleProperty, value);
        }

        public static double GetAngle(DependencyObject element)
        {
            return (double)element.GetValue(AngleProperty);
        }
    }
}
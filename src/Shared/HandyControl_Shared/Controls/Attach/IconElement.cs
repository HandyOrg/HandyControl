using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls;

public class IconElement
{
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
        "Geometry", typeof(Geometry), typeof(IconElement), new PropertyMetadata(default(Geometry)));

    public static void SetGeometry(DependencyObject element, Geometry value)
        => element.SetValue(GeometryProperty, value);

    public static Geometry GetGeometry(DependencyObject element)
        => (Geometry) element.GetValue(GeometryProperty);

    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
        "Width", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

    public static void SetWidth(DependencyObject element, double value)
        => element.SetValue(WidthProperty, value);

    public static double GetWidth(DependencyObject element)
        => (double) element.GetValue(WidthProperty);

    public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
        "Height", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

    public static void SetHeight(DependencyObject element, double value)
        => element.SetValue(HeightProperty, value);

    public static double GetHeight(DependencyObject element)
        => (double) element.GetValue(HeightProperty);
}

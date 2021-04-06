using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
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

        // Proposal


        public static object GetIconContent(DependencyObject obj)
        {
            return (object) obj.GetValue(IconContentProperty);
        }

        public static void SetIconContent(DependencyObject obj, object value)
        {
            obj.SetValue(IconContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.RegisterAttached("IconContent", typeof(object), typeof(IconElement), new PropertyMetadata(null));




        public static DataTemplate GetIconTemplate(DependencyObject obj)
        {
            return (DataTemplate) obj.GetValue(IconTemplateProperty);
        }

        public static void SetIconTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(IconTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.RegisterAttached("IconTemplate", typeof(DataTemplate), typeof(IconElement), new PropertyMetadata(null));




        public static bool GetIsIconVisible(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsIconVisibleProperty);
        }

        public static void SetIsIconVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsIconVisibleProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsIconVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsIconVisibleProperty =
            DependencyProperty.RegisterAttached("IsIconVisible", typeof(bool), typeof(IconElement), new PropertyMetadata(false));





        public static Dock GetIconPosition(DependencyObject obj)
        {
            return (Dock) obj.GetValue(IconPositionProperty);
        }

        public static void SetIconPosition(DependencyObject obj, Dock value)
        {
            obj.SetValue(IconPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.RegisterAttached("IconPosition", typeof(Dock), typeof(IconElement), new PropertyMetadata(Dock.Left));

    }
}

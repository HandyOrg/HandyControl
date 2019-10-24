using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Divider : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Divider), new PropertyMetadata(default(object)));

        public object Content
        {
            get => (object) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty DashedProperty = DependencyProperty.Register(
            "Dashed", typeof(bool), typeof(Divider), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool Dashed
        {
            get => (bool) GetValue(DashedProperty);
            set => SetValue(DashedProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(Divider), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
    }
}
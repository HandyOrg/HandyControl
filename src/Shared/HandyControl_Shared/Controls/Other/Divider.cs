using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Divider : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Divider), new PropertyMetadata(default(object)));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(Divider), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate", typeof(DataTemplate), typeof(Divider), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate) GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(
            "ContentStringFormat", typeof(string), typeof(Divider), new PropertyMetadata(default(string)));

        public string ContentStringFormat
        {
            get => (string) GetValue(ContentStringFormatProperty);
            set => SetValue(ContentStringFormatProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector", typeof(DataTemplateSelector), typeof(Divider), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(
            "LineStroke", typeof(Brush), typeof(Divider), new PropertyMetadata(default(Brush)));

        public Brush LineStroke
        {
            get => (Brush) GetValue(LineStrokeProperty);
            set => SetValue(LineStrokeProperty, value);
        }

        public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register(
            "LineStrokeThickness", typeof(double), typeof(Divider), new PropertyMetadata(ValueBoxes.Double1Box));

        public double LineStrokeThickness
        {
            get => (double) GetValue(LineStrokeThicknessProperty);
            set => SetValue(LineStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register(
            "LineStrokeDashArray", typeof(DoubleCollection), typeof(Divider), new PropertyMetadata(new DoubleCollection()));

        public DoubleCollection LineStrokeDashArray
        {
            get => (DoubleCollection) GetValue(LineStrokeDashArrayProperty);
            set => SetValue(LineStrokeDashArrayProperty, value);
        }
    }
}

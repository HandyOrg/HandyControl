using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace HandyControl.Controls
{
    [ContentProperty(nameof(Status))]
    public class Shield : ButtonBase
    {
        public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register(
            "Subject", typeof(string), typeof(Shield), new PropertyMetadata(default(string)));

        public string Subject
        {
            get => (string) GetValue(SubjectProperty);
            set => SetValue(SubjectProperty, value);
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(object), typeof(Shield), new PropertyMetadata(default(object)));

        public object Status
        {
            get => GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Brush), typeof(Shield), new PropertyMetadata(default(Brush)));

        public Brush Color
        {
            get => (Brush) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
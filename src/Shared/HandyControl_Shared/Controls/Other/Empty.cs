using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class Empty : ContentControl
    {
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(object), typeof(Empty), new PropertyMetadata(default(object)));

        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(
            "Logo", typeof(object), typeof(Empty), new PropertyMetadata(default(object)));

        public object Logo
        {
            get => GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }
    }
}

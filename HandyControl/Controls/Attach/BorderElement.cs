using HandyControl.Data;
using System.Windows;

namespace HandyControl.Controls
{
    public class BorderElement
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(BorderElement), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);

        public static readonly DependencyProperty ShowSideBorderProperty = DependencyProperty.RegisterAttached(
           "ShowSideBorder", typeof(bool), typeof(BorderElement), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetShowSideBorder(DependencyObject element, bool value) => element.SetValue(ShowSideBorderProperty, value);

        public static bool GetShowSideBorder(DependencyObject element) => (bool)element.GetValue(ShowSideBorderProperty);
    }
}
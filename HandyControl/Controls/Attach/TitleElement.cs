using System.Windows;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class TitleElement : DependencyObject
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background", typeof(Brush), typeof(TitleElement), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetBackground(DependencyObject element, Brush value)
            => element.SetValue(BackgroundProperty, value);

        public static Brush GetBackground(DependencyObject element)
            => (Brush) element.GetValue(BackgroundProperty);

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
            "Foreground", typeof(Brush), typeof(TitleElement), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetForeground(DependencyObject element, Brush value)
            => element.SetValue(ForegroundProperty, value);

        public static Brush GetForeground(DependencyObject element)
            => (Brush) element.GetValue(ForegroundProperty);

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached(
            "BorderBrush", typeof(Brush), typeof(TitleElement), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetBorderBrush(DependencyObject element, Brush value)
            => element.SetValue(BorderBrushProperty, value);

        public static Brush GetBorderBrush(DependencyObject element)
            => (Brush) element.GetValue(BorderBrushProperty);
    }
}
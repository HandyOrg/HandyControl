using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class ImageAttach
    {
        public static readonly DependencyProperty SourceFailedProperty = DependencyProperty.RegisterAttached(
            "SourceFailed", typeof(ImageSource), typeof(ImageAttach), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.Inherits, OnSourceFailedChanged));

        private static void OnSourceFailedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Image image)
            {
                if (e.NewValue is ImageSource)
                {
                    image.ImageFailed += Image_ImageFailed;
                }
                else
                {
                    image.ImageFailed -= Image_ImageFailed;
                }
            }
        }

        private static void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (sender is Image image)
            {
                image.SetCurrentValue(Image.SourceProperty, GetSourceFailed(image));
                image.ImageFailed -= Image_ImageFailed;
            }
        }

        public static void SetSourceFailed(DependencyObject element, ImageSource value)
            => element.SetValue(SourceFailedProperty, value);

        public static ImageSource GetSourceFailed(DependencyObject element)
            => (ImageSource) element.GetValue(SourceFailedProperty);
    }
}

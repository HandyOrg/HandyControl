using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class ImageRadioButton : RadioButton
    {
        public ImageRadioButton()
        {
            SetSideBrush(this, ResourceHelper.GetResource<Brush>(ResourceToken.PrimaryBrush));
        }

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageRadioButton), new PropertyMetadata(15.0));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageRadioButton), new PropertyMetadata(15.0));

        public static readonly DependencyProperty SideBrushProperty = DependencyProperty.RegisterAttached(
            "SideBrush", typeof(Brush), typeof(ImageRadioButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetSideBrush(DependencyObject element, Brush value) => element.SetValue(SideBrushProperty, value);

        public static Brush GetSideBrush(DependencyObject element) => (Brush)element.GetValue(SideBrushProperty);

    }
}

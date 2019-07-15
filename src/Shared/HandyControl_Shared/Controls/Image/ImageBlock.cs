using System.Windows;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class ImageBlock : FrameworkElement
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(ImageBlock), new FrameworkPropertyMetadata(default(ImageSource),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public ImageSource Source
        {
            get => (ImageSource) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public int Columns
        {
            get => (int) GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
            "Rows", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public int Rows
        {
            get => (int) GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            var imageSource = Source;

            if (imageSource != null) dc.DrawImage(imageSource, new Rect(new Point(), RenderSize));
        }
    }
}
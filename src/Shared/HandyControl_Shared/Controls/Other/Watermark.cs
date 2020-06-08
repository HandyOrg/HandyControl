using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using HandyControl.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementRoot, Type = typeof(Border))]
    [ContentProperty(nameof(Content))]
    public class Watermark : Control
    {
        private const string ElementRoot = "PART_Root";

        private Border _borderRoot;

        private DrawingBrush _brush;

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Angle
        {
            get => (double) GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Watermark), new PropertyMetadata(default));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty MarkProperty = DependencyProperty.Register(
            "Mark", typeof(object), typeof(Watermark), new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.AffectsRender));

        public object Mark
        {
            get => GetValue(MarkProperty);
            set => SetValue(MarkProperty, value);
        }

        public static readonly DependencyProperty MarkWidthProperty = DependencyProperty.Register(
            "MarkWidth", typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));

        public double MarkWidth
        {
            get => (double) GetValue(MarkWidthProperty);
            set => SetValue(MarkWidthProperty, value);
        }

        public static readonly DependencyProperty MarkHeightProperty = DependencyProperty.Register(
            "MarkHeight", typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));

        public double MarkHeight
        {
            get => (double) GetValue(MarkHeightProperty);
            set => SetValue(MarkHeightProperty, value);
        }

        public static readonly DependencyProperty MarkBrushProperty = DependencyProperty.Register(
            "MarkBrush", typeof(Brush), typeof(Watermark), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush MarkBrush
        {
            get => (Brush) GetValue(MarkBrushProperty);
            set => SetValue(MarkBrushProperty, value);
        }

        public static readonly DependencyProperty AutoSizeEnabledProperty = DependencyProperty.Register(
            "AutoSizeEnabled", typeof(bool), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool AutoSizeEnabled
        {
            get => (bool) GetValue(AutoSizeEnabledProperty);
            set => SetValue(AutoSizeEnabledProperty, value);
        }

        public static readonly DependencyProperty MarkMarginProperty = DependencyProperty.Register(
            "MarkMargin", typeof(Thickness), typeof(Watermark), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsRender));

        public Thickness MarkMargin
        {
            get => (Thickness) GetValue(MarkMarginProperty);
            set => SetValue(MarkMarginProperty, value);
        }

        public override void OnApplyTemplate() => _borderRoot = GetTemplateChild(ElementRoot) as Border;

        private void EnsureBrush()
        {
            var presenter = new ContentPresenter();

            if (Mark is Geometry geometry)
            {
                presenter.Content = new Path
                {
                    Width = MarkWidth,
                    Height = MarkHeight,
                    Fill = MarkBrush,
                    Stretch = Stretch.Uniform,
                    Data = geometry
                };
            }
            else if (Mark is string str)
            {
                presenter.Content = new TextBlock
                {
                    Text = str,
                    FontSize = FontSize
                };
            }
            else
            {
                presenter.Content = Mark;
            }

            Size markSize;
            if (AutoSizeEnabled)
            {
                presenter.Measure(new Size(double.MaxValue, double.MaxValue));
                markSize = presenter.DesiredSize;
            }
            else
            {
                markSize = new Size(MarkWidth, MarkHeight);
            }

            _brush = new DrawingBrush
            {
                ViewportUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.Uniform,
                TileMode = TileMode.Tile,
                Transform = new RotateTransform(Angle),
                Drawing = new GeometryDrawing
                {
                    Brush = new VisualBrush(new Border
                    {
                        Background = Brushes.Transparent,
                        Padding = MarkMargin,
                        Child = presenter
                    }), 
                    Geometry = new RectangleGeometry(new Rect(markSize))
                },
                Viewport = new Rect(markSize)
            };

            RenderOptions.SetCacheInvalidationThresholdMinimum(_brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(_brush, 2);
            RenderOptions.SetCachingHint(_brush, CachingHint.Cache);

            if (_borderRoot != null)
            {
                _borderRoot.Background = _brush;
            }
        }

        protected override void OnRender(DrawingContext drawingContext) => EnsureBrush();
    }
}

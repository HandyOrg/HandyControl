using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class OutlineText : FrameworkElement
    {
        private Pen _pen;

        private FormattedText _formattedText;

        private Geometry _textGeometry;

        private PathGeometry _clipGeometry;

        static OutlineText()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(OutlineText), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(OutlineText), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
        }

        public static readonly DependencyProperty StrokePositionProperty = DependencyProperty.Register(
            "StrokePosition", typeof(StrokePosition), typeof(OutlineText), new PropertyMetadata(default(StrokePosition)));

        public StrokePosition StrokePosition
        {
            get => (StrokePosition) GetValue(StrokePositionProperty);
            set => SetValue(StrokePositionProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(OutlineText), new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender, OnFormattedTextInvalidated));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(OutlineText),
            new PropertyMetadata(default(TextAlignment), OnFormattedTextUpdated));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment) GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(OutlineText),
            new PropertyMetadata(default(TextTrimming), OnFormattedTextInvalidated));

        public TextTrimming TextTrimming
        {
            get => (TextTrimming) GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(OutlineText),
            new PropertyMetadata(TextWrapping.NoWrap, OnFormattedTextInvalidated));

        public TextWrapping TextWrapping
        {
            get => (TextWrapping) GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(OutlineText), new PropertyMetadata(Brushes.Black, OnFormattedTextUpdated));

        public Brush Fill
        {
            get => (Brush) GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(OutlineText), new PropertyMetadata(Brushes.Black, OnFormattedTextUpdated));

        public Brush Stroke
        {
            get => (Brush) GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(OutlineText), new PropertyMetadata(ValueBoxes.Double0Box, OnFormattedTextUpdated));

        public double StrokeThickness
        {
            get => (double) GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
            typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontFamily FontFamily
        {
            get => (FontFamily)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
            typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
            typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontStretch FontStretch
        {
            get => (FontStretch)GetValue(FontStretchProperty);
            set => SetValue(FontStretchProperty, value);
        }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
            typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontStyle FontStyle
        {
            get => (FontStyle)GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
            typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (StrokeThickness > 0)
            {
                EnsureGeometry();

                drawingContext.DrawGeometry(Fill, null, _textGeometry);

                if (StrokePosition == StrokePosition.Outside)
                {
                    drawingContext.PushClip(_clipGeometry);
                }
                else if (StrokePosition == StrokePosition.Inside)
                {
                    drawingContext.PushClip(_textGeometry);
                }

                drawingContext.DrawGeometry(null, _pen, _textGeometry);

                if (StrokePosition == StrokePosition.Outside || StrokePosition == StrokePosition.Inside)
                {
                    drawingContext.Pop();
                }
            }
            else
            {
                UpdateFormattedText();
                drawingContext.DrawText(_formattedText, new Point());
            }
        }

        private void UpdatePen()
        {
            _pen = new Pen(Stroke, StrokeThickness);

            if (StrokePosition == StrokePosition.Outside || StrokePosition == StrokePosition.Inside)
            {
                _pen.Thickness = StrokeThickness * 2;
            }
        }

        private void EnsureFormattedText()
        {
            if (_formattedText != null || Text == null)
            {
                return;
            }

#if NET40 || NET45
            _formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentUICulture,
                FlowDirection,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize, Fill);
#else
            var source = PresentationSource.FromVisual(this);
            var dpi = 1.0;
            if (source?.CompositionTarget != null)
            {
                dpi = 96.0 * source.CompositionTarget.TransformToDevice.M11 / 96.0;
            }
            _formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentUICulture,
                FlowDirection,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), 
                FontSize, Fill, dpi);
#endif

            UpdateFormattedText();
        }

        private void EnsureGeometry()
        {
            if (_textGeometry != null)
            {
                return;
            }

            EnsureFormattedText();
            _textGeometry = _formattedText.BuildGeometry(new Point(0, 0));

            if (StrokePosition == StrokePosition.Outside)
            {
                var geometry = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight));
                _clipGeometry = Geometry.Combine(geometry, _textGeometry, GeometryCombineMode.Exclude, null);
            }
        }

        private void UpdateFormattedText()
        {
            if (_formattedText == null)
            {
                return;
            }

            _formattedText.MaxLineCount = TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
            _formattedText.TextAlignment = TextAlignment;
            _formattedText.Trimming = TextTrimming;

            _formattedText.SetFontSize(FontSize);
            _formattedText.SetFontStyle(FontStyle);
            _formattedText.SetFontWeight(FontWeight);
            _formattedText.SetFontFamily(FontFamily);
            _formattedText.SetFontStretch(FontStretch);
            _formattedText.SetForegroundBrush(Fill);
        }

        private static void OnFormattedTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlineText)d;
            outlinedTextBlock.UpdateFormattedText();
            outlinedTextBlock._textGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private static void OnFormattedTextInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlineText)d;
            outlinedTextBlock._formattedText = null;
            outlinedTextBlock._textGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            EnsureFormattedText();

            // constrain the formatted text according to the available size
            // the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width), things blow up when availableSize is infinite in both directions
            // the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
            _formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
            _formattedText.MaxTextHeight = Math.Max(0.0001d, availableSize.Height);

            UpdatePen();

            // return the desired size
            return new Size(_formattedText.Width, _formattedText.Height);
        }
    }
}
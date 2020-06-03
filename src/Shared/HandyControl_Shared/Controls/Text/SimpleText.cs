using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class SimpleText : FrameworkElement
    {
        private FormattedText _formattedText;

        static SimpleText()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(SimpleText), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(SimpleText), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SimpleText), new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender, OnFormattedTextInvalidated));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(SimpleText),
            new PropertyMetadata(default(TextAlignment), OnFormattedTextUpdated));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(SimpleText),
            new PropertyMetadata(default(TextTrimming), OnFormattedTextInvalidated));

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(SimpleText),
            new PropertyMetadata(TextWrapping.NoWrap, OnFormattedTextInvalidated));

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof(Brush), typeof(SimpleText), new PropertyMetadata(Brushes.Black, OnFormattedTextUpdated));

        public Brush Foreground
        {
            get => (Brush) GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
            typeof(SimpleText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontFamily FontFamily
        {
            get => (FontFamily)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
            typeof(SimpleText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
            typeof(SimpleText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontStretch FontStretch
        {
            get => (FontStretch)GetValue(FontStretchProperty);
            set => SetValue(FontStretchProperty, value);
        }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
            typeof(SimpleText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontStyle FontStyle
        {
            get => (FontStyle)GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
            typeof(SimpleText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext) => drawingContext.DrawText(_formattedText, new Point());

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
                FontSize, Foreground);
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
                FontSize, Foreground, dpi);
#endif

            UpdateFormattedText();
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
            _formattedText.SetForegroundBrush(Foreground);
        }

        private static void OnFormattedTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (SimpleText)d;
            outlinedTextBlock.UpdateFormattedText();

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private static void OnFormattedTextInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (SimpleText)d;
            outlinedTextBlock._formattedText = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            EnsureFormattedText();

            _formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
            _formattedText.MaxTextHeight = Math.Max(0.0001d, availableSize.Height);

            return new Size(_formattedText.Width, _formattedText.Height);
        }
    }
}
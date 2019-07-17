using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class ImageBlock : FrameworkElement
    {
        private readonly DispatcherTimer _dispatcherTimer;

        private int _indexMax;

        private int _indexMin;

        private int _currentIndex;

        public ImageBlock()
        {
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = Interval
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            OnPositionsChanged();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) => InvalidateVisual();

        public static readonly DependencyProperty StartColumnProperty = DependencyProperty.Register(
            "StartColumn", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

        private void OnPositionsChanged()
        {
            _indexMin = StartRow * Columns + StartColumn;
            _indexMax = EndRow * Columns + EndColumn;
        }

        private static void OnPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ImageBlock)d;
            ctl.OnPositionsChanged();
        }

        public int StartColumn
        {
            get => (int) GetValue(StartColumnProperty);
            set => SetValue(StartColumnProperty, value);
        }

        public static readonly DependencyProperty StartRowProperty = DependencyProperty.Register(
            "StartRow", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

        public int StartRow
        {
            get => (int) GetValue(StartRowProperty);
            set => SetValue(StartRowProperty, value);
        }

        public static readonly DependencyProperty EndColumnProperty = DependencyProperty.Register(
            "EndColumn", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

        public int EndColumn
        {
            get => (int) GetValue(EndColumnProperty);
            set => SetValue(EndColumnProperty, value);
        }

        public static readonly DependencyProperty EndRowProperty = DependencyProperty.Register(
            "EndRow", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

        public int EndRow
        {
            get => (int) GetValue(EndRowProperty);
            set => SetValue(EndRowProperty, value);
        }

        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            "IsPlaying", typeof(bool), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.TrueBox,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnIsPlayingChanged));

        private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ImageBlock)d;
            ctl._dispatcherTimer.IsEnabled = (bool) e.NewValue;
        }

        public bool IsPlaying
        {
            get => (bool) GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), obj => (int)obj >= 1);

        public int Columns
        {
            get => (int) GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
            "Rows", typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), obj => (int)obj >= 1);

        public int Rows
        {
            get => (int) GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(TimeSpan), typeof(ImageBlock), new PropertyMetadata(TimeSpan.FromSeconds(1), OnIntervalChanged));

        private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ImageBlock) d;
            ctl._dispatcherTimer.Interval = (TimeSpan) e.NewValue;
        }

        public TimeSpan Interval
        {
            get => (TimeSpan) GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(ImageBlock), new FrameworkPropertyMetadata(default(ImageSource),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (Source != null && Source is BitmapSource source)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var croppedBitmap = new CroppedBitmap(source, CalDisplayRect());
                dc.DrawImage(croppedBitmap, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            }
        }

        private Int32Rect CalDisplayRect()
        {
            if (_currentIndex > _indexMax)
            {
                _currentIndex = _indexMin;
            }

            var x = (int) (_currentIndex % Columns * RenderSize.Width);
            // ReSharper disable once PossibleLossOfFraction
            var y = (int) (_currentIndex / Columns * RenderSize.Height);

            var rect = new Int32Rect(x, y, (int)RenderSize.Width, (int)RenderSize.Height);
            _currentIndex++;
            return rect;
        }
    }
}
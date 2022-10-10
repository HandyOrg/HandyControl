using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ImageBlock : FrameworkElement
{
    private readonly DispatcherTimer _dispatcherTimer;

    private BitmapSource _source;

    private int _indexMax;

    private int _indexMin;

    private int _currentIndex;

    private int _blockWidth;

    private int _blockHeight;

    private bool _isDisposed;

    private int _columns = 1;

    public ImageBlock()
    {
        _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = Interval
        };

        IsVisibleChanged += ImageBlock_IsVisibleChanged;
    }

    ~ImageBlock() => Dispose();

    public void Dispose()
    {
        if (_isDisposed) return;

        IsVisibleChanged -= ImageBlock_IsVisibleChanged;
        _dispatcherTimer.Stop();
        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    private void ImageBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsVisible)
        {
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            if (IsPlaying)
            {
                _dispatcherTimer.Start();
            }
        }
        else
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
        }
    }

    private void UpdateDatas()
    {
        if (_source == null) return;

        _indexMin = StartRow * _columns + StartColumn;
        _indexMax = EndRow * _columns + EndColumn;
        _currentIndex = _indexMin;
        _blockWidth = _source.PixelWidth / _columns;
        _blockHeight = _source.PixelHeight / Rows;
    }

    private static void OnPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ImageBlock) d;

        if (e.Property == ColumnsProperty)
        {
            ctl._columns = (int) e.NewValue;
        }

        ctl.UpdateDatas();
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e) => InvalidateVisual();

    public static readonly DependencyProperty StartColumnProperty = DependencyProperty.Register(
        nameof(StartColumn), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

    public int StartColumn
    {
        get => (int) GetValue(StartColumnProperty);
        set => SetValue(StartColumnProperty, value);
    }

    public static readonly DependencyProperty StartRowProperty = DependencyProperty.Register(
        nameof(StartRow), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

    public int StartRow
    {
        get => (int) GetValue(StartRowProperty);
        set => SetValue(StartRowProperty, value);
    }

    public static readonly DependencyProperty EndColumnProperty = DependencyProperty.Register(
        nameof(EndColumn), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

    public int EndColumn
    {
        get => (int) GetValue(EndColumnProperty);
        set => SetValue(EndColumnProperty, value);
    }

    public static readonly DependencyProperty EndRowProperty = DependencyProperty.Register(
        nameof(EndRow), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged));

    public int EndRow
    {
        get => (int) GetValue(EndRowProperty);
        set => SetValue(EndRowProperty, value);
    }

    public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
        nameof(IsPlaying), typeof(bool), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.FalseBox,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnIsPlayingChanged));

    private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ImageBlock) d;
        if ((bool) e.NewValue)
        {
            ctl._dispatcherTimer.Start();
        }
        else
        {
            ctl._dispatcherTimer.Stop();
        }
    }

    public bool IsPlaying
    {
        get => (bool) GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
        nameof(Columns), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged), obj => (int) obj >= 1);

    public int Columns
    {
        get => (int) GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
        nameof(Rows), typeof(int), typeof(ImageBlock), new FrameworkPropertyMetadata(ValueBoxes.Int1Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnPositionsChanged), obj => (int) obj >= 1);

    public int Rows
    {
        get => (int) GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
        nameof(Interval), typeof(TimeSpan), typeof(ImageBlock), new PropertyMetadata(TimeSpan.FromSeconds(1), OnIntervalChanged));

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
        nameof(Source), typeof(ImageSource), typeof(ImageBlock), new FrameworkPropertyMetadata(default(ImageSource),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnSourceChanged));

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ImageBlock) d;
        ctl._source = e.NewValue as BitmapSource;
        ctl.UpdateDatas();
    }

    public ImageSource Source
    {
        get => (ImageSource) GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    protected override void OnRender(DrawingContext dc)
    {
        if (_source == null) return;
        var croppedBitmap = new CroppedBitmap(_source, CalDisplayRect());
        dc.DrawImage(croppedBitmap, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
    }

    private Int32Rect CalDisplayRect()
    {
        if (_currentIndex > _indexMax)
        {
            _currentIndex = _indexMin;
        }

        var x = _currentIndex % _columns * _blockWidth;
        var y = _currentIndex / _columns * _blockHeight;

        var rect = new Int32Rect(x, y, _blockWidth, _blockHeight);
        _currentIndex++;
        return rect;
    }
}

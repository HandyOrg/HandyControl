using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace HandyControl.Controls;

/// <summary>
///     A sprite-sheet frame animation player.
///     Slices the source image into a <see cref="Columns"/> x <see cref="Rows"/> grid and
///     plays back the frame range [StartRow,StartColumn] … [EndRow,EndColumn].
/// </summary>
public class ImageBlock : Control
{
    private readonly DispatcherTimer _dispatcherTimer;
    private IImage? _source;
    private int _indexMax;
    private int _indexMin;
    private int _currentIndex;
    private int _blockWidth;
    private int _blockHeight;
    private bool _isDisposed;
    private int _columns = 1;

    public ImageBlock()
    {
        _dispatcherTimer = new DispatcherTimer
        {
            Interval = Interval
        };
        _dispatcherTimer.Tick += DispatcherTimer_Tick;
    }

    ~ImageBlock() => Dispose();

    public void Dispose()
    {
        if (_isDisposed) return;

        _dispatcherTimer.Tick -= DispatcherTimer_Tick;
        _dispatcherTimer.Stop();
        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    private void UpdateDatas()
    {
        if (_source is null) return;

        _indexMin = StartRow * _columns + StartColumn;
        _indexMax = EndRow * _columns + EndColumn;
        _currentIndex = _indexMin;

        if (_source is Bitmap bitmap)
        {
            _blockWidth = bitmap.PixelSize.Width / _columns;
            _blockHeight = bitmap.PixelSize.Height / Rows;
        }
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs e) => InvalidateVisual();

    // ── Dependency Properties ──

    public static readonly StyledProperty<int> StartColumnProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(StartColumn));

    public int StartColumn
    {
        get => GetValue(StartColumnProperty);
        set => SetValue(StartColumnProperty, value);
    }

    public static readonly StyledProperty<int> StartRowProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(StartRow));

    public int StartRow
    {
        get => GetValue(StartRowProperty);
        set => SetValue(StartRowProperty, value);
    }

    public static readonly StyledProperty<int> EndColumnProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(EndColumn));

    public int EndColumn
    {
        get => GetValue(EndColumnProperty);
        set => SetValue(EndColumnProperty, value);
    }

    public static readonly StyledProperty<int> EndRowProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(EndRow));

    public int EndRow
    {
        get => GetValue(EndRowProperty);
        set => SetValue(EndRowProperty, value);
    }

    public static readonly StyledProperty<bool> IsPlayingProperty =
        AvaloniaProperty.Register<ImageBlock, bool>(nameof(IsPlaying));

    public bool IsPlaying
    {
        get => GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(Columns), defaultValue: 1,
            validate: v => v >= 1);

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public static readonly StyledProperty<int> RowsProperty =
        AvaloniaProperty.Register<ImageBlock, int>(nameof(Rows), defaultValue: 1,
            validate: v => v >= 1);

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public static readonly StyledProperty<TimeSpan> IntervalProperty =
        AvaloniaProperty.Register<ImageBlock, TimeSpan>(nameof(Interval),
            defaultValue: TimeSpan.FromSeconds(1));

    public TimeSpan Interval
    {
        get => GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }

    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageBlock, IImage?>(nameof(Source));

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    // ── Static constructor ──

    static ImageBlock()
    {
        AffectsRender<ImageBlock>(
            SourceProperty, StartColumnProperty, StartRowProperty,
            EndColumnProperty, EndRowProperty, ColumnsProperty, RowsProperty,
            IsPlayingProperty, IntervalProperty);

        SourceProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnSourceChanged(e));
        ColumnsProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnPositionsChanged(e));
        StartColumnProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnPositionsChanged(e));
        StartRowProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnPositionsChanged(e));
        EndColumnProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnPositionsChanged(e));
        EndRowProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnPositionsChanged(e));
        IsPlayingProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnIsPlayingChanged(e));
        IntervalProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnIntervalChanged(e));
        IsVisibleProperty.Changed.AddClassHandler<ImageBlock>((o, e) => o.OnIsVisibleChanged(e));
    }

    // ── Event handlers ──

    private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
    {
        _source = e.GetNewValue<IImage?>();
        UpdateDatas();
    }

    private void OnPositionsChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == ColumnsProperty)
        {
            _columns = e.GetNewValue<int>();
        }

        UpdateDatas();
    }

    private void OnIsPlayingChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.GetNewValue<bool>() && IsVisible)
        {
            _dispatcherTimer.Start();
        }
        else
        {
            _dispatcherTimer.Stop();
        }
    }

    private void OnIntervalChanged(AvaloniaPropertyChangedEventArgs e)
    {
        _dispatcherTimer.Interval = e.GetNewValue<TimeSpan>();
    }

    private void OnIsVisibleChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsVisible && IsPlaying)
        {
            _dispatcherTimer.Start();
        }
        else
        {
            _dispatcherTimer.Stop();
        }
    }

    // ── Render ──

    public override void Render(DrawingContext context)
    {
        if (_source is null) return;

        var rect = CalDisplayRect();
        var croppedBitmap = new CroppedBitmap(_source, rect);
        context.DrawImage(croppedBitmap, new Rect(0, 0, Bounds.Width, Bounds.Height));
    }

    private PixelRect CalDisplayRect()
    {
        if (_currentIndex > _indexMax)
        {
            _currentIndex = _indexMin;
        }

        var x = _currentIndex % _columns * _blockWidth;
        var y = _currentIndex / _columns * _blockHeight;

        var rect = new PixelRect(x, y, _blockWidth, _blockHeight);
        _currentIndex++;
        return rect;
    }
}

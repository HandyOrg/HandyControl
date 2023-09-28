using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;


namespace HandyControl.Controls;

/// <summary>
///     时间条
/// </summary>
[TemplatePart(Name = ElementBorderTop, Type = typeof(Border))]
[TemplatePart(Name = ElementTextBlockMove, Type = typeof(TextBlock))]
[TemplatePart(Name = ElementTextBlockSelected, Type = typeof(TextBlock))]
[TemplatePart(Name = ElementCanvasSpe, Type = typeof(Canvas))]
[TemplatePart(Name = ElementHotspots, Type = typeof(Panel))]
public class TimeBar : Control
{
    #region Constants

    private const string ElementBorderTop = "PART_BorderTop";

    private const string ElementTextBlockMove = "PART_TextBlockMove";

    private const string ElementTextBlockSelected = "PART_TextBlockSelected";

    private const string ElementCanvasSpe = "PART_CanvasSpe";

    private const string ElementHotspots = "PART_Hotspots";

    #endregion Constants

    #region Data

    private Border _borderTop;

    private TextBlock _textBlockMove;

    private TextBlock _textBlockSelected;

    private Canvas _canvasSpe;

    private Panel _panelHotspots;

    #endregion Data

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Bindable(true)]
    public Collection<DateTimeRange> Hotspots { get; }

    public static readonly DependencyProperty HotspotsBrushProperty = DependencyProperty.Register(
        nameof(HotspotsBrush), typeof(Brush), typeof(TimeBar), new PropertyMetadata(default(Brush)));

    public Brush HotspotsBrush
    {
        get => (Brush) GetValue(HotspotsBrushProperty);
        set => SetValue(HotspotsBrushProperty, value);
    }

    /// <summary>
    ///     是否显示刻度字符串
    /// </summary>
    public static readonly DependencyProperty ShowSpeStrProperty = DependencyProperty.Register(
        nameof(ShowSpeStr), typeof(bool), typeof(TimeBar), new PropertyMetadata(ValueBoxes.FalseBox));

    /// <summary>
    ///     是否显示刻度字符串
    /// </summary>
    public bool ShowSpeStr
    {
        get => (bool) GetValue(ShowSpeStrProperty);
        set => SetValue(ShowSpeStrProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
        nameof(TimeFormat), typeof(string), typeof(TimeBar), new PropertyMetadata("yyyy-MM-dd HH:mm:ss"));

    public string TimeFormat
    {
        get => (string) GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    /// <summary>
    ///     刻度字符串
    /// </summary>
    internal static readonly DependencyProperty SpeStrProperty = DependencyProperty.Register(
        nameof(SpeStr), typeof(string), typeof(TimeBar), new PropertyMetadata(Properties.Langs.Lang.Interval1h));

    /// <summary>
    ///     刻度字符串
    /// </summary>
    internal string SpeStr
    {
        get => (string) GetValue(SpeStrProperty);
        set => SetValue(SpeStrProperty, value);
    }

    /// <summary>
    ///     选中时间
    /// </summary>
    public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
        nameof(SelectedTime), typeof(DateTime), typeof(TimeBar), new PropertyMetadata(default(DateTime), OnSelectedTimeChanged));

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TimeBar { _textBlockSelected: { } } timeBar)
        {
            timeBar.OnSelectedTimeChanged((DateTime) e.NewValue);
        }
    }

    /// <summary>
    ///     选中时间
    /// </summary>
    public DateTime SelectedTime
    {
        get => (DateTime) GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    private void OnSelectedTimeChanged(DateTime time)
    {
        _textBlockSelected.Text = time.ToString(TimeFormat);
        if (!(_isDragging || _borderTopIsMouseLeftButtonDown))
        {
            _totalOffsetX = (_starTime - SelectedTime).TotalMilliseconds / _timeSpeList[SpeIndex] * _itemWidth;
        }
        UpdateSpeBlock();
        UpdateMouseFollowBlockPos();
    }

    /// <summary>
    ///     时间改变事件
    /// </summary>
    public static readonly RoutedEvent TimeChangedEvent =
        EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<DateTime>>), typeof(TimeBar));

    /// <summary>
    ///     刻度集合
    /// </summary>
    private readonly List<SpeTextBlock> _speBlockList = new();

    /// <summary>
    ///     初始化时时间
    /// </summary>
    private readonly DateTime _starTime;

    /// <summary>
    ///     时间段集合
    /// </summary>
    private readonly List<int> _timeSpeList = new()
    {
        7200000,
        3600000,
        1800000,
        600000,
        300000,
        60000,
        30000
    };

    /// <summary>
    ///     顶部border是否被按下
    /// </summary>
    private bool _borderTopIsMouseLeftButtonDown;

    /// <summary>
    ///     控件是否处于拖动中
    /// </summary>
    private bool _isDragging;

    /// <summary>
    ///     刻度单项宽度
    /// </summary>
    private double _itemWidth;

    /// <summary>
    ///     鼠标按下拖动时选中的时间
    /// </summary>
    private DateTime _mouseDownTime;

    /// <summary>
    ///     显示的刻度数目
    /// </summary>
    private int _speCount = 13;

    /// <summary>
    ///     刻度区间编号
    /// </summary>
    private int _speIndex = 1;

    /// <summary>
    ///     刻度单次偏移
    /// </summary>
    private double _tempOffsetX;

    /// <summary>
    ///     刻度总偏移
    /// </summary>
    private double _totalOffsetX;

    private readonly bool _isLoaded;

    private readonly SortedSet<DateTimeRange> _dateTimeRanges;

    public TimeBar()
    {
        _starTime = DateTime.Now;
        SelectedTime = new DateTime(_starTime.Year, _starTime.Month, _starTime.Day, 0, 0, 0);
        _starTime = SelectedTime;
        _isLoaded = true;

        var hotspots = new ObservableCollection<DateTimeRange>();
        _dateTimeRanges = new SortedSet<DateTimeRange>(ComparerGenerator.GetComparer<DateTimeRange>());
        hotspots.CollectionChanged += Items_CollectionChanged;
        Hotspots = hotspots;
    }

    private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (DateTimeRange item in e.NewItems)
            {
                _dateTimeRanges.Add(item);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (DateTimeRange item in e.OldItems)
            {
                _dateTimeRanges.Remove(item);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            foreach (DateTimeRange item in e.OldItems)
            {
                _dateTimeRanges.Remove(item);
            }

            foreach (DateTimeRange item in e.NewItems)
            {
                _dateTimeRanges.Add(item);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            _dateTimeRanges.Clear();
        }
    }

    public override void OnApplyTemplate()
    {
        if (_borderTop != null)
        {
            _borderTop.MouseLeftButtonDown -= BorderTop_OnMouseLeftButtonDown;
            _borderTop.PreviewMouseLeftButtonUp -= BorderTop_OnPreviewMouseLeftButtonUp;
        }

        base.OnApplyTemplate();

        _borderTop = GetTemplateChild(ElementBorderTop) as Border;
        _textBlockMove = GetTemplateChild(ElementTextBlockMove) as TextBlock;
        _textBlockSelected = GetTemplateChild(ElementTextBlockSelected) as TextBlock;
        _canvasSpe = GetTemplateChild(ElementCanvasSpe) as Canvas;
        _panelHotspots = GetTemplateChild(ElementHotspots) as Panel;

        CheckNull();
        _borderTop.MouseLeftButtonDown += BorderTop_OnMouseLeftButtonDown;
        _borderTop.PreviewMouseLeftButtonUp += BorderTop_OnPreviewMouseLeftButtonUp;
        var behavior = new MouseDragElementBehaviorEx
        {
            LockY = true,
        };
        behavior.DragBegun += DragElementBehavior_OnDragBegun;
        behavior.Dragging += MouseDragElementBehavior_OnDragging;
        behavior.DragFinished += MouseDragElementBehavior_OnDragFinished;
        var collection = Interaction.GetBehaviors(_borderTop);
        collection.Add(behavior);

        if (_isLoaded)
        {
            Update();
        }
        _textBlockSelected.Text = SelectedTime.ToString(TimeFormat);
    }

    private void CheckNull()
    {
        if (_borderTop == null || _textBlockMove == null || _textBlockSelected == null || _canvasSpe == null) throw new Exception();
    }

    /// <summary>
    ///     刻度区间编号
    /// </summary>
    public int SpeIndex
    {
        get => _speIndex;
        private set
        {
            if (_speIndex == value) return;

            if (value < 0)
            {
                SpeStr = Properties.Langs.Lang.Interval2h;
                _speIndex = 0;
                return;
            }
            if (value > 6)
            {
                SpeStr = Properties.Langs.Lang.Interval30s;
                _speIndex = 6;
                return;
            }
            SetSpeTimeFormat("HH:mm");
            switch (value)
            {
                case 0:
                    SpeStr = Properties.Langs.Lang.Interval2h;
                    break;
                case 1:
                    SpeStr = Properties.Langs.Lang.Interval1h;
                    break;
                case 2:
                    SpeStr = Properties.Langs.Lang.Interval30m;
                    break;
                case 3:
                    SpeStr = Properties.Langs.Lang.Interval10m;
                    break;
                case 4:
                    SpeStr = Properties.Langs.Lang.Interval5m;
                    break;
                case 5:
                    SpeStr = Properties.Langs.Lang.Interval1m;
                    break;
                case 6:
                    SetSpeTimeFormat("HH:mm:ss");
                    SpeStr = Properties.Langs.Lang.Interval30s;
                    break;
            }
            _speIndex = value;
        }
    }

    /// <summary>
    ///     时间改变事件
    /// </summary>
    public event EventHandler<FunctionEventArgs<DateTime>> TimeChanged
    {
        add => AddHandler(TimeChangedEvent, value);
        remove => RemoveHandler(TimeChangedEvent, value);
    }

    /// <summary>
    ///     设置刻度时间格式
    /// </summary>
    /// <param name="format"></param>
    private void SetSpeTimeFormat(string format)
    {
        foreach (var item in _speBlockList)
            item.TimeFormat = format;
    }

    /// <summary>
    ///     更新刻度
    /// </summary>
    private void UpdateSpeBlock()
    {
        var rest = (_totalOffsetX + _tempOffsetX) % _itemWidth;
        for (var i = 0; i < _speCount; i++)
        {
            var item = _speBlockList[i];
            item.MoveX(rest + (_itemWidth - item.Width) / 2);
        }
        var sub = rest <= 0 ? _speCount / 2 : _speCount / 2 - 1;

        for (var i = 0; i < _speCount; i++)
            _speBlockList[i].Time = TimeConvert(SelectedTime).AddMilliseconds((i - sub) * _timeSpeList[_speIndex]);

        if (_panelHotspots != null && _dateTimeRanges.Any())
        {
            UpdateHotspots();
        }
    }

    /// <summary>
    ///     时间转换
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private DateTime TimeConvert(DateTime time)
    {
        return _speIndex switch
        {
            0 => new DateTime(time.Year, time.Month, time.Day, time.Hour / 2 * 2, 0, 0),
            1 => new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0),
            2 => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 30 * 30, 0),
            3 => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 10 * 10, 0),
            4 => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 5 * 5, 0),
            5 => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0),
            6 => new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second / 30 * 30),
            _ => time
        };
    }

    /// <summary>
    ///     鼠标滚轮滚动时改变刻度区间
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        if (Mouse.LeftButton == MouseButtonState.Pressed) return;
        SpeIndex += e.Delta > 0 ? 1 : -1;
        _totalOffsetX = (_starTime - SelectedTime).TotalMilliseconds / _timeSpeList[SpeIndex] * _itemWidth;
        UpdateSpeBlock();
        UpdateMouseFollowBlockPos();
        e.Handled = true;
    }

    private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
    {
        _isDragging = true;
        _tempOffsetX = _borderTop.RenderTransform.Value.OffsetX;

        SelectedTime = _mouseDownTime - TimeSpan.FromMilliseconds(_tempOffsetX / _itemWidth * _timeSpeList[_speIndex]);
        _borderTopIsMouseLeftButtonDown = false;
    }

    private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e)
    {
        _tempOffsetX = 0;
        _totalOffsetX = (_totalOffsetX + _borderTop.RenderTransform.Value.OffsetX) % ActualWidth;
        _borderTop.RenderTransform = new TranslateTransform();
        RaiseEvent(new FunctionEventArgs<DateTime>(TimeChangedEvent, this)
        {
            Info = SelectedTime
        });
        _isDragging = false;
    }

    private void DragElementBehavior_OnDragBegun(object sender, MouseEventArgs e) => _mouseDownTime = SelectedTime;

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        Update();
    }

    /// <summary>
    ///     更新
    /// </summary>
    private void Update()
    {
        if (_canvasSpe == null) return;

        _speBlockList.Clear();
        _canvasSpe.Children.Clear();
        _speCount = (int) (ActualWidth / 800 * 9) | 1;

        var itemWidthOld = _itemWidth;
        _itemWidth = ActualWidth / _speCount;
        _totalOffsetX = _itemWidth / itemWidthOld * _totalOffsetX % ActualWidth;
        if (double.IsNaN(_totalOffsetX))
        {
            _totalOffsetX = 0;
        }

        var rest = (_totalOffsetX + _tempOffsetX) % _itemWidth;
        var sub = rest <= 0 || double.IsNaN(rest) ? _speCount / 2 : _speCount / 2 - 1;
        for (var i = 0; i < _speCount; i++)
        {
            var block = new SpeTextBlock
            {
                Time = TimeConvert(SelectedTime).AddMilliseconds((i - sub) * _timeSpeList[_speIndex]),
                TextAlignment = TextAlignment.Center,
                TimeFormat = "HH:mm"
            };
            _speBlockList.Add(block);
            _canvasSpe.Children.Add(block);
        }

        if (_speIndex == 6)
        {
            SetSpeTimeFormat("HH:mm:ss");
        }

        ShowSpeStr = ActualWidth > 320;
        for (var i = 0; i < _speCount; i++)
        {
            var item = _speBlockList[i];
            item.X = _itemWidth * i;
            item.MoveX((_itemWidth - item.Width) / 2);
        }

        UpdateSpeBlock();
        UpdateMouseFollowBlockPos();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        UpdateMouseFollowBlockPos();
    }

    /// <summary>
    ///     更新鼠标跟随块位置
    /// </summary>
    private void UpdateMouseFollowBlockPos()
    {
        var p = Mouse.GetPosition(this);
        var mlliseconds = (p.X - ActualWidth / 2) / _itemWidth * _timeSpeList[_speIndex];
        if (double.IsNaN(mlliseconds) || double.IsInfinity(mlliseconds)) return;
        _textBlockMove.Text = mlliseconds < 0
            ? (SelectedTime - TimeSpan.FromMilliseconds(-mlliseconds)).ToString(TimeFormat)
            : (SelectedTime + TimeSpan.FromMilliseconds(mlliseconds)).ToString(TimeFormat);
        _textBlockMove.Margin = new Thickness(p.X - _textBlockMove.ActualWidth / 2, 2, 0, 0);
    }

    private void UpdateHotspots()
    {
        var mlliseconds = ActualWidth * 0.5 / _itemWidth * _timeSpeList[_speIndex];
        if (double.IsNaN(mlliseconds) || double.IsInfinity(mlliseconds)) return;
        _panelHotspots.Children.Clear();

        foreach (var rect in GetHotspotsRectangle(mlliseconds))
        {
            _panelHotspots.Children.Add(rect);
        }
    }

    private void BorderTop_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _borderTopIsMouseLeftButtonDown = true;

    private void BorderTop_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_borderTopIsMouseLeftButtonDown)
        {
            _borderTopIsMouseLeftButtonDown = false;

            var p = Mouse.GetPosition(this);
            _tempOffsetX = ActualWidth / 2 - p.X;
            SelectedTime -= TimeSpan.FromMilliseconds(_tempOffsetX / _itemWidth * _timeSpeList[_speIndex]);
            _totalOffsetX = (_totalOffsetX + _tempOffsetX) % ActualWidth;
            _tempOffsetX = 0;
            UpdateMouseFollowBlockPos();
            RaiseEvent(new FunctionEventArgs<DateTime>(TimeChangedEvent, this)
            {
                Info = SelectedTime
            });
        }
    }

    private IEnumerable<Rectangle> GetHotspotsRectangle(double mlliseconds)
    {
        var timeSpan = TimeSpan.FromMilliseconds(mlliseconds);
        var selectedTime = SelectedTime;
        var start = selectedTime - timeSpan;
        var end = selectedTime + timeSpan;

        var set = _dateTimeRanges.GetViewBetween(new DateTimeRange(start), new DateTimeRange(end));
        var unitLength = ActualWidth / mlliseconds * 0.5;

        foreach (var range in set)
        {
            var width = range.TotalMilliseconds * unitLength;
            var sub = range.Start - start;
            var x = sub.TotalMilliseconds * unitLength;
            yield return new Rectangle
            {
                Fill = HotspotsBrush,
                Height = 4,
                Width = width,
                Margin = new Thickness(x, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }
    }
}

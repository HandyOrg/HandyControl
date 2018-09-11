using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     TimeBar.xaml 的交互逻辑
    /// </summary>
    public partial class TimeBar
    {
        /// <summary>
        ///     是否显示刻度字符串
        /// </summary>
        public static readonly DependencyProperty ShowSpeStrProperty = DependencyProperty.Register(
            "ShowSpeStr", typeof(bool), typeof(TimeBar), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     刻度字符串
        /// </summary>
        private static readonly DependencyProperty SpeStrProperty = DependencyProperty.Register(
            "SpeStr", typeof(string), typeof(TimeBar), new PropertyMetadata(Properties.Langs.Lang.Interval1h));

        /// <summary>
        ///     选中时间
        /// </summary>
        private static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
            "SelectedTime", typeof(DateTime), typeof(TimeBar));

        /// <summary>
        ///     时间改变事件
        /// </summary>
        public static readonly RoutedEvent TimeChangedEvent =
            EventManager.RegisterRoutedEvent("ButtonCloseClick", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<DateTime>>), typeof(TimeBar));

        /// <summary>
        ///     刻度集合
        /// </summary>
        private readonly List<SpeTextBlock> _speBlockList = new List<SpeTextBlock>();

        /// <summary>
        ///     初始化时时间
        /// </summary>
        private readonly DateTime _starTime;

        /// <summary>
        ///     时间段集合
        /// </summary>
        private readonly List<int> _timeSpeList = new List<int>
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

        public TimeBar()
        {
            InitializeComponent();

            _starTime = DateTime.Now;
            SelectedTime = new DateTime(_starTime.Year, _starTime.Month, _starTime.Day, 0, 0, 0);
            _starTime = SelectedTime;
        }

        /// <summary>
        ///     是否显示刻度字符串
        /// </summary>
        public bool ShowSpeStr
        {
            get => (bool)GetValue(ShowSpeStrProperty);
            set => SetValue(ShowSpeStrProperty, value);
        }

        /// <summary>
        ///     刻度字符串
        /// </summary>
        private string SpeStr
        {
            get => (string)GetValue(SpeStrProperty);
            set => SetValue(SpeStrProperty, value);
        }

        /// <summary>
        ///     选中时间
        /// </summary>
        private DateTime SelectedTime
        {
            get => (DateTime)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
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
        ///     设置显示的时间
        /// </summary>
        /// <param name="time"></param>
        public void SetShowTime(DateTime time)
        {
            if (_isDragging || _borderTopIsMouseLeftButtonDown) return;

            SelectedTime = time;
            _totalOffsetX = (_starTime - SelectedTime).TotalMilliseconds / _timeSpeList[SpeIndex] * _itemWidth;
            Update();
            TimeBar_OnMouseMove(null, null);
        }

        /// <summary>
        ///     更新
        /// </summary>
        private void Update()
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
        }

        /// <summary>
        ///     时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime TimeConvert(DateTime time)
        {
            switch (_speIndex)
            {
                case 0:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour / 2 * 2, 0, 0);
                case 1:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
                case 2:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 30 * 30, 0);
                case 3:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 10 * 10, 0);
                case 4:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 5 * 5, 0);
                case 5:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
                case 6:
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second / 30 * 30);
                default:
                    return time;
            }
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
            Update();
            TimeBar_OnMouseMove(null, null);
            e.Handled = true;
        }

        private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
        {
            _isDragging = true;
            _tempOffsetX = BorderTop.RenderTransform.Value.OffsetX;

            SelectedTime = _mouseDownTime -
                           TimeSpan.FromMilliseconds(_tempOffsetX / _itemWidth * _timeSpeList[_speIndex]);
            Update();
            _borderTopIsMouseLeftButtonDown = false;
        }

        private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e)
        {
            _tempOffsetX = 0;
            _totalOffsetX = (_totalOffsetX + BorderTop.RenderTransform.Value.OffsetX) % ActualWidth;
            BorderTop.RenderTransform = new TranslateTransform();
            RaiseEvent(new FunctionEventArgs<DateTime>(TimeChangedEvent, this)
            {
                Info = SelectedTime
            });
            _isDragging = false;
        }

        private void DragElementBehavior_OnDragBegun(object sender, MouseEventArgs e)
        {
            _mouseDownTime = SelectedTime;
        }

        private void TimeBar_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _speBlockList.Clear();
            CanvasSpe.Children.Clear();

            _speCount = (int)(ActualWidth / 800 * 9) | 1;

            var itemWidthOld = _itemWidth;
            _itemWidth = ActualWidth / _speCount;
            _totalOffsetX = _itemWidth / itemWidthOld * _totalOffsetX % ActualWidth;
            if (double.IsNaN(_totalOffsetX))
                _totalOffsetX = 0;

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
                CanvasSpe.Children.Add(block);
            }
            if (_speIndex == 6)
                SetSpeTimeFormat("HH:mm:ss");
            ShowSpeStr = ActualWidth > 320;
            for (var i = 0; i < _speCount; i++)
            {
                var item = _speBlockList[i];
                item.X = _itemWidth * i;
                item.MoveX((_itemWidth - item.Width) / 2);
            }

            Update();
            TimeBar_OnMouseMove(null, null);
        }

        private void TimeBar_OnMouseMove(object sender, MouseEventArgs e)
        {
            var p = Mouse.GetPosition(this);
            var mlliseconds = (p.X - ActualWidth / 2) / _itemWidth * _timeSpeList[_speIndex];
            if (double.IsInfinity(mlliseconds)) return;
            TextBlockSelected.Text = mlliseconds < 0
                ? (SelectedTime - TimeSpan.FromMilliseconds(-mlliseconds)).ToString("yyyy-MM-dd HH:mm:ss")
                : (SelectedTime + TimeSpan.FromMilliseconds(mlliseconds)).ToString("yyyy-MM-dd HH:mm:ss");
            TextBlockSelected.Margin = new Thickness(p.X - TextBlockSelected.ActualWidth / 2, 2, 0, 0);
        }

        private void BorderTop_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _borderTopIsMouseLeftButtonDown = true;
        }

        private void BorderTop_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_borderTopIsMouseLeftButtonDown)
            {
                _borderTopIsMouseLeftButtonDown = false;

                var p = Mouse.GetPosition(this);
                _tempOffsetX = ActualWidth / 2 - p.X;
                SelectedTime -= TimeSpan.FromMilliseconds(_tempOffsetX / _itemWidth * _timeSpeList[_speIndex]);
                Update();
                _totalOffsetX = (_totalOffsetX + _tempOffsetX) % ActualWidth;
                _tempOffsetX = 0;
                TimeBar_OnMouseMove(null, null);
                RaiseEvent(new FunctionEventArgs<DateTime>(TimeChangedEvent, this)
                {
                    Info = SelectedTime
                });
            }
        }
    }
}
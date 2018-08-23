using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <summary>
    ///     Carousel.xaml 的交互逻辑
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    public partial class Carousel
    {
        public static readonly DependencyProperty AutoRunProperty = DependencyProperty.Register(
            "AutoRun", typeof(bool), typeof(Carousel), new PropertyMetadata(default(bool), (o, args) =>
            {
                var ctl = (Carousel)o;
                ctl.TimerSwitch((bool)args.NewValue);
            }));

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(TimeSpan), typeof(Carousel), new PropertyMetadata(TimeSpan.FromSeconds(2)));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(Carousel), new PropertyMetadata(default(IEnumerable),
                (o, args) =>
                {
                    var ctl = (Carousel)o;
                    ctl.UpdatePageButtons();
                }));

        public static readonly DependencyProperty ExtendWidthProperty = DependencyProperty.Register(
            "ExtendWidth", typeof(double), typeof(Carousel), new PropertyMetadata(default(double)));

        public double ExtendWidth
        {
            get => (double)GetValue(ExtendWidthProperty);
            set => SetValue(ExtendWidthProperty, value);
        }

        public static readonly DependencyProperty IsCenterProperty = DependencyProperty.Register(
            "IsCenter", typeof(bool), typeof(Carousel), new PropertyMetadata(default(bool)));

        public bool IsCenter
        {
            get => (bool)GetValue(IsCenterProperty);
            set => SetValue(IsCenterProperty, value);
        }

        private int _pageIndex = -1;

        private Button _selectedButton;

        private DispatcherTimer _updateTimer;

        private readonly List<double> _widthList = new List<double>();

        public Carousel()
        {
            InitializeComponent();

            Loaded += (s, e) => UpdatePageButtons();
        }

        /// <summary>
        ///     是否自动跳转
        /// </summary>
        public bool AutoRun
        {
            get => (bool)GetValue(AutoRunProperty);
            set => SetValue(AutoRunProperty, value);
        }

        /// <summary>
        ///     跳转时间间隔
        /// </summary>
        public TimeSpan Interval
        {
            get => (TimeSpan)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (Items.Count == 0) return;
                if (_pageIndex == value) return;
                if (value < 0)
                    _pageIndex = Items.Count - 1;
                else if (value >= Items.Count)
                    _pageIndex = 0;
                else
                    _pageIndex = value;
                UpdatePageButtons(_pageIndex);
            }
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public ItemCollection Items => ListBoxMain.Items;

        /// <summary>
        ///     计时器开关
        /// </summary>
        private void TimerSwitch(bool run)
        {
            if (run)
            {
                _updateTimer = new DispatcherTimer
                {
                    Interval = Interval
                };
                _updateTimer.Tick += UpdateTimer_Tick;
                _updateTimer.Start();
            }
            else
            {
                _updateTimer.Tick -= UpdateTimer_Tick;
                _updateTimer.Stop();
                _updateTimer = null;
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (IsMouseOver) return;
            PageIndex++;
        }

        /// <summary>
        ///     更新页按钮
        /// </summary>
        public void UpdatePageButtons(int? index = null)
        {
            if (index == null)
            {
                var count = Items.Count;
                _widthList.Clear();
                _widthList.Add(0);
                var width = .0;
                foreach (FrameworkElement item in ListBoxMain.Items)
                {
                    width += item.Width + item.Margin.Left + item.Margin.Right;
                    _widthList.Add(width);
                }

                ListBoxMain.Width = _widthList.Last() + ExtendWidth;
                PanelPage.Children.Clear();
                for (var i = 0; i < count; i++)
                    PanelPage.Children.Add(CreatePateButton());
                if (count > 0)
                {
                    var button = PanelPage.Children[0];
                    button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, button));
                }
            }
            else
            {
                if (index >= 0 && index < PanelPage.Children.Count)
                {
                    var button = PanelPage.Children[(int)index];
                    button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, button));
                    UpdateItemsPosition();
                }
            }
        }

        /// <summary>
        ///     更新项的位置
        /// </summary>
        private void UpdateItemsPosition()
        {
            if (Items.Count == 0 || PageIndex == -1) return;
            if (!IsCenter)
            {
                ListBoxMain.BeginAnimation(MarginProperty,
                    AnimationHelper.CreateAnimation(new Thickness(-_widthList[PageIndex], 0, 0, 0)));
            }
            else
            {
                var ctl = (FrameworkElement)Items[PageIndex];
                var ctlWidth = ctl.Width + ctl.Margin.Left + ctl.Margin.Right;
                ListBoxMain.BeginAnimation(MarginProperty,
                    AnimationHelper.CreateAnimation(
                        new Thickness(-_widthList[PageIndex] + (ActualWidth - ctlWidth) / 2, 0, 0, 0)));
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateItemsPosition();
        }

        /// <summary>
        ///     创建页按钮
        /// </summary>
        /// <returns></returns>
        private Button CreatePateButton()
        {
            var button = new Button
            {
                Style = TryFindResource("ButtonOpacityStyle") as Style,
                Content = new Border
                {
                    Width = 10,
                    Height = 10,
                    CornerRadius = new CornerRadius(5),
                    Background = Brushes.White,
                    Margin = new Thickness(5, 0, 5, 0),
                    BorderThickness = new Thickness(1),
                    BorderBrush = TryFindResource("PrimaryBrush") as Brush
                }
            };
            return button;
        }

        private void ButtonPages_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null && _selectedButton.Content is Border borderOri)
                borderOri.Background = Brushes.White;
            _selectedButton = e.OriginalSource as Button;
            if (_selectedButton != null && _selectedButton.Content is Border border)
                border.Background = TryFindResource("PrimaryBrush") as Brush;
            var index = PanelPage.Children.IndexOf(_selectedButton);
            if (index != -1)
                PageIndex = index;
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex--;
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex++;
        }
    }
}
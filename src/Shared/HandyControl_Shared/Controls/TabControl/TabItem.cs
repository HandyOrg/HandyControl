using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        /// <summary>
        ///     动画速度
        /// </summary>
        private const int AnimationSpeed = 150;

        /// <summary>
        ///     选项卡是否处于拖动状态
        /// </summary>
        private static bool ItemIsDragging;

        /// <summary>
        ///     选项卡是否等待被拖动
        /// </summary>
        private bool _isWaiting;

        /// <summary>
        ///     拖动中的选项卡坐标
        /// </summary>
        private Point _dragPoint;

        /// <summary>
        ///     鼠标按下时选项卡位置
        /// </summary>
        private int _mouseDownIndex;

        /// <summary>
        ///     鼠标按下时选项卡横向偏移
        /// </summary>
        private double _mouseDownOffsetX;

        /// <summary>
        ///     鼠标按下时的坐标
        /// </summary>
        private Point _mouseDownPoint;

        /// <summary>
        ///     右侧可移动的最大值
        /// </summary>
        private double _maxMoveRight;

        /// <summary>
        ///     左侧可移动的最大值
        /// </summary>
        private double _maxMoveLeft;

        /// <summary>
        ///     选项卡宽度
        /// </summary>
        public double ItemWidth { get; internal set; }

        /// <summary>
        ///     选项卡拖动等待距离（在鼠标移动了超过20个像素无关单位后，选项卡才开始被拖动）
        /// </summary>
        private const double WaitLength = 20;

        /// <summary>
        ///     选项卡是否处于拖动状态
        /// </summary>
        private bool _isDragging;

        /// <summary>
        ///     选项卡是否已经被拖动
        /// </summary>
        private bool _isDragged;

        /// <summary>
        ///     目标横向位移
        /// </summary>
        internal double TargetOffsetX { get; set; }

        /// <summary>
        ///     当前编号
        /// </summary>
        private int _currentIndex;

        /// <summary>
        ///     标签容器横向滚动距离
        /// </summary>
        private double _scrollHorizontalOffset;

        private TabPanel _tabPanel;

        /// <summary>
        ///     标签容器
        /// </summary>
        internal TabPanel TabPanel
        {
            get
            {
                if (_tabPanel == null && TabControlParent != null)
                {
                    _tabPanel = TabControlParent.HeaderPanel;
                }

                return _tabPanel;
            }
            set => _tabPanel = value;
        }

        /// <summary>
        ///     当前编号
        /// </summary>
        internal int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex == value || value < 0) return;
                var oldIndex = _currentIndex;
                _currentIndex = value;
                UpdateItemOffsetX(oldIndex);
            }
        }

        /// <summary>
        ///     是否显示关闭按钮
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty =
            TabControl.ShowCloseButtonProperty.AddOwner(typeof(TabItem));

        /// <summary>
        ///     是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        public static void SetShowCloseButton(DependencyObject element, bool value)
            => element.SetValue(ShowCloseButtonProperty, value);

        public static bool GetShowCloseButton(DependencyObject element)
            => (bool)element.GetValue(ShowCloseButtonProperty);

        /// <summary>
        ///     是否显示上下文菜单
        /// </summary>
        public static readonly DependencyProperty ShowContextMenuProperty =
            TabControl.ShowContextMenuProperty.AddOwner(typeof(TabItem), new FrameworkPropertyMetadata(OnShowContextMenuChanged));

        private static void OnShowContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (TabItem) d;
            if (ctl.Menu != null)
            {
                var show = (bool)e.NewValue;
                ctl.Menu.IsEnabled = show;
                ctl.Menu.Show(show);
            }
        }

        /// <summary>
        ///     是否显示上下文菜单
        /// </summary>
        public bool ShowContextMenu
        {
            get => (bool) GetValue(ShowContextMenuProperty);
            set => SetValue(ShowContextMenuProperty, value);
        }

        public static void SetShowContextMenu(DependencyObject element, bool value)
            => element.SetValue(ShowContextMenuProperty, value);

        public static bool GetShowContextMenu(DependencyObject element)
            => (bool)element.GetValue(ShowContextMenuProperty);

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
            "Menu", typeof(ContextMenu), typeof(TabItem), new PropertyMetadata(default(ContextMenu), OnMenuChanged));

        private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (TabItem) d;
            ctl.OnMenuChanged(e.NewValue as ContextMenu);
        }

        private void OnMenuChanged(ContextMenu menu)
        {
            if (IsLoaded && menu != null)
            {
                var parent = TabControlParent;
                if (parent == null) return;

                var item = parent.ItemContainerGenerator.ItemFromContainer(this);

                menu.DataContext = item;
                menu.SetBinding(IsEnabledProperty, new Binding(ShowContextMenuProperty.Name)
                {
                    Source = this
                });
                menu.SetBinding(VisibilityProperty, new Binding(ShowContextMenuProperty.Name)
                {
                    Source = this,
                    Converter = ResourceHelper.GetResource<IValueConverter>(ResourceToken.Boolean2VisibilityConverter)
                });
            }
        }

        public ContextMenu Menu
        {
            get => (ContextMenu) GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }

        /// <summary>
        ///     更新选项卡横向偏移
        /// </summary>
        /// <param name="oldIndex"></param>
        private void UpdateItemOffsetX(int oldIndex)
        {
            if (!_isDragging) return;
            var moveItem = TabPanel.ItemDic[CurrentIndex];
            moveItem.CurrentIndex -= CurrentIndex - oldIndex;
            var offsetX = moveItem.TargetOffsetX;
            var resultX = offsetX + (oldIndex - CurrentIndex) * ItemWidth;
            TabPanel.ItemDic[CurrentIndex] = this;
            TabPanel.ItemDic[moveItem.CurrentIndex] = moveItem;
            moveItem.CreateAnimation(offsetX, resultX);
        }

        public TabItem()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
            CommandBindings.Add(new CommandBinding(ControlCommands.CloseAll,
                (s, e) => { TabControlParent.CloseAllItems(); }));
            CommandBindings.Add(new CommandBinding(ControlCommands.CloseOther,
                (s, e) => { TabControlParent.CloseOtherItems(this); }));

            Loaded += (s, e) => OnMenuChanged(Menu);
        }

        private TabControl TabControlParent => ItemsControl.ItemsControlFromItemContainer(this) as TabControl;

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (VisualTreeHelper.HitTest(this, e.GetPosition(this)) == null) return;
            IsSelected = true;
            Focus();
        }

        protected override void OnHeaderChanged(object oldHeader, object newHeader)
        {
            base.OnHeaderChanged(oldHeader, newHeader);

            if (TabPanel != null)
            {
                TabPanel.ForceUpdate = true;
                InvalidateMeasure();
                TabPanel.ForceUpdate = true;
            }
        }

        internal void Close()
        {
            var parent = TabControlParent;
            if (parent == null) return;

            var item = parent.ItemContainerGenerator.ItemFromContainer(this);

            var argsClosing = new CancelRoutedEventArgs(ClosingEvent, item);
            RaiseEvent(argsClosing);
            if (argsClosing.Cancel) return;

            TabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, parent.IsAnimationEnabled
                    ? new Duration(TimeSpan.FromMilliseconds(200))
                    : new Duration(TimeSpan.FromMilliseconds(1)));
            
            parent.IsInternalAction = true;
            RaiseEvent(new RoutedEventArgs(ClosedEvent, item));

            var list = parent.GetActualList();
            list?.Remove(item);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (VisualTreeHelper.HitTest(this, e.GetPosition(this)) == null) return;
            var parent = TabControlParent;
            if (parent == null) return;

            if (parent.IsDraggable && !ItemIsDragging && !_isDragging)
            {
                parent.UpdateScroll();
                TabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromSeconds(0)));
                _mouseDownOffsetX = RenderTransform.Value.OffsetX;
                _scrollHorizontalOffset = parent.GetHorizontalOffset();
                var mx = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                _mouseDownIndex = CalLocationIndex(mx);
                var subIndex = _mouseDownIndex - CalLocationIndex(_scrollHorizontalOffset);
                _maxMoveLeft = -subIndex * ItemWidth;
                _maxMoveRight = parent.ActualWidth - ActualWidth + _maxMoveLeft;

                _isDragging = true;
                ItemIsDragging = true;
                _isWaiting = true;
                _dragPoint = e.GetPosition(parent);
                _dragPoint = new Point(_dragPoint.X + _scrollHorizontalOffset, _dragPoint.Y);
                _mouseDownPoint = _dragPoint;
                CaptureMouse();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (ItemIsDragging && _isDragging)
            {
                var parent = TabControlParent;
                if (parent == null) return;

                var subX = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                CurrentIndex = CalLocationIndex(subX);
                
                var p = e.GetPosition(parent);
                p = new Point(p.X + _scrollHorizontalOffset, p.Y);
                
                var subLeft = p.X - _dragPoint.X;
                var totalLeft = p.X - _mouseDownPoint.X;

                if (Math.Abs(subLeft) <= WaitLength && _isWaiting) return;
                
                _isWaiting = false;
                _isDragged = true;
                
                var left = subLeft + RenderTransform.Value.OffsetX;
                if (totalLeft < _maxMoveLeft)
                {
                    left = _maxMoveLeft + _mouseDownOffsetX;
                }
                else if (totalLeft > _maxMoveRight)
                {
                    left = _maxMoveRight + _mouseDownOffsetX;
                }
                
                var t = new TranslateTransform(left, 0);
                RenderTransform = t;
                _dragPoint = p;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            
            ReleaseMouseCapture();
            
            if (_isDragged)
            {
                var parent = TabControlParent;
                if (parent == null) return;

                var subX = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                var index = CalLocationIndex(subX);
                var left = index * ItemWidth;
                var offsetX = RenderTransform.Value.OffsetX;
                CreateAnimation(offsetX, offsetX - subX + left, index);
            }
            
            _isDragging = false;
            ItemIsDragging = false;
            _isDragged = false;
        }

        /// <summary>
        ///     创建动画
        /// </summary>
        internal void CreateAnimation(double offsetX, double resultX, int index = -1)
        {
            var parent = TabControlParent;

            void AnimationCompleted()
            {
                RenderTransform = new TranslateTransform(resultX, 0);
                if (index == -1) return;

                var list = parent.GetActualList();
                if (list == null) return;

                var item = parent.ItemContainerGenerator.ItemFromContainer(this);
                if (item == null) return;

                TabPanel.CanUpdate = false;
                parent.IsInternalAction = true;

                list.Remove(item);
                parent.IsInternalAction = true;
                list.Insert(index, item);
                _tabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(0)));
                TabPanel.CanUpdate = true;
                TabPanel.ForceUpdate = true;
                TabPanel.Measure(new Size(TabPanel.DesiredSize.Width, ActualHeight));
                TabPanel.ForceUpdate = false;
                
                Focus();
                IsSelected = true;

                if (!IsMouseCaptured)
                {
                    parent.SetCurrentValue(Selector.SelectedIndexProperty, _currentIndex);
                }
            }

            TargetOffsetX = resultX;
            if (!parent.IsAnimationEnabled)
            {
                AnimationCompleted();
                return;
            }

            var animation = AnimationHelper.CreateAnimation(resultX, AnimationSpeed);
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s1, e1) => AnimationCompleted();
            var f = new TranslateTransform(offsetX, 0);
            RenderTransform = f;
            f.BeginAnimation(TranslateTransform.XProperty, animation, HandoffBehavior.Compose);
        }

        /// <summary>
        ///     计算选项卡当前合适的位置编号
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private int CalLocationIndex(double left)
        {
            if (_isWaiting)
            {
                return CurrentIndex;
            } 
            
            var maxIndex = TabControlParent.Items.Count - 1;
            var div = (int)(left / ItemWidth);
            var rest = left % ItemWidth;
            var result = rest / ItemWidth > .5 ? div + 1 : div;

            return result > maxIndex ? maxIndex : result;
        }

        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(EventHandler), typeof(TabItem));

        public event EventHandler Closing
        {
            add => AddHandler(ClosingEvent, value);
            remove => RemoveHandler(ClosingEvent, value);
        }

        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(TabItem));

        public event EventHandler Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = OverflowButtonKey, Type = typeof(ContextMenuToggleButton))]
    [TemplatePart(Name = HeaderPanelKey, Type = typeof(TabPanel))]
    [TemplatePart(Name = OverflowScrollviewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ScrollButtonLeft, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ScrollButtonRight, Type = typeof(ButtonBase))]
    [TemplatePart(Name = HeaderBorder, Type = typeof(Border))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        private const string OverflowButtonKey = "PART_OverflowButton";

        private const string HeaderPanelKey = "PART_HeaderPanel";

        private const string OverflowScrollviewer = "PART_OverflowScrollviewer";

        private const string ScrollButtonLeft = "PART_ScrollButtonLeft";

        private const string ScrollButtonRight = "PART_ScrollButtonRight";

        private const string HeaderBorder = "PART_HeaderBorder";

        private ContextMenuToggleButton _buttonOverflow;

        internal TabPanel HeaderPanel { get; private set; }

        private ScrollViewer _scrollViewerOverflow;

        private ButtonBase _buttonScrollLeft;

        private ButtonBase _buttonScrollRight;

        private Border _headerBorder;

        /// <summary>
        ///     是否为内部操作
        /// </summary>
        internal bool IsInternalAction;

        /// <summary>
        ///     是否启用动画
        /// </summary>
        public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register(
            "IsAnimationEnabled", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否启用动画
        /// </summary>
        public bool IsAnimationEnabled
        {
            get => (bool)GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        /// <summary>
        ///     是否可以拖动
        /// </summary>
        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register(
            "IsDraggable", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否可以拖动
        /// </summary>
        public bool IsDraggable
        {
            get => (bool)GetValue(IsDraggableProperty);
            set => SetValue(IsDraggableProperty, value);
        }

        /// <summary>
        ///     是否显示关闭按钮
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.RegisterAttached(
            "ShowCloseButton", typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetShowCloseButton(DependencyObject element, bool value)
            => element.SetValue(ShowCloseButtonProperty, value);

        public static bool GetShowCloseButton(DependencyObject element)
            => (bool) element.GetValue(ShowCloseButtonProperty);

        /// <summary>
        ///     是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        /// <summary>
        ///     是否显示上下文菜单
        /// </summary>
        public static readonly DependencyProperty ShowContextMenuProperty = DependencyProperty.RegisterAttached(
            "ShowContextMenu", typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetShowContextMenu(DependencyObject element, bool value)
            => element.SetValue(ShowContextMenuProperty, value);

        public static bool GetShowContextMenu(DependencyObject element)
            => (bool) element.GetValue(ShowContextMenuProperty);

        /// <summary>
        ///     是否显示上下文菜单
        /// </summary>
        public bool ShowContextMenu
        {
            get => (bool)GetValue(ShowContextMenuProperty);
            set => SetValue(ShowContextMenuProperty, value);
        }

        /// <summary>
        ///     是否将标签填充
        /// </summary>
        public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(
            "IsTabFillEnabled", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否将标签填充
        /// </summary>
        public bool IsTabFillEnabled
        {
            get => (bool)GetValue(IsTabFillEnabledProperty);
            set => SetValue(IsTabFillEnabledProperty, value);
        }

        /// <summary>
        ///     标签宽度
        /// </summary>
        public static readonly DependencyProperty TabItemWidthProperty = DependencyProperty.Register(
            "TabItemWidth", typeof(double), typeof(TabControl), new PropertyMetadata(200.0));

        /// <summary>
        ///     标签宽度
        /// </summary>
        public double TabItemWidth
        {
            get => (double)GetValue(TabItemWidthProperty);
            set => SetValue(TabItemWidthProperty, value);
        }

        /// <summary>
        ///     标签高度
        /// </summary>
        public static readonly DependencyProperty TabItemHeightProperty = DependencyProperty.Register(
            "TabItemHeight", typeof(double), typeof(TabControl), new PropertyMetadata(30.0));

        /// <summary>
        ///     标签高度
        /// </summary>
        public double TabItemHeight
        {
            get => (double)GetValue(TabItemHeightProperty);
            set => SetValue(TabItemHeightProperty, value);
        }

        /// <summary>
        ///     是否可以滚动
        /// </summary>
        public static readonly DependencyProperty IsScrollableProperty = DependencyProperty.Register(
            "IsScrollable", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否可以滚动
        /// </summary>
        public bool IsScrollable
        {
            get => (bool) GetValue(IsScrollableProperty);
            set => SetValue(IsScrollableProperty, value);
        }

        /// <summary>
        ///     是否显示溢出按钮
        /// </summary>
        public static readonly DependencyProperty ShowOverflowButtonProperty = DependencyProperty.Register(
            "ShowOverflowButton", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.TrueBox));

        /// <summary>
        ///     是否显示溢出按钮
        /// </summary>
        public bool ShowOverflowButton
        {
            get => (bool) GetValue(ShowOverflowButtonProperty);
            set => SetValue(ShowOverflowButtonProperty, value);
        }

        /// <summary>
        ///     是否显示滚动按钮
        /// </summary>
        public static readonly DependencyProperty ShowScrollButtonProperty = DependencyProperty.Register(
            "ShowScrollButton", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否显示滚动按钮
        /// </summary>
        public bool ShowScrollButton
        {
            get => (bool) GetValue(ShowScrollButtonProperty);
            set => SetValue(ShowScrollButtonProperty, value);
        }

        /// <summary>
        ///     可见的标签数量
        /// </summary>
        private int _itemShowCount;

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (HeaderPanel == null)
            {
                IsInternalAction = false;
                return;
            }

            UpdateOverflowButton();

            if (IsInternalAction)
            {
                IsInternalAction = false;
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    if (!(ItemContainerGenerator.ContainerFromIndex(i) is TabItem item)) return;
                    item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    item.TabPanel = HeaderPanel;
                }
            }

            _headerBorder?.InvalidateMeasure();
            IsInternalAction = false;
        }

        public override void OnApplyTemplate()
        {
            if (_buttonOverflow != null)
            {
                if (_buttonOverflow.Menu != null)
                {
                    _buttonOverflow.Menu.Closed -= Menu_Closed;
                    _buttonOverflow.Menu = null;
                }

                _buttonOverflow.Click -= ButtonOverflow_Click;
            }

            if (_buttonScrollLeft != null) _buttonScrollLeft.Click -= ButtonScrollLeft_Click;
            if (_buttonScrollRight != null) _buttonScrollRight.Click -= ButtonScrollRight_Click;

            base.OnApplyTemplate();
            HeaderPanel = GetTemplateChild(HeaderPanelKey) as TabPanel;

            if (IsTabFillEnabled) return;

            _buttonOverflow = GetTemplateChild(OverflowButtonKey) as ContextMenuToggleButton;
            _scrollViewerOverflow = GetTemplateChild(OverflowScrollviewer) as ScrollViewer;
            _buttonScrollLeft = GetTemplateChild(ScrollButtonLeft) as ButtonBase;
            _buttonScrollRight = GetTemplateChild(ScrollButtonRight) as ButtonBase;
            _headerBorder = GetTemplateChild(HeaderBorder) as Border;

            if (_buttonScrollLeft != null) _buttonScrollLeft.Click += ButtonScrollLeft_Click;
            if (_buttonScrollRight != null) _buttonScrollRight.Click += ButtonScrollRight_Click;

            if (_buttonOverflow != null)
            {
                var menu = new ContextMenu
                {
                    Placement = PlacementMode.Bottom,
                    PlacementTarget = _buttonOverflow
                };
                menu.Closed += Menu_Closed;
                _buttonOverflow.Menu = menu;
                _buttonOverflow.Click += ButtonOverflow_Click;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateOverflowButton();
        }

        private void UpdateOverflowButton()
        {
            if (!IsTabFillEnabled)
            {
                _itemShowCount = (int)(ActualWidth / TabItemWidth);
                _buttonOverflow?.Show(ShowOverflowButton && Items.Count > 0 && Items.Count >= _itemShowCount);
            }
        }

        private void Menu_Closed(object sender, RoutedEventArgs e) => _buttonOverflow.IsChecked = false;

        private void ButtonScrollRight_Click(object sender, RoutedEventArgs e) =>
            _scrollViewerOverflow.ScrollToHorizontalOffsetInternal(Math.Min(
                _scrollViewerOverflow.CurrentHorizontalOffset + TabItemWidth, _scrollViewerOverflow.ScrollableWidth));

        private void ButtonScrollLeft_Click(object sender, RoutedEventArgs e) =>
            _scrollViewerOverflow.ScrollToHorizontalOffsetInternal(Math.Max(
                _scrollViewerOverflow.CurrentHorizontalOffset - TabItemWidth, 0));

        private void ButtonOverflow_Click(object sender, RoutedEventArgs e)
        {
            if (_buttonOverflow.IsChecked == true)
            {
                _buttonOverflow.Menu.Items.Clear();
                for (var i = 0; i < Items.Count; i++)
                {
                    if(!(ItemContainerGenerator.ContainerFromIndex(i) is TabItem item)) continue;

                    var menuItem = new MenuItem
                    {
                        HeaderStringFormat = ItemStringFormat,
                        HeaderTemplate = ItemTemplate,
                        HeaderTemplateSelector = ItemTemplateSelector,
                        Header = item.Header,
                        Width = TabItemWidth,
                        IsChecked = item.IsSelected,
                        IsCheckable = true
                    };

                    menuItem.Click += delegate
                    {
                        _buttonOverflow.IsChecked = false;

                        var list = GetActualList();
                        if (list == null) return;

                        var actualItem = ItemContainerGenerator.ItemFromContainer(item);
                        if (actualItem == null) return;

                        var index = list.IndexOf(actualItem);
                        if (index >= _itemShowCount)
                        {
                            list.Remove(actualItem);
                            list.Insert(0, actualItem);
                            if (IsAnimationEnabled)
                            {
                                HeaderPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(200)));
                            }
                            else
                            {
                                HeaderPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(0)));
                            }
                            HeaderPanel.ForceUpdate = true;
                            HeaderPanel.Measure(new Size(HeaderPanel.DesiredSize.Width, ActualHeight));
                            HeaderPanel.ForceUpdate = false;
                            SetCurrentValue(SelectedIndexProperty, ValueBoxes.Int0Box);
                        }

                        item.IsSelected = true;
                    };
                    _buttonOverflow.Menu.Items.Add(menuItem);
                }
            }
        }

        internal double GetHorizontalOffset() => _scrollViewerOverflow?.CurrentHorizontalOffset ?? 0;

        internal void UpdateScroll() => _scrollViewerOverflow?.RaiseEvent(new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, 0)
        {
            RoutedEvent = MouseWheelEvent
        });

        internal void CloseAllItems() => CloseOtherItems(null);

        internal void CloseOtherItems(TabItem currentItem)
        {
            var actualItem = currentItem != null ? ItemContainerGenerator.ItemFromContainer(currentItem) : null;

            var list = GetActualList();
            if (list == null) return;

            IsInternalAction = true;

            for (var i = 0; i < Items.Count; i++)
            {
                var item = list[i];
                if (!Equals(item, actualItem) && item != null)
                {
                    var argsClosing = new CancelRoutedEventArgs(TabItem.ClosingEvent, item);

                    if (!(ItemContainerGenerator.ContainerFromItem(item) is TabItem tabItem)) continue;

                    tabItem.RaiseEvent(argsClosing);
                    if (argsClosing.Cancel) return;

                    tabItem.RaiseEvent(new RoutedEventArgs(TabItem.ClosedEvent, item));
                    list.Remove(item);

                    i--;
                }
            }

            SetCurrentValue(SelectedIndexProperty, Items.Count == 0 ? -1 : 0);
        }

        internal IList GetActualList()
        {
            IList list;
            if (ItemsSource != null)
            {
                list = ItemsSource as IList;
            }
            else
            {
                list = Items;
            }

            return list;
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is TabItem;

        protected override DependencyObject GetContainerForItemOverride() => new TabItem();
    }
}
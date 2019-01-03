using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = OverflowButtonKey, Type = typeof(ContextMenuToggleButton))]
    [TemplatePart(Name = HeaderPanelKey, Type = typeof(TabPanel))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        private const string OverflowButtonKey = "PART_OverflowButton";

        private const string HeaderPanelKey = "PART_HeaderPanel";

        private ContextMenuToggleButton _buttonOverflow;

        private TabPanel _headerPanel;

        /// <summary>
        ///     是否为内部操作
        /// </summary>
        internal bool IsInternalAction;

        /// <summary>
        ///     是否启用动画
        /// </summary>
        public static readonly DependencyProperty IsEnableAnimationProperty = DependencyProperty.Register(
            "IsEnableAnimation", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否启用动画
        /// </summary>
        public bool IsEnableAnimation
        {
            get => (bool)GetValue(IsEnableAnimationProperty);
            set => SetValue(IsEnableAnimationProperty, value);
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
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        /// <summary>
        ///     是否将标签填充
        /// </summary>
        public static readonly DependencyProperty IsEnableTabFillProperty = DependencyProperty.Register(
            "IsEnableTabFill", typeof(bool), typeof(TabControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否将标签填充
        /// </summary>
        public bool IsEnableTabFill
        {
            get => (bool)GetValue(IsEnableTabFillProperty);
            set => SetValue(IsEnableTabFillProperty, value);
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
        ///     可见的标签数量
        /// </summary>
        private int _itemShowCount;

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (_headerPanel == null)
            {
                IsInternalAction = false;
                return;
            }

            if (!IsEnableTabFill)
            {
                _itemShowCount = (int)(ActualWidth / TabItemWidth);
                _buttonOverflow.Show(Items.Count > 0  && Items.Count >= _itemShowCount);
            }

            if (IsInternalAction)
            {
                IsInternalAction = false;
                return;
            }
            if (IsEnableAnimation)
            {
                _headerPanel.ClearValue(TabPanel.FluidMoveDurationProperty);
            }
            else
            {
                _headerPanel.FluidMoveDuration = new Duration(TimeSpan.FromSeconds(0));
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TabItem item in e.NewItems)
                {
                    item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    item.IsSelected = true;
                    item.TabPanel = _headerPanel;
                }
            }
            IsInternalAction = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerPanel = Template.FindName(HeaderPanelKey, this) as TabPanel;

            if (IsEnableTabFill) return;

            _buttonOverflow = Template.FindName(OverflowButtonKey, this) as ContextMenuToggleButton;
            if (_buttonOverflow != null)
            {
                _itemShowCount = (int)(ActualWidth / TabItemWidth);
                _buttonOverflow.Show(Items.Count > 0 && Items.Count >= _itemShowCount);

                var menu = new ContextMenu
                {
                    Placement = PlacementMode.Bottom,
                    PlacementTarget = _buttonOverflow
                };
                menu.Closed += (s, e) => _buttonOverflow.IsChecked = false;
                _buttonOverflow.Menu = menu;
                _buttonOverflow.Click += (s, e) =>
                {
                    if (_buttonOverflow.IsChecked == true)
                    {
                        _buttonOverflow.Menu.Items.Clear();
                        foreach (TabItem item in Items)
                        {
                            var menuItem = new MenuItem
                            {
                                Header = item.Header,
                                Width = TabItemWidth,
                                IsChecked = item.IsSelected,
                                IsCheckable = true
                            };
                            menuItem.Click += delegate
                            {
                                _buttonOverflow.IsChecked = false;
                                var index = Items.IndexOf(item);
                                if (index >= _itemShowCount)
                                {
                                    Items.Remove(item);
                                    Items.Insert(0, item);
                                    if (IsEnableAnimation)
                                    {
                                        _headerPanel.ClearValue(TabPanel.FluidMoveDurationProperty);
                                    }
                                    else
                                    {
                                        _headerPanel.FluidMoveDuration = new Duration(TimeSpan.FromSeconds(0));
                                    }
                                    _headerPanel.ForceUpdate = true;
                                    _headerPanel.Measure(new Size(_headerPanel.DesiredSize.Width, ActualHeight));
                                    _headerPanel.ForceUpdate = false;
                                }
                                item.IsSelected = true;
                            };
                            _buttonOverflow.Menu.Items.Add(menuItem);
                        }
                    }
                };
            }
        }
    }
}
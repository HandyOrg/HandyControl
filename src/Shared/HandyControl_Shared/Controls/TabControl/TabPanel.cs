using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class TabPanel : Panel
    {
        private int _itemCount;

        /// <summary>
        ///     是否可以更新
        /// </summary>
        internal bool CanUpdate = true;

        /// <summary>
        ///     选项卡字典
        /// </summary>
        internal Dictionary<int, TabItem> ItemDic = new Dictionary<int, TabItem>();

        public static readonly DependencyPropertyKey FluidMoveDurationPropertyKey =
            DependencyProperty.RegisterReadOnly("FluidMoveDuration", typeof(Duration), typeof(TabPanel),
                new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(0))));

        /// <summary>
        ///     流式行为持续时间
        /// </summary>
        public static readonly DependencyProperty FluidMoveDurationProperty =
            FluidMoveDurationPropertyKey.DependencyProperty;

        /// <summary>
        ///     流式行为持续时间
        /// </summary>
        public Duration FluidMoveDuration
        {
            get => (Duration)GetValue(FluidMoveDurationProperty);
            set => SetValue(FluidMoveDurationProperty, value);
        }

        /// <summary>
        ///     是否将标签填充
        /// </summary>
        public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(
            "IsTabFillEnabled", typeof(bool), typeof(TabPanel), new PropertyMetadata(ValueBoxes.FalseBox));

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
            "TabItemWidth", typeof(double), typeof(TabPanel), new PropertyMetadata(200.0));

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
            "TabItemHeight", typeof(double), typeof(TabPanel), new PropertyMetadata(30.0));

        /// <summary>
        ///     标签高度
        /// </summary>
        public double TabItemHeight
        {
            get => (double)GetValue(TabItemHeightProperty);
            set => SetValue(TabItemHeightProperty, value);
        }

        /// <summary>
        ///     是否可以强制更新
        /// </summary>
        internal bool ForceUpdate;

        private Size _oldSize;

        /// <summary>
        ///     是否已经加载
        /// </summary>
        private bool _isLoaded;

        protected override Size MeasureOverride(Size constraint)
        {
            if ((_itemCount == InternalChildren.Count || !CanUpdate) && !ForceUpdate && !IsTabFillEnabled) return _oldSize;
            constraint.Height = TabItemHeight;
            _itemCount = InternalChildren.Count;

            var size = new Size();

            ItemDic.Clear();

            var count = InternalChildren.Count;
            if (count == 0)
            {
                _oldSize = new Size();
                return _oldSize;
            }
            constraint.Width += InternalChildren.Count;

            var itemWidth = .0;
            var arr = new int[count];

            if (!IsTabFillEnabled)
            {
                itemWidth = TabItemWidth;
            }
            else
            {
                if (TemplatedParent is TabControl tabControl)
                {
                    arr = ArithmeticHelper.DivideInt2Arr((int)tabControl.ActualWidth + InternalChildren.Count, count);
                }
            }

            for (var index = 0; index < count; index++)
            {
                if (IsTabFillEnabled)
                {
                    itemWidth = arr[index];
                }
                if (InternalChildren[index] is TabItem tabItem)
                {
                    tabItem.RenderTransform = new TranslateTransform();
                    tabItem.MaxWidth = itemWidth;
                    var rect = new Rect
                    {
                        X = size.Width - tabItem.BorderThickness.Left,
                        Width = itemWidth,
                        Height = TabItemHeight
                    };
                    tabItem.Arrange(rect);
                    tabItem.ItemWidth = itemWidth - tabItem.BorderThickness.Left;
                    tabItem.CurrentIndex = index;
                    tabItem.TargetOffsetX = 0;
                    ItemDic[index] = tabItem;
                    size.Width += tabItem.ItemWidth;
                }
            }
            size.Height = constraint.Height;
            _oldSize = size;
            return _oldSize;
        }

        public TabPanel()
        {
            Loaded += (s, e) =>
            {
                if (_isLoaded) return;
                ForceUpdate = true;
                Measure(new Size(DesiredSize.Width, ActualHeight));
                ForceUpdate = false;
                foreach (var item in ItemDic.Values)
                {
                    item.TabPanel = this;
                }
                _isLoaded = true;
            };
        }
    }
}
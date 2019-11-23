﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <summary>
    ///     轮播控件
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    [TemplatePart(Name = ElementPanelPage, Type = typeof(Panel))]
    [TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsPresenter))]
    public class Carousel : ListBox
    {
        #region Constants

        private const string ElementPanelPage = "PART_PanelPage";
        private const string ElementItemsControl = "PART_ItemsControl";

        #endregion Constants

        #region Data

        private Panel _panelPage;

        private bool _appliedTemplate;

        private ItemsPresenter _itemsControl;

        private int _pageIndex = -1;

        private RadioButton _selectedButton;

        private DispatcherTimer _updateTimer;

        private readonly List<double> _widthList = new List<double>();

        #endregion Data

        public override void OnApplyTemplate()
        {
            _appliedTemplate = false;

            _panelPage?.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ButtonPages_OnClick));

            base.OnApplyTemplate();

            _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsPresenter;
            _panelPage = GetTemplateChild(ElementPanelPage) as Panel;

            if (!CheckNull()) return;

            _panelPage.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ButtonPages_OnClick));
            _appliedTemplate = true;
            Update();
        }

        private void Update()
        {
            TimerSwitch(AutoRun);
            UpdatePageButtons(_pageIndex);
        }

        private bool CheckNull() => !(_itemsControl == null || _panelPage == null);

        public static readonly DependencyProperty AutoRunProperty = DependencyProperty.Register(
            "AutoRun", typeof(bool), typeof(Carousel), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
            {
                var ctl = (Carousel)o;
                ctl.TimerSwitch((bool)args.NewValue);
            }));

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(TimeSpan), typeof(Carousel), new PropertyMetadata(TimeSpan.FromSeconds(2)));

        public static readonly DependencyProperty ExtendWidthProperty = DependencyProperty.Register(
            "ExtendWidth", typeof(double), typeof(Carousel), new PropertyMetadata(ValueBoxes.Double0Box));

        public double ExtendWidth
        {
            get => (double)GetValue(ExtendWidthProperty);
            set => SetValue(ExtendWidthProperty, value);
        }

        public static readonly DependencyProperty IsCenterProperty = DependencyProperty.Register(
            "IsCenter", typeof(bool), typeof(Carousel), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsCenter
        {
            get => (bool)GetValue(IsCenterProperty);
            set => SetValue(IsCenterProperty, value);
        }

        public static readonly DependencyProperty PageButtonStyleProperty = DependencyProperty.Register(
            "PageButtonStyle", typeof(Style), typeof(Carousel), new PropertyMetadata(default(Style)));

        public Style PageButtonStyle
        {
            get => (Style) GetValue(PageButtonStyleProperty);
            set => SetValue(PageButtonStyleProperty, value);
        }

        public Carousel()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Prev, ButtonPrev_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Next, ButtonNext_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Selected, ButtonPages_OnClick));

            Loaded += (s, e) => UpdatePageButtons();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            UpdatePageButtons();
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

        /// <summary>
        ///     计时器开关
        /// </summary>
        private void TimerSwitch(bool run)
        {
            if (!_appliedTemplate) return;

            if (_updateTimer != null)
            {
                _updateTimer.Tick -= UpdateTimer_Tick;
                _updateTimer.Stop();
                _updateTimer = null;
            }

            if (!run) return;
            _updateTimer = new DispatcherTimer
            {
                Interval = Interval
            };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (IsMouseOver) return;
            PageIndex++;
        }

        /// <summary>
        ///     更新页按钮
        /// </summary>
        public void UpdatePageButtons(int index = -1)
        {
            if (!CheckNull()) return;
            if (!_appliedTemplate) return;

            var count = Items.Count;
            _widthList.Clear();
            _widthList.Add(0);
            var width = .0;
            foreach (FrameworkElement item in Items)
            {
                item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                width += item.DesiredSize.Width;
                _widthList.Add(width);
            }

            _itemsControl.Width = _widthList.Last() + ExtendWidth;
            _panelPage.Children.Clear();
            for (var i = 0; i < count; i++)
            {
                _panelPage.Children.Add(new RadioButton
                {
                    Style = PageButtonStyle
                });
            }

            if (index == -1 && count > 0) index = 0;
            if (index >= 0 && index < count)
            {
                if (_panelPage.Children[index] is RadioButton button)
                {
                    button.IsChecked = true;
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
            if (!CheckNull()) return;
            if (!_appliedTemplate) return;
            if (Items.Count == 0) return;
            if (!IsCenter)
            {
                _itemsControl.BeginAnimation(MarginProperty,
                    AnimationHelper.CreateAnimation(new Thickness(-_widthList[PageIndex], 0, 0, 0)));
            }
            else
            {
                var ctl = (FrameworkElement)Items[PageIndex];
                var ctlWidth = ctl.DesiredSize.Width;
                _itemsControl.BeginAnimation(MarginProperty,
                    AnimationHelper.CreateAnimation(
                        new Thickness(-_widthList[PageIndex] + (ActualWidth - ctlWidth) / 2, 0, 0, 0)));
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateItemsPosition();
        }

        private void ButtonPages_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckNull()) return;

            _selectedButton = e.OriginalSource as RadioButton;
            
            var index = _panelPage.Children.IndexOf(_selectedButton);
            if (index != -1)
            {
                PageIndex = index;
            }
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;
    }
}
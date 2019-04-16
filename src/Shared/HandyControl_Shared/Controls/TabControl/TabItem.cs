using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Interactivity;
using HandyControl.Tools;

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
        ///     标签容器
        /// </summary>
        internal TabPanel TabPanel { get; set; }

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
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, Close));
            CommandBindings.Add(new CommandBinding(ControlCommands.CloseAll, (s, e) =>
            {
                TabControlParent.IsInternalAction = true;
                TabControlParent.Items.Clear();
            }));
            CommandBindings.Add(new CommandBinding(ControlCommands.CloseOther, (s, e) =>
            {
                TabControlParent.IsInternalAction = true;
                var enumerator = ((IEnumerable)TabControlParent.Items).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    if (!Equals(item) && item != null)
                    {
                        TabControlParent.Items.Remove(item);
                        enumerator = ((IEnumerable)TabControlParent.Items).GetEnumerator();
                    }
                }
            }));
        }

        private TabControl TabControlParent => new Lazy<TabControl>(() => ItemsControl.ItemsControlFromItemContainer(this) as TabControl).Value;

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            IsSelected = true;
            Focus();
        }

        /// <summary>
        ///     关闭
        /// </summary>
        private void Close(object sender, RoutedEventArgs e)
        {
            if (TabControlParent.IsEnableAnimation)
            {
                TabPanel.ClearValue(TabPanel.FluidMoveDurationProperty);
            }
            else
            {
                TabPanel.FluidMoveDuration = new Duration(TimeSpan.FromSeconds(0));
            }
            TabControlParent.IsInternalAction = true;
            TabControlParent.Items.Remove(this);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (TabControlParent.IsDraggable && !ItemIsDragging && !_isDragging)
            {
                TabPanel.FluidMoveDuration = new Duration(TimeSpan.FromSeconds(0));
                _mouseDownOffsetX = RenderTransform.Value.OffsetX;
                var mx = TranslatePoint(new Point(), TabControlParent).X;
                _mouseDownIndex = CalLocationIndex(mx);
                _maxMoveLeft = -_mouseDownIndex * ItemWidth;
                _maxMoveRight = TabControlParent.ActualWidth - ActualWidth + _maxMoveLeft;

                _isDragging = true;
                ItemIsDragging = true;
                _isWaiting = true;
                _dragPoint = e.GetPosition(TabControlParent);
                _mouseDownPoint = _dragPoint;
                CaptureMouse();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ItemIsDragging && _isDragging)
            {
                var subX = TranslatePoint(new Point(), TabControlParent).X;
                CurrentIndex = CalLocationIndex(subX);
                var p = e.GetPosition(TabControlParent);
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
                var subX = TranslatePoint(new Point(), TabControlParent).X;
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
            void AnimationCompleted()
            {
                RenderTransform = new TranslateTransform(resultX, 0);
                if (index == -1) return;
                var parent = TabControlParent;
                TabPanel.CanUpdate = false;
                parent.IsInternalAction = true;
                parent.Items.Remove(this);
                parent.IsInternalAction = true;
                parent.Items.Insert(index, this);
                TabPanel.CanUpdate = true;
                TabPanel.ForceUpdate = true;
                TabPanel.Measure(new Size(TabPanel.DesiredSize.Width, ActualHeight));
                TabPanel.ForceUpdate = false;
                Focus();
                IsSelected = true;
            }
            TargetOffsetX = resultX;
            if (!TabControlParent.IsEnableAnimation)
            {
                AnimationCompleted();
                return;
            }
            var animation = AnimationHelper.CreateAnimation(resultX, AnimationSpeed);
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s1, e1) => AnimationCompleted();
            var f = new TranslateTransform(offsetX, 0);
            RenderTransform = f;
            f.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        ///     计算选项卡当前合适的位置编号
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private int CalLocationIndex(double left)
        {
            var maxIndex = TabControlParent.Items.Count - 1;
            var div = (int)(left / ItemWidth);
            var rest = left % ItemWidth;
            var result = rest / ItemWidth > .5 ? div + 1 : div;

            return result > maxIndex ? maxIndex : result;
        }
    }
}
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        private double _totalVerticalOffset;

        private bool _isRunning;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (!IsEnableInertia)
            {
                base.OnMouseWheel(e);
                return;
            }
            e.Handled = true;

            if (!_isRunning)
            {
                _totalVerticalOffset = VerticalOffset;
                CurrentVerticalOffset = VerticalOffset;
            }
            _totalVerticalOffset = Math.Min(Math.Max(0, _totalVerticalOffset - e.Delta), ScrollableHeight);
            var animation = AnimationHelper.CreateAnimation(_totalVerticalOffset, 500);
            animation.EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s,e1)=>
            {
                CurrentVerticalOffset = _totalVerticalOffset;
                _isRunning = false;
            };
            _isRunning = true;
            BeginAnimation(CurrentVerticalOffsetProperty, animation);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) =>
            IsPenetrating ? null : base.HitTestCore(hitTestParameters);

        /// <summary>
        ///     是否支持惯性
        /// </summary>
        public static readonly DependencyProperty IsEnableInertiaProperty = DependencyProperty.Register(
            "IsEnableInertia", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     是否支持惯性
        /// </summary>
        public bool IsEnableInertia
        {
            get => (bool) GetValue(IsEnableInertiaProperty);
            set => SetValue(IsEnableInertiaProperty, value);
        }

        /// <summary>
        ///     控件是否可以穿透点击
        /// </summary>
        public static readonly DependencyProperty IsPenetratingProperty = DependencyProperty.Register(
            "IsPenetrating", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     控件是否可以穿透点击
        /// </summary>
        public bool IsPenetrating
        {
            get => (bool) GetValue(IsPenetratingProperty);
            set => SetValue(IsPenetratingProperty, value);
        }

        /// <summary>
        ///     当前垂直滚动偏移
        /// </summary>
        private static readonly DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register(
            "CurrentVerticalOffset", typeof(double), typeof(ScrollViewer), new PropertyMetadata(default(double), CurrentVerticalOffsetChangedCallback));

        private static void CurrentVerticalOffsetChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer ctl && e.NewValue is double v)
            {
                ctl.ScrollToVerticalOffset(v);
            }
        }

        /// <summary>
        ///     当前垂直滚动偏移
        /// </summary>
        private double CurrentVerticalOffset
        {
            get => (double) GetValue(CurrentVerticalOffsetProperty);
            set => SetValue(CurrentVerticalOffsetProperty, value);
        }
    }
}
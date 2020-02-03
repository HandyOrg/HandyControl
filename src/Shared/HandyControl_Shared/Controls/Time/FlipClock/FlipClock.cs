using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HandyControl.Controls
{
    public class FlipClock : Control
    {
        private readonly DispatcherTimer _dispatcherTimer;

        private bool _isDisposed;

        public static readonly DependencyProperty NumberListProperty = DependencyProperty.Register(
            "NumberList", typeof(List<int>), typeof(FlipClock), new PropertyMetadata(new List<int> {0, 0, 0, 0, 0, 0}));

        public List<int> NumberList
        {
            get => (List<int>) GetValue(NumberListProperty);
            set => SetValue(NumberListProperty, value);
        }

        public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(
            "DisplayTime", typeof(DateTime), typeof(FlipClock), new PropertyMetadata(default(DateTime), OnDisplayTimeChanged));

        private static void OnDisplayTimeChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (FlipClock) s;
            var v = (DateTime) e.NewValue;

            ctl.NumberList = new List<int>
            {
                v.Hour / 10,
                v.Hour % 10,
                v.Minute / 10,
                v.Minute % 10,
                v.Second / 10,
                v.Second % 10
            };
        }

        public DateTime DisplayTime
        {
            get => (DateTime) GetValue(DisplayTimeProperty);
            set => SetValue(DisplayTimeProperty, value);
        }

        public FlipClock()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            IsVisibleChanged += FlipClock_IsVisibleChanged;
        }

        ~FlipClock() => Dispose();

        public void Dispose()
        {
            if (_isDisposed) return;

            IsVisibleChanged -= FlipClock_IsVisibleChanged;
            _dispatcherTimer.Stop();
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }

        private void FlipClock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                _dispatcherTimer.Tick += DispatcherTimer_Tick;
                _dispatcherTimer.Start();
            }
            else
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) => DisplayTime = DateTime.Now;
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HandyControl.Controls
{
    public class FlipClock : Control
    {
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
            var dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) => DisplayTime = DateTime.Now;
    }
}
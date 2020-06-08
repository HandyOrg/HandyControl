using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    /// <summary>
    ///     标记
    /// </summary>
    public class Badge : ContentControl
    {
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<int>>), typeof(Badge));

        public event EventHandler<FunctionEventArgs<int>> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Badge), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int0Box, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Badge)d;
            var v = (int)e.NewValue;
            ctl.Text = v <= ctl.Maximum ? v.ToString() : $"{ctl.Maximum}+";
            if (ctl.IsInitialized)
            {
                ctl.RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, ctl)
                {
                    Info = v
                });
            }
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(BadgeStatus), typeof(Badge), new PropertyMetadata(default(BadgeStatus)));

        public BadgeStatus Status
        {
            get => (BadgeStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int99Box, OnValueChanged));

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty BadgeMarginProperty = DependencyProperty.Register(
            "BadgeMargin", typeof(Thickness), typeof(Badge), new PropertyMetadata(default(Thickness)));

        public Thickness BadgeMargin
        {
            get => (Thickness)GetValue(BadgeMarginProperty);
            set => SetValue(BadgeMarginProperty, value);
        }

        public static readonly DependencyProperty ShowBadgeProperty = DependencyProperty.Register(
            "ShowBadge", typeof(bool), typeof(Badge), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowBadge
        {
            get => (bool) GetValue(ShowBadgeProperty);
            set => SetValue(ShowBadgeProperty, value);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize) => null;
    }
}
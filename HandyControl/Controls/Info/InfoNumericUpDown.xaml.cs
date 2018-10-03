using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Tools;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     InfoNumericUpDown.xaml 的交互逻辑
    /// </summary>
    internal partial class InfoNumericUpDown
    {
        public InfoNumericUpDown()
        {
            InitializeComponent();

            ErrorTextBlock = TextBlockError;
            ContentTextBox = MyTextBox;
            TitleStackPanel = StackPanelTitle;

            (TryFindResource("Storyboard2") as Storyboard)?.Begin();

            Loaded += (s, e) =>
            {
                StackPanelTitle.Focusable = true;
                StackPanelTitle.Focus();
                StackPanelTitle.Focusable = false;
            };
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(InfoNumericUpDown), new PropertyMetadata(100.0, (o, args) =>
            {
                o.CoerceValue(MinimumProperty);
                o.CoerceValue(ValueProperty);
            }, (o, value) =>
            {
                var info = (InfoNumericUpDown)o;
                var max = (double)value;
                if (max < info.Minimum) max = info.Minimum;
                return max;
            }), ValidateHelper.IsInRangeOfDouble);

        /// <summary>
        ///     最大值
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        ///     最小值
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(InfoNumericUpDown), new PropertyMetadata(0.0, (o, args) =>
            {
                o.CoerceValue(MaximumProperty);
                o.CoerceValue(ValueProperty);
            }, (o, value) =>
            {
                var info = (InfoNumericUpDown)o;
                var min = (double)value;
                if (min > info.Maximum) min = info.Maximum;
                return min;
            }), ValidateHelper.IsInRangeOfDouble);

        /// <summary>
        ///     最小值
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        ///     当前值
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(InfoNumericUpDown), new PropertyMetadata(1.0, (o, args) =>
            {
                o.CoerceValue(MinimumProperty);
                o.CoerceValue(MaximumProperty);
                var ctl = (InfoNumericUpDown)o;
                ctl.RaiseEvent(new FunctionEventArgs<double>(ValueChangedEvent, ctl)
                {
                    Info = (double)args.NewValue
                });
            }, (o, value) =>
            {
                var info = (InfoNumericUpDown)o;
                var current = (double)value;

                if (current < info.Minimum) current = info.Minimum;
                if (current > info.Maximum) current = info.Maximum;

                return current;
            }), ValidateHelper.IsInRangeOfDouble);

        /// <summary>
        ///     当前值
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     最小改变值
        /// </summary>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            "SmallChange", typeof(double), typeof(InfoNumericUpDown), new PropertyMetadata(1.0), ValidateHelper.IsInRangeOfDouble);

        /// <summary>
        ///     最小改变值
        /// </summary>
        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        /// <summary>
        ///     是否显示上下调整按钮
        /// </summary>
        public static readonly DependencyProperty ShowUpDownButtonProperty = DependencyProperty.Register(
            "ShowUpDownButton", typeof(bool), typeof(InfoNumericUpDown), new PropertyMetadata(true));

        /// <summary>
        ///     是否显示上下调整按钮
        /// </summary>
        public bool ShowUpDownButton
        {
            get => (bool)GetValue(ShowUpDownButtonProperty);
            set => SetValue(ShowUpDownButtonProperty, value);
        }

        /// <summary>
        ///     值改变事件
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<double>>), typeof(InfoNumericUpDown));

        /// <summary>
        ///     值改变事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<double>> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        private void ButtonUp_OnClick(object sender, RoutedEventArgs e) => Value += SmallChange;

        private void ButtonDown_OnClick(object sender, RoutedEventArgs e) => Value -= SmallChange;

        private void MyTextBox_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Value += SmallChange;
            }
            else
            {
                Value -= SmallChange;
            }
            MyTextBox.Select(MyTextBox.Text.Length, 0);
            e.Handled = true;
        }
    }
}
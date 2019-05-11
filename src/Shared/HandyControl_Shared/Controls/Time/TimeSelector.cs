using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HandyControl.Controls
{
    public class TimeSelector : Control
    {
        private ObservableCollection<TimeButton> HourButtons = new ObservableCollection<TimeButton>();
        private ObservableCollection<TimeButton> MinuteButtons = new ObservableCollection<TimeButton>();
        private ObservableCollection<TimeButton> SecondButtons = new ObservableCollection<TimeButton>();

        #region 控件内部元素
        private ListBox PART_Hour;
        private ListBox PART_Minute;
        private ListBox PART_Second;
        #endregion

        #region 私有属性
        private int Hour = 0;
        private int Minute = 0;
        private int Second = 0;
        #endregion

        #region Fields
        public TimePickerList Owner { get; set; }
        #endregion

        #region 事件定义

        #region SelectedTimeChanged
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(TimeSelector));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add
            {
                this.AddHandler(SelectedTimeChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(SelectedTimeChangedEvent, value);
            }
        }

        public virtual void OnSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedTimeChangedEvent);
            this.RaiseEvent(arg);
        }
        #endregion

        #endregion

        #region 依赖属性set get

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(TimeSelector), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;
            DateTime dt = (DateTime)e.NewValue;

            timeSelector.SetButtonSelected();

            timeSelector.OnSelectedTimeChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        public void SetButtonSelected()
        {
            if (!this.SelectedTime.HasValue)
            {
                return;
            }

            this.Hour = this.SelectedTime.Value.Hour;
            this.Minute = this.SelectedTime.Value.Minute;
            this.Second = this.SelectedTime.Value.Second;

            if (this.PART_Hour != null)
            {
                for (int i = 0; i < this.PART_Hour.Items.Count; i++)
                {
                    TimeButton timeButton = this.PART_Hour.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(this.SelectedTime.Value.Hour)))
                    {
                        this.PART_Hour.SelectedIndex = i;
                        this.PART_Hour.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (this.PART_Minute != null)
            {
                for (int i = 0; i < this.PART_Minute.Items.Count; i++)
                {
                    TimeButton timeButton = this.PART_Minute.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(this.SelectedTime.Value.Minute)))
                    {
                        this.PART_Minute.SelectedIndex = i;
                        this.PART_Minute.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (this.PART_Second != null)
            {
                for (int i = 0; i < this.PART_Second.Items.Count; i++)
                {
                    TimeButton timeButton = this.PART_Second.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(this.SelectedTime.Value.Second)))
                    {
                        this.PART_Second.SelectedIndex = i;
                        this.PART_Second.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }
        }
        #endregion

        #region SelectedHour
        public int? SelectedHour
        {
            get { return (int?)GetValue(SelectedHourProperty); }
            set { SetValue(SelectedHourProperty, value); }
        }

        public static readonly DependencyProperty SelectedHourProperty =
            DependencyProperty.Register("SelectedHour", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedHourChanged));

        private static void SelectedHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;
            timeSelector.Hour = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }
        #endregion

        #region SelectedMinute

        public int? SelectedMinute
        {
            get { return (int?)GetValue(SelectedMinuteProperty); }
            set { SetValue(SelectedMinuteProperty, value); }
        }

        public static readonly DependencyProperty SelectedMinuteProperty =
            DependencyProperty.Register("SelectedMinute", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedMinuteChanged));

        private static void SelectedMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;

            timeSelector.Minute = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }

        #endregion

        #region SelectedSecond

        public int? SelectedSecond
        {
            get { return (int?)GetValue(SelectedSecondProperty); }
            set { SetValue(SelectedSecondProperty, value); }
        }

        public static readonly DependencyProperty SelectedSecondProperty =
            DependencyProperty.Register("SelectedSecond", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedSecondChanged));

        private static void SelectedSecondChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;

            timeSelector.Second = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }

        #endregion

        #region ItemHeight
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(TimeSelector), new PropertyMetadata(28.0));
        #endregion

        #endregion

        #region Constructors
        static TimeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelector), new FrameworkPropertyMetadata(typeof(TimeSelector)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Hour = this.GetTemplateChild("PART_Hour") as ListBox;
            this.PART_Minute = this.GetTemplateChild("PART_Minute") as ListBox;
            this.PART_Second = this.GetTemplateChild("PART_Second") as ListBox;

            if (this.PART_Hour != null)
            {
                this.CreateHourButtons();
                this.PART_Hour.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(HourButton_Click), true);
            }

            if (this.PART_Minute != null)
            {
                this.CreateMinuteButtons();
                this.PART_Minute.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(MinuteButton_Click), true);
            }

            if (this.PART_Second != null)
            {
                this.CreateSecondButtons();
                this.PART_Second.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(SecondButton_Click), true);
            }
        }
        #endregion

        #region Private方法

        private void MinuteButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = this.PART_Minute.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedMinute = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Minute.AnimateScrollIntoView(selectedItem);
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = this.PART_Second.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedSecond = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Second.AnimateScrollIntoView(selectedItem);
        }

        private void HourButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = this.PART_Hour.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedHour = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Hour.AnimateScrollIntoView(selectedItem);
        }

        private void CreateHourButtons()
        {
            this.CreateItems(24, this.HourButtons);
            this.CreateExtraItem(this.HourButtons);
            this.PART_Hour.ItemsSource = this.HourButtons;
        }

        private void CreateMinuteButtons()
        {
            this.CreateItems(60, this.MinuteButtons);
            this.CreateExtraItem(this.MinuteButtons);
            this.PART_Minute.ItemsSource = this.MinuteButtons;
        }

        private void CreateSecondButtons()
        {
            this.CreateItems(60, this.SecondButtons);
            this.CreateExtraItem(this.SecondButtons);
            this.PART_Second.ItemsSource = this.SecondButtons;
        }

        private void CreateItems(int itemsCount, ObservableCollection<TimeButton> list)
        {
            for (int i = 0; i < itemsCount; i++)
            {
                TimeButton timeButton = new TimeButton();
                timeButton.SetValue(TimeButton.HeightProperty, this.ItemHeight);
                timeButton.SetValue(TimeButton.DataContextProperty, i);
                timeButton.SetValue(TimeButton.ContentProperty, (i < 10) ? "0" + i : i.ToString());
                timeButton.SetValue(TimeButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }

        private void CreateExtraItem(ObservableCollection<TimeButton> list)
        {
            double height = this.ItemHeight;
            if (this.Owner != null)
            {
                height = this.Owner.DropDownHeight;
            }
            else
            {
                height = double.IsNaN(this.Height) ? height : this.Height;
            }

            for (int i = 0; i < (height - this.ItemHeight) / this.ItemHeight; i++)
            {
                TimeButton timeButton = new TimeButton();
                timeButton.SetValue(TimeButton.HeightProperty, this.ItemHeight);
                timeButton.SetValue(TimeButton.IsEnabledProperty, false);
                timeButton.SetValue(TimeButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }

        private DateTime GetDateTime(int hour, int minute, int second)
        {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);
            return dt;
        }

        /// <summary>
        /// 设置选中的时间
        /// </summary>
        private void SetSelectedTime()
        {
            DateTime dt = this.GetDateTime(this.Hour, this.Minute, this.Second);
            this.SelectedTime = dt;
        }
        #endregion
    }
    public class ScrollViewerBehavior
    {
        public static DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset",
                                                typeof(double),
                                                typeof(ScrollViewerBehavior),
                                                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static void SetVerticalOffset(FrameworkElement target, double value)
        {
            target.SetValue(VerticalOffsetProperty, value);
        }
        public static double GetVerticalOffset(FrameworkElement target)
        {
            return (double)target.GetValue(VerticalOffsetProperty);
        }
        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollViewer = target as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }
    }
}
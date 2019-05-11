using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls
{
    public class TimePickerList : Control
    {
        #region Private属性
        private TextBox PART_TextBox;
        private TimeSelector PART_TimeSelector;
        private Popup PART_Popup;
        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #region TimeStringFormat
        public string TimeStringFormat
        {
            get { return (string)GetValue(TimeStringFormatProperty); }
            set { SetValue(TimeStringFormatProperty, value); }
        }

        public static readonly DependencyProperty TimeStringFormatProperty =
            DependencyProperty.Register("TimeStringFormat", typeof(string), typeof(TimePickerList), new PropertyMetadata("HH:mm:ss"));
        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(TimePickerList), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePickerList timePicker = d as TimePickerList;
            DateTime dt = (DateTime)e.NewValue;

            timePicker.PART_TimeSelector.SelectedTime = dt;
        }
        #endregion

        #region DropDownHeight

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }

        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(TimePickerList), new PropertyMetadata(168d));

        #endregion

        #endregion

        #region Constructors
        static TimePickerList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerList), new FrameworkPropertyMetadata(typeof(TimePickerList)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_TextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
            this.PART_TimeSelector = this.GetTemplateChild("PART_TimeSelector") as TimeSelector;
            this.PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
            if (this.PART_TimeSelector != null)
            {
                this.PART_TimeSelector.Owner = this;
                this.PART_TimeSelector.SelectedTimeChanged += PART_TimeSelector_SelectedTimeChanged;
            }
            if (this.PART_Popup != null)
            {
                this.PART_Popup.Opened += PART_Popup_Opened;
            }
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            this.PART_TimeSelector.SetButtonSelected();
        }
        #endregion

        #region Private方法
        private void PART_TimeSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (this.PART_TextBox != null && e.NewValue != null)
            {
                this.PART_TextBox.Text = e.NewValue.Value.ToString(this.TimeStringFormat);
                this.SelectedTime = e.NewValue;
            }
        }
        #endregion
    }
}
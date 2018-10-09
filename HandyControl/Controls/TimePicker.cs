﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using HandyControl.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
    [TemplatePart(Name = ElementButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    [TemplatePart(Name = ElementClock, Type = typeof(Clock))]
    public class TimePicker : Control
    {
        #region Constants

        private const string ElementRoot = "PART_Root";
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";
        private const string ElementPopup = "PART_Popup";
        private const string ElementClock = "PART_Clock";

        #endregion Constants

        #region Data

        private Clock _clock;
        private string _defaultText;
        private ButtonBase _dropDownButton;
        private Popup _popUp;
        private bool _disablePopupReopen;
        private WatermarkTextBox _textBox;
        private IDictionary<DependencyProperty, bool> _isHandlerSuspended;
        private DateTime? _originalSelectedTime;

        #endregion Data

        #region Public Events

        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct,
                typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(TimePicker));

        public event EventHandler<FunctionEventArgs<DateTime?>> SelectedTimeChanged
        {
            add => AddHandler(SelectedTimeChangedEvent, value);
            remove => RemoveHandler(SelectedTimeChangedEvent, value);
        }

        public event RoutedEventHandler ClockClosed;

        public event RoutedEventHandler ClockOpened;

        #endregion Public Events

        static TimePicker()
        {
            EventManager.RegisterClassHandler(typeof(TimePicker), GotFocusEvent, new RoutedEventHandler(OnGotFocus));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(false));
        }

        #region Public Properties

        #region TimeFormat

        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
            "TimeFormat", typeof(string), typeof(TimePicker), new PropertyMetadata("HH:mm:ss"));

        public string TimeFormat
        {
            get => (string)GetValue(TimeFormatProperty);
            set => SetValue(TimeFormatProperty, value);
        }

        #endregion TimeFormat

        #region ClockStyle

        public Style ClockStyle
        {
            get => (Style)GetValue(ClockStyleProperty);
            set => SetValue(ClockStyleProperty, value);
        }

        public static readonly DependencyProperty ClockStyleProperty = DependencyProperty.Register("ClockStyle", typeof(Style), typeof(TimePicker));

        #endregion CalendarStyle

        #region DisplayTime

        public DateTime DisplayTime
        {
            get => (DateTime)GetValue(DisplayTimeProperty);
            set => SetValue(DisplayTimeProperty, value);
        }

        public static readonly DependencyProperty DisplayTimeProperty =
            DependencyProperty.Register(
                "DisplayTime",
                typeof(DateTime),
                typeof(TimePicker),
                new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceDisplayTime));

        private static object CoerceDisplayTime(DependencyObject d, object value)
        {
            var dp = (TimePicker)d;
            dp._clock.DisplayTime = (DateTime)value;
            return dp._clock.DisplayTime;
        }

        #endregion DisplayTime

        #region IsDropDownOpen

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(
            "IsDropDownOpen",
            typeof(bool),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged, OnCoerceIsDropDownOpen));

        private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue) => d is TimePicker dp && !dp.IsEnabled ? false : baseValue;

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dp = d as TimePicker;

            var newValue = (bool)e.NewValue;
            if (dp?._popUp != null && dp._popUp.IsOpen != newValue)
            {
                dp._popUp.IsOpen = newValue;
                if (newValue)
                {
                    dp._originalSelectedTime = dp.SelectedTime;

                    dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate
                    {
                        dp._clock.Focus();
                    });
                }
            }
        }

        #endregion IsDropDownOpen

        #region SelectedTime

        public DateTime? SelectedTime
        {
            get => (DateTime?)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register(
            "SelectedTime",
            typeof(DateTime?),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged, CoerceSelectedTime));

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimePicker dp)) return;

            if (dp.SelectedTime.HasValue)
            {
                var time = dp.SelectedTime.Value;
                dp.SetTextInternal(dp.DateTimeToString(time));
            }

            dp.RaiseEvent(new FunctionEventArgs<DateTime?>(SelectedTimeChangedEvent, dp)
            {
                Info = dp.SelectedTime
            });
        }

        private static object CoerceSelectedTime(DependencyObject d, object value)
        {
            var dp = (TimePicker)d;
            dp._clock.SelectedTime = (DateTime?)value;
            return dp._clock.SelectedTime;
        }

        #endregion SelectedDate

        #region Text

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimePicker dp && !dp.IsHandlerSuspended(TextProperty))
            {
                if (e.NewValue is string newValue)
                {
                    if (dp._textBox != null)
                    {
                        dp._textBox.Text = newValue;
                    }
                    else
                    {
                        dp._defaultText = newValue;
                    }

                    dp.SetSelectedTime();
                }
                else
                {
                    dp.SetValueNoCallback(SelectedTimeProperty, null);
                }
            }
        }

        /// <summary>
        /// Sets the local Text property without breaking bindings
        /// </summary>
        /// <param name="value"></param>
        private void SetTextInternal(string value)
        {
            SetCurrentValue(TextProperty, value);
        }

        #endregion Text

        #endregion Public Properties

        #region Public Methods

        public override void OnApplyTemplate()
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            if (_popUp != null)
            {
                _popUp.RemoveHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(PopUp_PreviewMouseLeftButtonDown));
                _popUp.Opened -= PopUp_Opened;
                _popUp.Closed -= PopUp_Closed;
                _popUp.Child = null;
            }

            if (_dropDownButton != null)
            {
                _dropDownButton.Click -= DropDownButton_Click;
                _dropDownButton.RemoveHandler(MouseLeaveEvent, new MouseEventHandler(DropDownButton_MouseLeave));
            }

            if (_textBox != null)
            {
                _textBox.RemoveHandler(KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
                _textBox.RemoveHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(TextBox_TextChanged));
                _textBox.RemoveHandler(LostFocusEvent, new RoutedEventHandler(TextBox_LostFocus));
            }

            if (_clock != null)
            {
                _clock.RemoveHandler(Clock.SelectedTimeChangedEvent, new EventHandler<FunctionEventArgs<DateTime?>>(Clock_SelectedTimeChangedEvent));
                _clock.Confirmed -= Clock_Confirmed;
            }

            base.OnApplyTemplate();

            _clock = GetTemplateChild(ElementClock) as Clock;
            _popUp = GetTemplateChild(ElementPopup) as Popup;
            _dropDownButton = GetTemplateChild(ElementButton) as Button;
            _textBox = GetTemplateChild(ElementTextBox) as WatermarkTextBox;

            CheckNull();

            _clock.ApplyTemplate();
            _clock.AddHandler(Clock.SelectedTimeChangedEvent, new EventHandler<FunctionEventArgs<DateTime?>>(Clock_SelectedTimeChangedEvent));
            _clock.Confirmed += Clock_Confirmed;

            _popUp.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(PopUp_PreviewMouseLeftButtonDown));
            _popUp.Opened += PopUp_Opened;
            _popUp.Closed += PopUp_Closed;
            _popUp.Child = _clock;

            if (IsDropDownOpen)
            {
                _popUp.IsOpen = true;
            }

            _dropDownButton.Click += DropDownButton_Click;
            _dropDownButton.AddHandler(MouseLeaveEvent, new MouseEventHandler(DropDownButton_MouseLeave), true);
            if (SelectedTime == null)
            {
                _textBox.Text = DateTime.Now.ToString(TimeFormat);
            }
            _textBox.AddHandler(KeyDownEvent, new KeyEventHandler(TextBox_KeyDown), true);
            _textBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(TextBox_TextChanged), true);
            _textBox.AddHandler(LostFocusEvent, new RoutedEventHandler(TextBox_LostFocus), true);

            if (SelectedTime == null)
            {
                if (!string.IsNullOrEmpty(_defaultText))
                {
                    _textBox.Text = _defaultText;
                    SetSelectedTime();
                }
            }
            else
            {
                _textBox.Text = DateTimeToString((DateTime)SelectedTime);
            }

            if (_originalSelectedTime == null)
            {
                _originalSelectedTime = DateTime.Now;
            }
            SetCurrentValue(DisplayTimeProperty, _originalSelectedTime);
        }

        public override string ToString() => SelectedTime?.ToString(TimeFormat) ?? string.Empty;

        #endregion

        #region Protected Methods

        protected virtual void OnClockClosed(RoutedEventArgs e)
        {
            var handler = ClockClosed;
            handler?.Invoke(this, e);
        }

        protected virtual void OnClockOpened(RoutedEventArgs e)
        {
            var handler = ClockOpened;
            handler?.Invoke(this, e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void CheckNull()
        {
            if (_clock == null || _dropDownButton == null || _popUp == null || _textBox == null) throw new Exception();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSelectedTime();
        }

        private void SetIsHandlerSuspended(DependencyProperty property, bool value)
        {
            if (value)
            {
                if (_isHandlerSuspended == null)
                {
                    _isHandlerSuspended = new Dictionary<DependencyProperty, bool>(2);
                }

                _isHandlerSuspended[property] = true;
            }
            else
            {
                _isHandlerSuspended?.Remove(property);
            }
        }

        private void SetValueNoCallback(DependencyProperty property, object value)
        {
            SetIsHandlerSuspended(property, true);
            try
            {
                SetCurrentValue(property, value);
            }
            finally
            {
                SetIsHandlerSuspended(property, false);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValueNoCallback(TextProperty, _textBox.Text);
        }

        private bool ProcessTimePickerKey(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.System:
                {
                    switch (e.SystemKey)
                    {
                        case Key.Down:
                        {
                            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                            {
                                TogglePopUp();
                                return true;
                            }

                            break;
                        }
                    }

                    break;
                }

                case Key.Enter:
                {
                    SetSelectedTime();
                    return true;
                }
            }

            return false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessTimePickerKey(e) || e.Handled;
        }

        private void DropDownButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _disablePopupReopen = false;
        }

        private bool IsHandlerSuspended(DependencyProperty property)
        {
            return _isHandlerSuspended != null && _isHandlerSuspended.ContainsKey(property);
        }

        private void PopUp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Popup popup && !popup.StaysOpen)
            {
                if (_dropDownButton?.InputHitTest(e.GetPosition(_dropDownButton)) != null)
                {
                    _disablePopupReopen = true;
                }
            }
        }

        private void Clock_SelectedTimeChangedEvent(object sender, FunctionEventArgs<DateTime?> e) => SelectedTime = e.Info;

        private void Clock_Confirmed() => TogglePopUp();

        private void PopUp_Opened(object sender, EventArgs e)
        {
            if (!IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.TrueBox);
            }

            _clock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            OnClockOpened(new RoutedEventArgs());
        }

        private void PopUp_Closed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.FalseBox);
            }

            if (_clock.IsKeyboardFocusWithin)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }

            OnClockClosed(new RoutedEventArgs());
        }

        private void DropDownButton_Click(object sender, RoutedEventArgs e) => TogglePopUp();

        private void TogglePopUp()
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.FalseBox);
            }
            else
            {
                if (_disablePopupReopen)
                {
                    _disablePopupReopen = false;
                }
                else
                {
                    SetSelectedTime();
                    SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.TrueBox);
                }
            }
        }

        private void SafeSetText(string s)
        {
            if (string.Compare(Text, s, StringComparison.Ordinal) != 0)
            {
                SetCurrentValue(TextProperty, s);
            }
        }

        private DateTime? ParseText(string text)
        {
            try
            {
                return DateTime.Parse(text);
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private DateTime? SetTextBoxValue(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SafeSetText(s);
                return SelectedTime;
            }

            var d = ParseText(s);

            if (d != null)
            {
                SafeSetText(DateTimeToString((DateTime)d));
                return d;
            }

            if (SelectedTime != null)
            {
                var newtext = DateTimeToString((DateTime) SelectedTime);
                SafeSetText(newtext);
                return SelectedTime;
            }
            SafeSetText(DateTimeToString(DisplayTime));
            return DisplayTime;
        }

        private void SetSelectedTime()
        {
            if (_textBox != null)
            {
                if (!string.IsNullOrEmpty(_textBox.Text))
                {
                    var s = _textBox.Text;

                    if (SelectedTime != null)
                    {
                        var selectedTime = DateTimeToString(SelectedTime.Value);

                        if (string.Compare(selectedTime, s, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                    }

                    var d = SetTextBoxValue(s);
                    if (!SelectedTime.Equals(d))
                    {
                        SetCurrentValue(SelectedTimeProperty, d);
                        SetCurrentValue(DisplayTimeProperty, d);
                    }
                }
                else
                {
                    if (SelectedTime.HasValue)
                    {
                        SetCurrentValue(SelectedTimeProperty, null);
                    }
                }
            }
            else
            {
                var d = SetTextBoxValue(_defaultText);
                if (!SelectedTime.Equals(d))
                {
                    SetCurrentValue(SelectedTimeProperty, d);
                }
            }
        }

        private string DateTimeToString(DateTime d) => d.ToString(TimeFormat);

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var picker = (TimePicker)sender;
            if (!e.Handled && picker._textBox != null)
            {
                if (Equals(e.OriginalSource, picker))
                {
                    picker._textBox.Focus();
                    e.Handled = true;
                }
                else if (Equals(e.OriginalSource, picker._textBox))
                {
                    picker._textBox.SelectAll();
                    e.Handled = true;
                }
            }
        }

        #endregion Private Methods
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    /// <summary>
    ///     时间日期选择器
    /// </summary>
    [TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
    [TemplatePart(Name = ElementButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    public class DateTimePicker : Control, IDataInput
    {
        #region Constants

        private const string ElementRoot = "PART_Root";

        private const string ElementTextBox = "PART_TextBox";

        private const string ElementButton = "PART_Button";

        private const string ElementPopup = "PART_Popup";

        #endregion Constants

        #region Data

        private CalendarWithClock _calendarWithClock;

        private string _defaultText;

        private ButtonBase _dropDownButton;

        private Popup _popup;

        private bool _disablePopupReopen;

        private WatermarkTextBox _textBox;

        private IDictionary<DependencyProperty, bool> _isHandlerSuspended;

        private DateTime? _originalSelectedDateTime;

        #endregion Data

        #region Public Events

        public static readonly RoutedEvent SelectedDateTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Direct,
                typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(DateTimePicker));

        public event EventHandler<FunctionEventArgs<DateTime?>> SelectedDateTimeChanged
        {
            add => AddHandler(SelectedDateTimeChangedEvent, value);
            remove => RemoveHandler(SelectedDateTimeChangedEvent, value);
        }

        public event RoutedEventHandler PickerClosed;

        public event RoutedEventHandler PickerOpened;

        #endregion Public Events

        static DateTimePicker()
        {
            EventManager.RegisterClassHandler(typeof(DateTimePicker), GotFocusEvent, new RoutedEventHandler(OnGotFocus));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(false));
        }

        public DateTimePicker()
        {
            InitCalendarWithClock();
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                SetCurrentValue(SelectedDateTimeProperty, null);
                SetCurrentValue(TextProperty, "");
                _textBox.Text = string.Empty;
            }));
        }

        #region Public Properties

        public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(
            "DateTimeFormat", typeof(string), typeof(DateTimePicker), new PropertyMetadata("yyyy-MM-dd HH:mm:ss"));

        public string DateTimeFormat
        {
            get => (string) GetValue(DateTimeFormatProperty);
            set => SetValue(DateTimeFormatProperty, value);
        }

        public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register(
            "CalendarStyle", typeof(Style), typeof(DateTimePicker), new PropertyMetadata(default(Style)));

        public Style CalendarStyle
        {
            get => (Style) GetValue(CalendarStyleProperty);
            set => SetValue(CalendarStyleProperty, value);
        }

        public static readonly DependencyProperty DisplayDateTimeProperty = DependencyProperty.Register(
            "DisplayDateTime", typeof(DateTime), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceDisplayDateTime));

        private static object CoerceDisplayDateTime(DependencyObject d, object value)
        {
            var dp = (DateTimePicker)d;
            dp._calendarWithClock.DisplayDateTime = (DateTime)value;

            return dp._calendarWithClock.DisplayDateTime;
        }

        public DateTime DisplayDateTime
        {
            get => (DateTime) GetValue(DisplayDateTimeProperty);
            set => SetValue(DisplayDateTimeProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(DateTimePicker), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged, OnCoerceIsDropDownOpen));

        private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue) => d is DateTimePicker dp && !dp.IsEnabled ? false : baseValue;

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dp = d as DateTimePicker;

            var newValue = (bool)e.NewValue;
            if (dp?._popup != null && dp._popup.IsOpen != newValue)
            {
                dp._popup.IsOpen = newValue;
                if (newValue)
                {
                    dp._originalSelectedDateTime = dp.SelectedDateTime;

                    dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate
                    {
                        dp._calendarWithClock.Focus();
                    });
                }
            }
        }

        public bool IsDropDownOpen
        {
            get => (bool) GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTime?), typeof(DateTimePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateTimeChanged, CoerceSelectedDateTime));

        private static object CoerceSelectedDateTime(DependencyObject d, object value)
        {
            var dp = (DateTimePicker)d;
            dp._calendarWithClock.SelectedDateTime = (DateTime?)value;
            return dp._calendarWithClock.SelectedDateTime;
        }

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DateTimePicker dp)) return;

            if (dp.SelectedDateTime.HasValue)
            {
                var time = dp.SelectedDateTime.Value;
                dp.SetTextInternal(dp.DateTimeToString(time));
            }

            dp.RaiseEvent(new FunctionEventArgs<DateTime?>(SelectedDateTimeChangedEvent, dp)
            {
                Info = dp.SelectedDateTime
            });
        }

        public DateTime? SelectedDateTime
        {
            get => (DateTime?) GetValue(SelectedDateTimeProperty);
            set => SetValue(SelectedDateTimeProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(DateTimePicker), new FrameworkPropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker dp && !dp.IsHandlerSuspended(TextProperty))
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

                    dp.SetSelectedDateTime();
                }
                else
                {
                    dp.SetValueNoCallback(SelectedDateTimeProperty, null);
                }
            }
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Sets the local Text property without breaking bindings
        /// </summary>
        /// <param name="value"></param>
        private void SetTextInternal(string value)
        {
            SetCurrentValue(TextProperty, value);
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(DateTimePicker), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(DateTimePicker), new PropertyMetadata(default(TextType)));

        public TextType TextType
        {
            get => (TextType) GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool) GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            if (_popup != null)
            {
                _popup.PreviewMouseLeftButtonDown -= PopupPreviewMouseLeftButtonDown;
                _popup.Opened -= PopupOpened;
                _popup.Closed -= PopupClosed;
                _popup.Child = null;
            }

            if (_dropDownButton != null)
            {
                _dropDownButton.Click -= DropDownButton_Click;
                _dropDownButton.MouseLeave -= DropDownButton_MouseLeave;
            }

            if (_textBox != null)
            {
                _textBox.KeyDown -= TextBox_KeyDown;
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.LostFocus -= TextBox_LostFocus;
            }

            base.OnApplyTemplate();

            _popup = GetTemplateChild(ElementPopup) as Popup;
            _dropDownButton = GetTemplateChild(ElementButton) as Button;
            _textBox = GetTemplateChild(ElementTextBox) as WatermarkTextBox;

            CheckNull();

            _popup.PreviewMouseLeftButtonDown += PopupPreviewMouseLeftButtonDown;
            _popup.Opened += PopupOpened;
            _popup.Closed += PopupClosed;
            _popup.Child = _calendarWithClock;

            if (IsDropDownOpen)
            {
                _popup.IsOpen = true;
            }

            _dropDownButton.Click += DropDownButton_Click;
            _dropDownButton.MouseLeave += DropDownButton_MouseLeave;
            if (SelectedDateTime == null)
            {
                _textBox.Text = DateTime.Now.ToString(DateTimeFormat);
            }
            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.LostFocus += TextBox_LostFocus;

            if (SelectedDateTime == null)
            {
                if (!string.IsNullOrEmpty(_defaultText))
                {
                    _textBox.Text = _defaultText;
                    SetSelectedDateTime();
                }
            }
            else
            {
                _textBox.Text = DateTimeToString(SelectedDateTime.Value);
            }

            if (_originalSelectedDateTime == null)
            {
                _originalSelectedDateTime = DateTime.Now;
            }
            SetCurrentValue(DisplayDateTimeProperty, _originalSelectedDateTime);
        }

        public virtual bool VerifyData()
        {
            OperationResult<bool> result;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(Text);
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    result = OperationResult.Success();
                }
                else if (InfoElement.GetNecessary(this))
                {
                    result = OperationResult.Failed(Properties.Langs.Lang.IsNecessary);
                }
                else
                {
                    result = OperationResult.Success();
                }
            }

            IsError = !result.Data;
            ErrorStr = result.Message;
            return result.Data;
        }

        public override string ToString() => SelectedDateTime?.ToString(DateTimeFormat) ?? string.Empty;

        #endregion

        #region Protected Methods

        protected virtual void OnPickerClosed(RoutedEventArgs e)
        {
            var handler = PickerClosed;
            handler?.Invoke(this, e);
        }

        protected virtual void OnPickerOpened(RoutedEventArgs e)
        {
            var handler = PickerOpened;
            handler?.Invoke(this, e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void CheckNull()
        {
            if (_dropDownButton == null || _popup == null || _textBox == null)
                throw new Exception();
        }

        private void InitCalendarWithClock()
        {
            _calendarWithClock = new CalendarWithClock
            {
                ShowConfirmButton = true
            };
            _calendarWithClock.SelectedDateTimeChanged += CalendarWithClock_SelectedDateTimeChanged;
            _calendarWithClock.Confirmed += CalendarWithClock_Confirmed;
        }

        private void CalendarWithClock_Confirmed() => TogglePopup();

        private void CalendarWithClock_SelectedDateTimeChanged(object sender, FunctionEventArgs<DateTime?> e) => SelectedDateTime = e.Info;

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSelectedDateTime();
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
            VerifyData();
        }

        private bool ProcessDateTimePickerKey(KeyEventArgs e)
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
                                        TogglePopup();
                                        return true;
                                    }

                                    break;
                                }
                        }

                        break;
                    }

                case Key.Enter:
                    {
                        SetSelectedDateTime();
                        return true;
                    }
            }

            return false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessDateTimePickerKey(e) || e.Handled;
        }

        private void DropDownButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _disablePopupReopen = false;
        }

        private bool IsHandlerSuspended(DependencyProperty property)
        {
            return _isHandlerSuspended != null && _isHandlerSuspended.ContainsKey(property);
        }

        private void PopupPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Popup popup && !popup.StaysOpen)
            {
                if (_dropDownButton?.InputHitTest(e.GetPosition(_dropDownButton)) != null)
                {
                    _disablePopupReopen = true;
                }
            }
        }

        private void PopupOpened(object sender, EventArgs e)
        {
            if (!IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
            }

            _calendarWithClock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            OnPickerOpened(new RoutedEventArgs());
        }

        private void PopupClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
            }

            if (_calendarWithClock.IsKeyboardFocusWithin)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }

            OnPickerClosed(new RoutedEventArgs());
        }

        private void DropDownButton_Click(object sender, RoutedEventArgs e) => TogglePopup();

        private void TogglePopup()
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
            }
            else
            {
                if (_disablePopupReopen)
                {
                    _disablePopupReopen = false;
                }
                else
                {
                    SetSelectedDateTime();
                    SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
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
                return SelectedDateTime;
            }

            var d = ParseText(s);

            if (d != null)
            {
                SafeSetText(DateTimeToString((DateTime)d));
                return d;
            }

            if (SelectedDateTime != null)
            {
                var newtext = DateTimeToString(SelectedDateTime.Value);
                SafeSetText(newtext);
                return SelectedDateTime;
            }
            SafeSetText(DateTimeToString(DisplayDateTime));
            return DisplayDateTime;
        }

        private void SetSelectedDateTime()
        {
            if (_textBox != null)
            {
                if (!string.IsNullOrEmpty(_textBox.Text))
                {
                    var s = _textBox.Text;

                    if (SelectedDateTime != null)
                    {
                        if (SelectedDateTime != DisplayDateTime)
                        {
                            SetCurrentValue(DisplayDateTimeProperty, SelectedDateTime);
                        }

                        var selectedTime = DateTimeToString(SelectedDateTime.Value);

                        if (string.Compare(selectedTime, s, StringComparison.Ordinal) == 0)
                        {
                            return;
                        }
                    }

                    var d = SetTextBoxValue(s);
                    if (!SelectedDateTime.Equals(d))
                    {
                        SetCurrentValue(SelectedDateTimeProperty, d);
                        SetCurrentValue(DisplayDateTimeProperty, d);
                    }
                }
                else
                {
                    if (SelectedDateTime.HasValue)
                    {
                        SetCurrentValue(SelectedDateTimeProperty, null);
                    }
                }
            }
            else
            {
                var d = SetTextBoxValue(_defaultText);
                if (!SelectedDateTime.Equals(d))
                {
                    SetCurrentValue(SelectedDateTimeProperty, d);
                }
            }
        }

        private string DateTimeToString(DateTime d) => d.ToString(DateTimeFormat);

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var picker = (DateTimePicker)sender;
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

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     时间选择器
/// </summary>
[TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
[TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
[TemplatePart(Name = ElementButton, Type = typeof(Button))]
[TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
public class TimePicker : Control, IDataInput
{
    #region Constants

    private const string ElementRoot = "PART_Root";

    private const string ElementTextBox = "PART_TextBox";

    private const string ElementButton = "PART_Button";

    private const string ElementPopup = "PART_Popup";

    #endregion Constants

    #region Data

    private string _defaultText;

    private ButtonBase _dropDownButton;

    private Popup _popup;

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
        KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
    }

    public TimePicker()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(SelectedTimeProperty, null);
            SetCurrentValue(TextProperty, "");
            _textBox.Text = string.Empty;
        }));
    }

    #region Public Properties

    #region TimeFormat

    public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
        nameof(TimeFormat), typeof(string), typeof(TimePicker), new PropertyMetadata("HH:mm:ss"));

    public string TimeFormat
    {
        get => (string) GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion TimeFormat

    #region DisplayTime

    public DateTime DisplayTime
    {
        get => (DateTime) GetValue(DisplayTimeProperty);
        set => SetValue(DisplayTimeProperty, value);
    }

    public static readonly DependencyProperty DisplayTimeProperty =
        DependencyProperty.Register(
            nameof(DisplayTime),
            typeof(DateTime),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceDisplayTime));

    private static object CoerceDisplayTime(DependencyObject d, object value)
    {
        var dp = (TimePicker) d;
        dp.Clock.DisplayTime = (DateTime) value;
        return dp.Clock.DisplayTime;
    }

    #endregion DisplayTime

    #region IsDropDownOpen

    public bool IsDropDownOpen
    {
        get => (bool) GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty IsDropDownOpenProperty =
        DependencyProperty.Register(
            nameof(IsDropDownOpen),
            typeof(bool),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged, OnCoerceIsDropDownOpen));

    private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
    {
        return d is TimePicker { IsEnabled: false }
            ? false
            : baseValue;
    }

    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var dp = d as TimePicker;

        var newValue = (bool) e.NewValue;
        if (dp?._popup != null && dp._popup.IsOpen != newValue)
        {
            dp._popup.IsOpen = newValue;
            if (newValue)
            {
                dp._originalSelectedTime = dp.SelectedTime;

                dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action) delegate
                {
                    dp.Clock.Focus();
                });
            }
        }
    }

    #endregion IsDropDownOpen

    #region SelectedTime

    public DateTime? SelectedTime
    {
        get => (DateTime?) GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public static readonly DependencyProperty SelectedTimeProperty =
        DependencyProperty.Register(
            nameof(SelectedTime),
            typeof(DateTime?),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged, CoerceSelectedTime));

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TimePicker dp) return;

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
        var dp = (TimePicker) d;
        dp.Clock.SelectedTime = (DateTime?) value;
        return dp.Clock.SelectedTime;
    }

    #endregion SelectedDate

    #region Text

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
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

    public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

    public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
        nameof(IsError), typeof(bool), typeof(TimePicker), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsError
    {
        get => (bool) GetValue(IsErrorProperty);
        set => SetValue(IsErrorProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
        nameof(ErrorStr), typeof(string), typeof(TimePicker), new PropertyMetadata(default(string)));

    public string ErrorStr
    {
        get => (string) GetValue(ErrorStrProperty);
        set => SetValue(ErrorStrProperty, value);
    }

    public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
        nameof(TextType), typeof(TextType), typeof(TimePicker), new PropertyMetadata(default(TextType)));

    public TextType TextType
    {
        get => (TextType) GetValue(TextTypeProperty);
        set => SetValue(TextTypeProperty, value);
    }

    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
        nameof(ShowClearButton), typeof(bool), typeof(TimePicker), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowClearButton
    {
        get => (bool) GetValue(ShowClearButtonProperty);
        set => SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        TextBoxBase.SelectionBrushProperty.AddOwner(typeof(TimePicker));

    public Brush SelectionBrush
    {
        get => (Brush) GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

    public static readonly DependencyProperty SelectionTextBrushProperty =
        TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(TimePicker));

    public Brush SelectionTextBrush
    {
        get => (Brush) GetValue(SelectionTextBrushProperty);
        set => SetValue(SelectionTextBrushProperty, value);
    }

#endif

    public static readonly DependencyProperty SelectionOpacityProperty =
        TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(TimePicker));

    public double SelectionOpacity
    {
        get => (double) GetValue(SelectionOpacityProperty);
        set => SetValue(SelectionOpacityProperty, value);
    }

    public static readonly DependencyProperty CaretBrushProperty =
        TextBoxBase.CaretBrushProperty.AddOwner(typeof(TimePicker));

    public Brush CaretBrush
    {
        get => (Brush) GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

    public static readonly DependencyProperty ClockProperty = DependencyProperty.Register(
        nameof(Clock), typeof(ClockBase), typeof(TimePicker), new FrameworkPropertyMetadata(default(Clock), FrameworkPropertyMetadataOptions.NotDataBindable, OnClockChanged));

    private static void OnClockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (TimePicker) d;

        if (e.OldValue is ClockBase oldClock)
        {
            oldClock.SelectedTimeChanged -= ctl.Clock_SelectedTimeChanged;
            oldClock.Confirmed -= ctl.Clock_Confirmed;
            ctl.SetPopupChild(null);
        }

        if (e.NewValue is ClockBase newClock)
        {
            newClock.ShowConfirmButton = true;
            newClock.SelectedTimeChanged += ctl.Clock_SelectedTimeChanged;
            newClock.Confirmed += ctl.Clock_Confirmed;
            ctl.SetPopupChild(newClock);
        }
    }

    public ClockBase Clock
    {
        get => (ClockBase) GetValue(ClockProperty);
        set => SetValue(ClockProperty, value);
    }

    #endregion Public Properties

    #region Public Methods

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

        var isError = !result.Data;
        if (isError)
        {
            SetCurrentValue(IsErrorProperty, ValueBoxes.TrueBox);
            SetCurrentValue(ErrorStrProperty, result.Message);
        }
        else
        {
            isError = Validation.GetHasError(this);
            if (isError)
            {
                SetCurrentValue(ErrorStrProperty, Validation.GetErrors(this)[0].ErrorContent?.ToString());
            }
            else
            {
                SetCurrentValue(IsErrorProperty, ValueBoxes.FalseBox);
                SetCurrentValue(ErrorStrProperty, default(string));
            }
        }
        return !isError;
    }

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
        _popup.Child = Clock;

        _dropDownButton.Click += DropDownButton_Click;
        _dropDownButton.MouseLeave += DropDownButton_MouseLeave;

        var selectedTime = SelectedTime;

        if (_textBox != null)
        {
            if (selectedTime == null)
            {
                _textBox.Text = DateTime.Now.ToString(TimeFormat);
            }

            _textBox.SetBinding(SelectionBrushProperty, new Binding(SelectionBrushProperty.Name) { Source = this });
#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)
            _textBox.SetBinding(SelectionTextBrushProperty, new Binding(SelectionTextBrushProperty.Name) { Source = this });
#endif
            _textBox.SetBinding(SelectionOpacityProperty, new Binding(SelectionOpacityProperty.Name) { Source = this });
            _textBox.SetBinding(CaretBrushProperty, new Binding(CaretBrushProperty.Name) { Source = this });

            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.LostFocus += TextBox_LostFocus;

            if (selectedTime == null)
            {
                if (!string.IsNullOrEmpty(_defaultText))
                {
                    _textBox.Text = _defaultText;
                    SetSelectedTime();
                }
            }
            else
            {
                _textBox.Text = DateTimeToString(selectedTime.Value);
            }
        }

        EnsureClock();

        if (selectedTime is null)
        {
            _originalSelectedTime ??= DateTime.Now;
            SetCurrentValue(DisplayTimeProperty, _originalSelectedTime);
        }
        else
        {
            SetCurrentValue(DisplayTimeProperty, selectedTime);
        }
    }

    public override string ToString() => SelectedTime?.ToString(TimeFormat) ?? string.Empty;

    #endregion

    #region Protected Methods

    protected virtual void OnClockClosed(RoutedEventArgs e)
    {
        var handler = ClockClosed;
        handler?.Invoke(this, e);
        Clock?.OnClockClosed();
    }

    protected virtual void OnClockOpened(RoutedEventArgs e)
    {
        var handler = ClockOpened;
        handler?.Invoke(this, e);
        Clock?.OnClockOpened();
    }

    #endregion Protected Methods

    #region Private Methods

    private void CheckNull()
    {
        if (_dropDownButton == null || _popup == null || _textBox == null)
            throw new Exception();
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) => SetSelectedTime();

    private void SetIsHandlerSuspended(DependencyProperty property, bool value)
    {
        if (value)
        {
            _isHandlerSuspended ??= new Dictionary<DependencyProperty, bool>(2);
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

    private void PopupPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Popup { StaysOpen: false })
        {
            if (_dropDownButton?.InputHitTest(e.GetPosition(_dropDownButton)) != null)
            {
                _disablePopupReopen = true;
            }
        }
    }

    private void Clock_SelectedTimeChanged(object sender, FunctionEventArgs<DateTime?> e) => SelectedTime = e.Info;

    private void Clock_Confirmed() => TogglePopup();

    private void PopupOpened(object sender, EventArgs e)
    {
        if (!IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
        }

        Clock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

        OnClockOpened(new RoutedEventArgs());
    }

    private void PopupClosed(object sender, EventArgs e)
    {
        if (IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
        }

        if (Clock.IsKeyboardFocusWithin)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        OnClockClosed(new RoutedEventArgs());
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
                SetSelectedTime();
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
            return SelectedTime;
        }

        var d = ParseText(s);

        if (d != null)
        {
            SafeSetText(DateTimeToString((DateTime) d));
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
        var picker = (TimePicker) sender;
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

    private void EnsureClock()
    {
        if (Clock is not null)
        {
            return;
        }

        SetCurrentValue(ClockProperty, new Clock());
        SetPopupChild(Clock);
    }

    private void SetPopupChild(UIElement element)
    {
        if (_popup is not null)
        {
            _popup.Child = Clock;
        }
    }

    #endregion Private Methods
}

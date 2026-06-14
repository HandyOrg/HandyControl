using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace HandyControl.Controls;

[TemplatePart(ElementRoot, typeof(Grid))]
[TemplatePart(ElementTextBox, typeof(TextBox))]
[TemplatePart(ElementButton, typeof(Button))]
[TemplatePart(ElementPopup, typeof(Popup))]
[PseudoClasses(":dropdownopen")]
public class TimePicker : TemplatedControl
{
    private const string ElementRoot = "PART_Root";
    private const string ElementTextBox = "PART_TextBox";
    private const string ElementButton = "PART_Button";
    private const string ElementPopup = "PART_Popup";

    private string? _defaultText;
    private Button? _dropDownButton;
    private Popup? _popup;
    private TextBox? _textBox;
    private DateTime? _originalSelectedTime;
    private bool _isHandlerSuspendedText;

    public event EventHandler<DateTime?>? SelectedTimeChanged;
    public event EventHandler? ClockClosed;
    public event EventHandler? ClockOpened;

    public static readonly StyledProperty<string> TimeFormatProperty =
        AvaloniaProperty.Register<TimePicker, string>(nameof(TimeFormat), "HH:mm:ss");

    public string TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    public static readonly StyledProperty<DateTime> DisplayTimeProperty =
        AvaloniaProperty.Register<TimePicker, DateTime>(nameof(DisplayTime), DateTime.Now,
            defaultBindingMode: BindingMode.TwoWay);

    public DateTime DisplayTime
    {
        get => GetValue(DisplayTimeProperty);
        set => SetValue(DisplayTimeProperty, value);
    }

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<TimePicker, bool>(nameof(IsDropDownOpen),
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly StyledProperty<DateTime?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, DateTime?>(nameof(SelectedTime),
            defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<TimePicker, string>(nameof(Text), string.Empty);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<TimePicker, string?>(nameof(Watermark));

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<ClockBase?> ClockProperty =
        AvaloniaProperty.Register<TimePicker, ClockBase?>(nameof(Clock));

    public ClockBase? Clock
    {
        get => GetValue(ClockProperty);
        set => SetValue(ClockProperty, value);
    }

    static TimePicker()
    {
        IsDropDownOpenProperty.Changed.AddClassHandler<TimePicker>((o, e) => o.OnIsDropDownOpenChanged(e));
        SelectedTimeProperty.Changed.AddClassHandler<TimePicker>((o, e) => o.OnSelectedTimePropertyChanged(e));
        TextProperty.Changed.AddClassHandler<TimePicker>((o, e) => o.OnTextPropertyChanged(e));
        ClockProperty.Changed.AddClassHandler<TimePicker>((o, e) => o.OnClockChanged(e));
        DisplayTimeProperty.Changed.AddClassHandler<TimePicker>((o, e) => o.OnDisplayTimePropertyChanged(e));
    }

    public TimePicker()
    {
        UpdatePseudoClasses();
    }

    private void OnIsDropDownOpenChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = (bool) (e.NewValue ?? false);
        if (!IsEnabled) newValue = false;

        if (_popup != null && _popup.IsOpen != newValue)
        {
            _popup.IsOpen = newValue;
            if (newValue)
            {
                _originalSelectedTime = SelectedTime;
                Clock?.Focus();
            }
        }
        UpdatePseudoClasses();
    }

    private void OnSelectedTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var v = (DateTime?) e.NewValue;
        if (Clock != null) Clock.SelectedTime = v;
        if (v.HasValue)
        {
            SetTextInternal(DateTimeToString(v.Value));
        }
        else
        {
            SetTextInternal(string.Empty);
        }
        SelectedTimeChanged?.Invoke(this, v);
    }

    private void OnDisplayTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (Clock != null && e.NewValue is DateTime dt)
        {
            Clock.DisplayTime = dt;
        }
    }

    private void OnTextPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_isHandlerSuspendedText) return;
        if (e.NewValue is string newValue)
        {
            if (_textBox != null)
            {
                _textBox.Text = newValue;
            }
            else
            {
                _defaultText = newValue;
            }
            SetSelectedTime();
        }
        else
        {
            SetValueNoCallback(SelectedTimeProperty, null);
        }
    }

    private void OnClockChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ClockBase oldClock)
        {
            oldClock.SelectedTimeChanged -= Clock_SelectedTimeChanged;
            oldClock.Confirmed -= Clock_Confirmed;
            SetPopupChild(null);
        }

        if (e.NewValue is ClockBase newClock)
        {
            newClock.ShowConfirmButton = true;
            newClock.SelectedTimeChanged += Clock_SelectedTimeChanged;
            newClock.Confirmed += Clock_Confirmed;
            SetPopupChild(newClock);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_popup != null)
        {
            _popup.Opened -= PopupOpened;
            _popup.Closed -= PopupClosed;
            _popup.Child = null;
        }
        if (_dropDownButton != null)
        {
            _dropDownButton.Click -= DropDownButton_Click;
        }
        if (_textBox != null)
        {
            _textBox.KeyDown -= TextBox_KeyDown;
            _textBox.TextChanged -= TextBox_TextChanged;
            _textBox.LostFocus -= TextBox_LostFocus;
        }

        base.OnApplyTemplate(e);

        _popup = e.NameScope.Find<Popup>(ElementPopup);
        _dropDownButton = e.NameScope.Find<Button>(ElementButton);
        _textBox = e.NameScope.Find<TextBox>(ElementTextBox);

        if (_popup != null)
        {
            _popup.Opened += PopupOpened;
            _popup.Closed += PopupClosed;
            if (Clock != null) _popup.Child = Clock;
        }
        if (_dropDownButton != null)
        {
            _dropDownButton.Click += DropDownButton_Click;
        }
        if (_textBox != null)
        {
            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.LostFocus += TextBox_LostFocus;

            var selectedTime = SelectedTime;
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

        var sel = SelectedTime;
        if (sel is null)
        {
            _originalSelectedTime ??= DateTime.Now;
            SetCurrentValue(DisplayTimeProperty, _originalSelectedTime.Value);
        }
        else
        {
            SetCurrentValue(DisplayTimeProperty, sel.Value);
        }
    }

    private void SetTextInternal(string value) => SetCurrentValue(TextProperty, value);

    private void SetValueNoCallback(AvaloniaProperty property, object? value)
    {
        _isHandlerSuspendedText = true;
        try { SetCurrentValue(property, value); }
        finally { _isHandlerSuspendedText = false; }
    }

    private void TextBox_LostFocus(object? sender, RoutedEventArgs e) => SetSelectedTime();

    private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_textBox == null) return;
        SetValueNoCallback(TextProperty, _textBox.Text ?? string.Empty);
    }

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SetSelectedTime();
            e.Handled = true;
        }
    }

    private void Clock_SelectedTimeChanged(object? sender, DateTime? e) => SelectedTime = e;

    private void Clock_Confirmed() => TogglePopup();

    private void PopupOpened(object? sender, EventArgs e)
    {
        if (!IsDropDownOpen) SetCurrentValue(IsDropDownOpenProperty, true);
        Clock?.OnClockOpened();
        ClockOpened?.Invoke(this, EventArgs.Empty);
    }

    private void PopupClosed(object? sender, EventArgs e)
    {
        if (IsDropDownOpen) SetCurrentValue(IsDropDownOpenProperty, false);
        Clock?.OnClockClosed();
        ClockClosed?.Invoke(this, EventArgs.Empty);
    }

    private void DropDownButton_Click(object? sender, RoutedEventArgs e) => TogglePopup();

    private void TogglePopup()
    {
        if (IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
        else
        {
            SetSelectedTime();
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
    }

    private DateTime? ParseText(string text)
    {
        if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var d))
            return d;
        return null;
    }

    private void SetSelectedTime()
    {
        var s = _textBox?.Text ?? _defaultText ?? string.Empty;
        if (!string.IsNullOrEmpty(s))
        {
            if (SelectedTime != null)
            {
                var existing = DateTimeToString(SelectedTime.Value);
                if (string.Equals(existing, s, StringComparison.Ordinal)) return;
            }

            var d = ParseText(s);
            if (d != null)
            {
                SetCurrentValue(TextProperty, DateTimeToString(d.Value));
                if (!Equals(SelectedTime, d))
                {
                    SetCurrentValue(SelectedTimeProperty, d);
                    SetCurrentValue(DisplayTimeProperty, d.Value);
                }
            }
            else if (SelectedTime != null)
            {
                SetCurrentValue(TextProperty, DateTimeToString(SelectedTime.Value));
            }
            else
            {
                SetCurrentValue(TextProperty, DateTimeToString(DisplayTime));
            }
        }
        else
        {
            if (SelectedTime.HasValue)
            {
                SetCurrentValue(SelectedTimeProperty, null);
            }
            else
            {
                SetTextInternal(string.Empty);
            }
        }
    }

    private string DateTimeToString(DateTime d) => d.ToString(TimeFormat, CultureInfo.CurrentCulture);

    private void EnsureClock()
    {
        if (Clock is not null) return;
        SetCurrentValue(ClockProperty, new Clock());
    }

    private void SetPopupChild(Control? element)
    {
        if (_popup is not null)
        {
            _popup.Child = element ?? Clock;
        }
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":dropdownopen", IsDropDownOpen);
    }

    public override string ToString() => SelectedTime?.ToString(TimeFormat) ?? string.Empty;

    public void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, null);
        SetCurrentValue(TextProperty, string.Empty);
    }
}

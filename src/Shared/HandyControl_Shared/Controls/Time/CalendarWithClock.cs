using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementButtonConfirm, Type = typeof(Button))]
[TemplatePart(Name = ElementClockPresenter, Type = typeof(ContentPresenter))]
[TemplatePart(Name = ElementCalendarPresenter, Type = typeof(ContentPresenter))]
public class CalendarWithClock : Control
{
    #region Constants

    private const string ElementButtonConfirm = "PART_ButtonConfirm";

    private const string ElementClockPresenter = "PART_ClockPresenter";

    private const string ElementCalendarPresenter = "PART_CalendarPresenter";

    #endregion Constants

    #region Data

    private ContentPresenter _clockPresenter;

    private ContentPresenter _calendarPresenter;

    private Clock _clock;

    private Calendar _calendar;

    private Button _buttonConfirm;

    private bool _isLoaded;

    private IDictionary<DependencyProperty, bool> _isHandlerSuspended;

    #endregion Data

    #region Public Events

    public static readonly RoutedEvent SelectedDateTimeChangedEvent =
        EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Direct,
            typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(CalendarWithClock));

    public event EventHandler<FunctionEventArgs<DateTime?>> SelectedDateTimeChanged
    {
        add => AddHandler(SelectedDateTimeChangedEvent, value);
        remove => RemoveHandler(SelectedDateTimeChangedEvent, value);
    }

    public event EventHandler<FunctionEventArgs<DateTime>> DisplayDateTimeChanged;

    public event Action Confirmed;

    #endregion Public Events

    public CalendarWithClock()
    {
        InitCalendarAndClock();
        Loaded += (s, e) =>
        {
            if (_isLoaded) return;
            _isLoaded = true;
            DisplayDateTime = SelectedDateTime ?? DateTime.Now;
        };
    }

    #region Public Properties

    public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(
        nameof(DateTimeFormat), typeof(string), typeof(CalendarWithClock), new PropertyMetadata("yyyy-MM-dd HH:mm:ss"));

    public string DateTimeFormat
    {
        get => (string) GetValue(DateTimeFormatProperty);
        set => SetValue(DateTimeFormatProperty, value);
    }

    public static readonly DependencyProperty ShowConfirmButtonProperty = DependencyProperty.Register(
        nameof(ShowConfirmButton), typeof(bool), typeof(CalendarWithClock), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowConfirmButton
    {
        get => (bool) GetValue(ShowConfirmButtonProperty);
        set => SetValue(ShowConfirmButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
        nameof(SelectedDateTime), typeof(DateTime?), typeof(CalendarWithClock), new PropertyMetadata(default(DateTime?), OnSelectedDateTimeChanged));

    private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (CalendarWithClock) d;
        var v = (DateTime?) e.NewValue;
        ctl.OnSelectedDateTimeChanged(new FunctionEventArgs<DateTime?>(SelectedDateTimeChangedEvent, ctl)
        {
            Info = v
        });
    }

    public DateTime? SelectedDateTime
    {
        get => (DateTime?) GetValue(SelectedDateTimeProperty);
        set => SetValue(SelectedDateTimeProperty, value);
    }

    public static readonly DependencyProperty DisplayDateTimeProperty = DependencyProperty.Register(
        nameof(DisplayDateTime), typeof(DateTime), typeof(CalendarWithClock), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDisplayDateTimeChanged));

    private static void OnDisplayDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (CalendarWithClock) d;
        if (ctl.IsHandlerSuspended(DisplayDateTimeProperty)) return;
        var v = (DateTime) e.NewValue;
        ctl._clock.SelectedTime = v;
        ctl._calendar.SelectedDate = v;
        ctl._calendar.DisplayDate = v;
        ctl.OnDisplayDateTimeChanged(new FunctionEventArgs<DateTime>(v));
    }

    public DateTime DisplayDateTime
    {
        get => (DateTime) GetValue(DisplayDateTimeProperty);
        set => SetValue(DisplayDateTimeProperty, value);
    }

    #endregion

    #region Public Methods

    public override void OnApplyTemplate()
    {
        if (_buttonConfirm != null)
        {
            _buttonConfirm.Click -= ButtonConfirm_OnClick;
        }

        base.OnApplyTemplate();

        _buttonConfirm = GetTemplateChild(ElementButtonConfirm) as Button;
        _clockPresenter = GetTemplateChild(ElementClockPresenter) as ContentPresenter;
        _calendarPresenter = GetTemplateChild(ElementCalendarPresenter) as ContentPresenter;

        CheckNull();

        _clockPresenter.Content = _clock;
        _calendarPresenter.Content = _calendar;

        _buttonConfirm.Click += ButtonConfirm_OnClick;
    }

    #endregion

    #region Protected Methods

    protected virtual void OnSelectedDateTimeChanged(FunctionEventArgs<DateTime?> e) => RaiseEvent(e);

    protected virtual void OnDisplayDateTimeChanged(FunctionEventArgs<DateTime> e)
    {
        var handler = DisplayDateTimeChanged;
        handler?.Invoke(this, e);
    }

    #endregion Protected Methods

    #region Private Methods

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

    private bool IsHandlerSuspended(DependencyProperty property)
    {
        return _isHandlerSuspended != null && _isHandlerSuspended.ContainsKey(property);
    }

    private void CheckNull()
    {
        if (_buttonConfirm == null || _clockPresenter == null || _calendarPresenter == null) throw new Exception();
    }

    private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        SelectedDateTime = DisplayDateTime;
        Confirmed?.Invoke();
    }

    private void InitCalendarAndClock()
    {
        _clock = new Clock
        {
            BorderThickness = new Thickness(),
            Background = Brushes.Transparent
        };
        TitleElement.SetBackground(_clock, Brushes.Transparent);
        _clock.DisplayTimeChanged += Clock_DisplayTimeChanged;

        _calendar = new Calendar
        {
            BorderThickness = new Thickness(),
            Background = Brushes.Transparent,
            Focusable = false
        };
        TitleElement.SetBackground(_calendar, Brushes.Transparent);
        _calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
    }

    private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        Mouse.Capture(null);
        UpdateDisplayTime();
    }

    private void Clock_DisplayTimeChanged(object sender, FunctionEventArgs<DateTime> e) => UpdateDisplayTime();

    private void UpdateDisplayTime()
    {
        if (_calendar.SelectedDate != null)
        {
            var date = _calendar.SelectedDate.Value;
            var time = _clock.DisplayTime;

            var result = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
            SetValueNoCallback(DisplayDateTimeProperty, result);
        }
    }

    #endregion
}

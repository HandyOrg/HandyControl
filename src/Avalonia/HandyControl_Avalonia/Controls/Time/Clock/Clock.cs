using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;

namespace HandyControl.Controls;

[TemplatePart(ElementButtonAm, typeof(RadioButton))]
[TemplatePart(ElementButtonPm, typeof(RadioButton))]
[TemplatePart(ElementCanvas, typeof(Canvas))]
[TemplatePart(ElementBorderTitle, typeof(Border))]
[TemplatePart(ElementBorderClock, typeof(Border))]
[TemplatePart(ElementPanelNum, typeof(CirclePanel))]
[TemplatePart(ElementTimeStr, typeof(TextBlock))]
public class Clock : ClockBase
{
    private const string ElementButtonAm = "PART_ButtonAm";
    private const string ElementButtonPm = "PART_ButtonPm";
    private const string ElementCanvas = "PART_Canvas";
    private const string ElementBorderTitle = "PART_BorderTitle";
    private const string ElementBorderClock = "PART_BorderClock";
    private const string ElementPanelNum = "PART_PanelNum";
    private const string ElementTimeStr = "PART_TimeStr";

    private RadioButton? _buttonAm;
    private RadioButton? _buttonPm;
    private Canvas? _canvas;
    private Border? _borderTitle;
    private Border? _borderClock;
    private ClockRadioButton? _currentButton;
    private RotateTransform? _rotateTransformClock;
    private CirclePanel? _circlePanel;
    private List<ClockRadioButton>? _radioButtonList;
    private TextBlock? _blockTime;
    private int _secValue;

    public static readonly StyledProperty<ControlTheme?> ClockRadioButtonThemeProperty =
        AvaloniaProperty.Register<Clock, ControlTheme?>(nameof(ClockRadioButtonTheme));

    public ControlTheme? ClockRadioButtonTheme
    {
        get => GetValue(ClockRadioButtonThemeProperty);
        set => SetValue(ClockRadioButtonThemeProperty, value);
    }

    private int SecValue
    {
        get => _secValue;
        set
        {
            if (value < 0) _secValue = 59;
            else if (value > 59) _secValue = 0;
            else _secValue = value;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        AppliedTemplate = false;
        if (_buttonAm != null) _buttonAm.Click -= ButtonAm_OnClick;
        if (_buttonPm != null) _buttonPm.Click -= ButtonPm_OnClick;
        if (ButtonConfirm != null) ButtonConfirm.Click -= ButtonConfirm_OnClick;
        if (_borderTitle != null) _borderTitle.PointerWheelChanged -= BorderTitle_OnPointerWheelChanged;
        if (_canvas != null)
        {
            _canvas.PointerWheelChanged -= Canvas_OnPointerWheelChanged;
            _canvas.RemoveHandler(Button.ClickEvent, Canvas_OnClick);
            _canvas.PointerMoved -= Canvas_OnPointerMoved;
        }

        base.OnApplyTemplate(e);

        _buttonAm = e.NameScope.Find<RadioButton>(ElementButtonAm);
        _buttonPm = e.NameScope.Find<RadioButton>(ElementButtonPm);
        ButtonConfirm = e.NameScope.Find<Button>(ElementButtonConfirm);
        _borderTitle = e.NameScope.Find<Border>(ElementBorderTitle);
        _canvas = e.NameScope.Find<Canvas>(ElementCanvas);
        _borderClock = e.NameScope.Find<Border>(ElementBorderClock);
        _circlePanel = e.NameScope.Find<CirclePanel>(ElementPanelNum);
        _blockTime = e.NameScope.Find<TextBlock>(ElementTimeStr);

        if (!CheckNull()) return;

        _buttonAm!.Click += ButtonAm_OnClick;
        _buttonPm!.Click += ButtonPm_OnClick;
        ButtonConfirm!.Click += ButtonConfirm_OnClick;
        _borderTitle!.PointerWheelChanged += BorderTitle_OnPointerWheelChanged;

        _canvas!.PointerWheelChanged += Canvas_OnPointerWheelChanged;
        _canvas.AddHandler(Button.ClickEvent, Canvas_OnClick);
        _canvas.PointerMoved += Canvas_OnPointerMoved;

        _rotateTransformClock = new RotateTransform();
        _borderClock!.RenderTransform = _rotateTransformClock;
        _borderClock.RenderTransformOrigin = new RelativePoint(0.5, 1, RelativeUnit.Relative);

        _radioButtonList = new List<ClockRadioButton>();
        _circlePanel!.Children.Clear();
        for (var i = 0; i < 12; i++)
        {
            var num = i + 1;
            var button = new ClockRadioButton
            {
                Num = num,
                Content = num
            };
            if (ClockRadioButtonTheme != null)
            {
                button.Theme = ClockRadioButtonTheme;
            }
            _radioButtonList.Add(button);
            _circlePanel.Children.Add(button);
        }

        AppliedTemplate = true;
        if (SelectedTime.HasValue)
        {
            Update(SelectedTime.Value);
        }
        else
        {
            DisplayTime = DateTime.Now;
            Update(DisplayTime);
        }
    }

    private bool CheckNull() =>
        _buttonAm != null && _buttonPm != null && ButtonConfirm != null && _canvas != null &&
        _borderTitle != null && _borderClock != null && _circlePanel != null && _blockTime != null;

    private void BorderTitle_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (e.Delta.Y < 0) SecValue--;
        else SecValue++;
        UpdateInternal();
        e.Handled = true;
    }

    private void Canvas_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (_rotateTransformClock == null) return;
        var value = (int) _rotateTransformClock.Angle;
        if (e.Delta.Y < 0) value += 6;
        else value -= 6;
        if (value < 0) value += 360;
        _rotateTransformClock.Angle = value;
        UpdateInternal();
        e.Handled = true;
    }

    private void Canvas_OnClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is ClockRadioButton crb)
        {
            _currentButton = crb;
            UpdateInternal();
        }
    }

    private void Canvas_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_canvas == null || _rotateTransformClock == null) return;
        var props = e.GetCurrentPoint(_canvas).Properties;
        if (!props.IsLeftButtonPressed) return;

        var p = e.GetPosition(_canvas);
        var dx = p.X - 85;
        var dy = p.Y - 85;
        var angle = Math.Atan2(dy, dx) * 180.0 / Math.PI + 90;
        if (angle < 0) angle += 360;
        angle -= angle % 6;
        _rotateTransformClock.Angle = angle;
        UpdateInternal();
    }

    private void UpdateInternal()
    {
        if (!AppliedTemplate || _currentButton == null || _buttonAm == null || _buttonPm == null) return;
        var hValue = _currentButton.Num;
        if (_buttonPm.IsChecked == true)
        {
            hValue += 12;
            if (hValue == 24) hValue = 12;
        }
        else if (hValue == 12)
        {
            hValue = 0;
        }
        if (hValue == 12 && _buttonAm.IsChecked == true)
        {
            _buttonPm.IsChecked = true;
            _buttonAm.IsChecked = false;
        }

        if (_blockTime != null)
        {
            DisplayTime = GetDisplayTime();
            _blockTime.Text = DisplayTime.ToString(TimeFormat);
        }
    }

    internal override void Update(DateTime time)
    {
        if (!AppliedTemplate || _buttonAm == null || _buttonPm == null ||
            _rotateTransformClock == null || _radioButtonList == null) return;

        var h = time.Hour;
        var m = time.Minute;

        if (h >= 12)
        {
            _buttonPm.IsChecked = true;
            _buttonAm.IsChecked = false;
        }
        else
        {
            _buttonPm.IsChecked = false;
            _buttonAm.IsChecked = true;
        }

        _rotateTransformClock.Angle = m * 6;

        var hRest = h % 12;
        if (hRest == 0) hRest = 12;
        var ctl = _radioButtonList[hRest - 1];
        ctl.IsChecked = true;
        _currentButton = ctl;

        _secValue = time.Second;
        UpdateInternal();
    }

    private DateTime GetDisplayTime()
    {
        if (_currentButton == null || _buttonAm == null || _buttonPm == null || _rotateTransformClock == null)
            return DateTime.Now;

        var hValue = _currentButton.Num;
        if (_buttonPm.IsChecked == true)
        {
            hValue += 12;
            if (hValue == 24) hValue = 12;
        }
        else if (hValue == 12)
        {
            hValue = 0;
        }
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, now.Day, hValue,
            (int) Math.Abs(_rotateTransformClock.Angle) % 360 / 6, _secValue);
    }

    private void ButtonAm_OnClick(object? sender, RoutedEventArgs e) => UpdateInternal();
    private void ButtonPm_OnClick(object? sender, RoutedEventArgs e) => UpdateInternal();
}

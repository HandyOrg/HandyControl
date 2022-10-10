using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Tools;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementButtonAm, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementButtonPm, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = ElementBorderTitle, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderClock, Type = typeof(Border))]
[TemplatePart(Name = ElementPanelNum, Type = typeof(CirclePanel))]
[TemplatePart(Name = ElementTimeStr, Type = typeof(TextBlock))]
public class Clock : ClockBase
{
    #region Constants

    private const string ElementButtonAm = "PART_ButtonAm";
    private const string ElementButtonPm = "PART_ButtonPm";
    private const string ElementCanvas = "PART_Canvas";
    private const string ElementBorderTitle = "PART_BorderTitle";
    private const string ElementBorderClock = "PART_BorderClock";
    private const string ElementPanelNum = "PART_PanelNum";
    private const string ElementTimeStr = "PART_TimeStr";

    #endregion Constants

    #region Data

    private RadioButton _buttonAm;

    private RadioButton _buttonPm;

    private Canvas _canvas;

    private Border _borderTitle;

    private Border _borderClock;

    private ClockRadioButton _currentButton;

    private RotateTransform _rotateTransformClock;

    private CirclePanel _circlePanel;

    private List<ClockRadioButton> _radioButtonList;

    private TextBlock _blockTime;

    private int _secValue;

    #endregion Data

    #region Public Properties

    public static readonly DependencyProperty ClockRadioButtonStyleProperty = DependencyProperty.Register(
        nameof(ClockRadioButtonStyle), typeof(Style), typeof(Clock), new PropertyMetadata(default(Style)));

    public Style ClockRadioButtonStyle
    {
        get => (Style) GetValue(ClockRadioButtonStyleProperty);
        set => SetValue(ClockRadioButtonStyleProperty, value);
    }

    private int SecValue
    {
        get => _secValue;
        set
        {
            if (value < 0)
            {
                _secValue = 59;
            }
            else if (value > 59)
            {
                _secValue = 0;
            }
            else
            {
                _secValue = value;
            }
        }
    }

    #endregion Public Properties

    #region Public Methods

    public override void OnApplyTemplate()
    {
        AppliedTemplate = false;
        if (_buttonAm != null)
        {
            _buttonAm.Click -= ButtonAm_OnClick;
        }

        if (_buttonPm != null)
        {
            _buttonPm.Click -= ButtonPm_OnClick;
        }

        if (ButtonConfirm != null)
        {
            ButtonConfirm.Click -= ButtonConfirm_OnClick;
        }

        if (_borderTitle != null)
        {
            _borderTitle.MouseWheel -= BorderTitle_OnMouseWheel;
        }

        if (_canvas != null)
        {
            _canvas.MouseWheel -= Canvas_OnMouseWheel;
            _canvas.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Canvas_OnClick));
            _canvas.MouseMove -= Canvas_OnMouseMove;
        }

        base.OnApplyTemplate();

        _buttonAm = GetTemplateChild(ElementButtonAm) as RadioButton;
        _buttonPm = GetTemplateChild(ElementButtonPm) as RadioButton;
        ButtonConfirm = GetTemplateChild(ElementButtonConfirm) as Button;
        _borderTitle = GetTemplateChild(ElementBorderTitle) as Border;
        _canvas = GetTemplateChild(ElementCanvas) as Canvas;
        _borderClock = GetTemplateChild(ElementBorderClock) as Border;
        _circlePanel = GetTemplateChild(ElementPanelNum) as CirclePanel;
        _blockTime = GetTemplateChild(ElementTimeStr) as TextBlock;

        if (!CheckNull()) return;

        _buttonAm.Click += ButtonAm_OnClick;
        _buttonPm.Click += ButtonPm_OnClick;
        ButtonConfirm.Click += ButtonConfirm_OnClick;
        _borderTitle.MouseWheel += BorderTitle_OnMouseWheel;

        _canvas.MouseWheel += Canvas_OnMouseWheel;
        _canvas.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Canvas_OnClick));
        _canvas.MouseMove += Canvas_OnMouseMove;

        _rotateTransformClock = new RotateTransform();
        _borderClock.RenderTransform = _rotateTransformClock;

        _radioButtonList = new List<ClockRadioButton>();
        for (var i = 0; i < 12; i++)
        {
            var num = i + 1;
            var button = new ClockRadioButton
            {
                Num = num,
                Content = num
            };
            button.SetBinding(StyleProperty, new Binding(ClockRadioButtonStyleProperty.Name) { Source = this });
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

    #endregion Public Methods

    #region Private Methods

    private bool CheckNull()
    {
        if (_buttonPm == null || _buttonAm == null || ButtonConfirm == null || _canvas == null ||
            _borderTitle == null || _borderClock == null || _circlePanel == null ||
            _blockTime == null) return false;

        return true;
    }

    private void BorderTitle_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (e.Delta < 0)
        {
            SecValue--;
            Update();
        }
        else
        {
            SecValue++;
            Update();
        }
        e.Handled = true;
    }

    private void Canvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var value = (int) _rotateTransformClock.Angle;
        if (e.Delta < 0)
        {
            value += 6;
        }
        else
        {
            value -= 6;
        }
        if (value < 0)
        {
            value = value + 360;
        }
        _rotateTransformClock.Angle = value;

        Update();
        e.Handled = true;
    }

    private void Canvas_OnClick(object sender, RoutedEventArgs e)
    {
        _currentButton = e.OriginalSource as ClockRadioButton;
        if (_currentButton != null)
        {
            Update();
        }
    }

    private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var value = ArithmeticHelper.CalAngle(new Point(85, 85), e.GetPosition(_canvas)) + 90;
            if (value < 0)
            {
                value = value + 360;
            }
            value = value - value % 6;
            _rotateTransformClock.Angle = value;
            Update();
        }
    }

    private void Update()
    {
        if (!AppliedTemplate) return;
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

    /// <summary>
    ///     更新
    /// </summary>
    /// <param name="time"></param>
    internal override void Update(DateTime time)
    {
        if (!AppliedTemplate) return;
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
        ctl.RaiseEvent(new RoutedEventArgs { RoutedEvent = ButtonBase.ClickEvent });

        _secValue = time.Second;
        Update();
    }

    /// <summary>
    ///     获取显示时间
    /// </summary>
    /// <returns></returns>
    private DateTime GetDisplayTime()
    {
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
        return new DateTime(now.Year, now.Month, now.Day, hValue, (int) Math.Abs(_rotateTransformClock.Angle) % 360 / 6, _secValue);
    }

    private void ButtonAm_OnClick(object sender, RoutedEventArgs e) => Update();

    private void ButtonPm_OnClick(object sender, RoutedEventArgs e) => Update();

    #endregion Private Methods       
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementButtonAm, Type = typeof(RadioButton))]
    [TemplatePart(Name = ElementButtonPm, Type = typeof(RadioButton))]
    [TemplatePart(Name = ElementButtonConfirm, Type = typeof(Button))]
    [TemplatePart(Name = ElementCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = ElementBorderTitle, Type = typeof(Border))]
    [TemplatePart(Name = ElementBorderClock, Type = typeof(Border))]
    [TemplatePart(Name = ElementPanelNum, Type = typeof(CirclePanel))]
    [TemplatePart(Name = ElementTimeStr, Type = typeof(TextBlock))]
    public class Clock : Control
    {
        #region Constants

        private const string ElementButtonAm = "PART_ButtonAm";
        private const string ElementButtonPm = "PART_ButtonPm";
        private const string ElementButtonConfirm = "PART_ButtonConfirm";
        private const string ElementCanvas = "PART_Canvas";
        private const string ElementBorderTitle = "PART_BorderTitle";
        private const string ElementBorderClock = "PART_BorderClock";
        private const string ElementPanelNum = "PART_PanelNum";
        private const string ElementTimeStr = "PART_TimeStr";

        #endregion Constants

        #region Data

        private RadioButton _buttonAm;

        private RadioButton _buttonPm;

        private Button _buttonConfirm;

        private Canvas _canvas;

        private Border _borderTitle;

        private Border _borderClock;

        private ClockRadioButton _currentButton;

        private RotateTransform _rotateTransformClock;

        private CirclePanel _circlePanel;

        private List<ClockRadioButton> _radioButtonList;

        private TextBlock _blockTime;

        private int _secValue;

        private bool _appliedTemplate;

        #endregion Data

        #region Public Events

        public static readonly RoutedEvent SelectedTimeChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct,
                typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(Clock));

        public event EventHandler<FunctionEventArgs<DateTime?>> SelectedTimeChanged
        {
            add => AddHandler(SelectedTimeChangedEvent, value);
            remove => RemoveHandler(SelectedTimeChangedEvent, value);
        }

        public event EventHandler<FunctionEventArgs<DateTime>> DisplayTimeChanged;

        public event Action Confirmed;

        #endregion Public Events

        #region Public Properties

        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
            "SelectedTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime?), OnSelectedTimeChanged));

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Clock)d;
            var v = (DateTime?)e.NewValue;
            ctl.DisplayTime = v ?? DateTime.Now;
            ctl.OnSelectedTimeChanged(new FunctionEventArgs<DateTime?>(SelectedTimeChangedEvent, ctl)
            {
                Info = v
            });
        }

        public DateTime? SelectedTime
        {
            get => (DateTime?) GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
            "TimeFormat", typeof(string), typeof(Clock), new PropertyMetadata("HH:mm:ss"));

        public string TimeFormat
        {
            get => (string) GetValue(TimeFormatProperty);
            set => SetValue(TimeFormatProperty, value);
        }

        public static readonly DependencyProperty ClockRadioButtonStyleProperty = DependencyProperty.Register(
            "ClockRadioButtonStyle", typeof(Style), typeof(Clock), new PropertyMetadata(default(Style)));

        public Style ClockRadioButtonStyle
        {
            get => (Style) GetValue(ClockRadioButtonStyleProperty);
            set => SetValue(ClockRadioButtonStyleProperty, value);
        }

        public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(
            "DisplayTime", typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDisplayTimeChanged));

        private static void OnDisplayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Clock) d;
            var v = (DateTime) e.NewValue;
            ctl.Update(v);
            ctl.OnDisplayTimeChanged(new FunctionEventArgs<DateTime>(v));
        }

        public DateTime DisplayTime
        {
            get => (DateTime) GetValue(DisplayTimeProperty);
            set => SetValue(DisplayTimeProperty, value);
        }

        internal static readonly DependencyProperty ShowConfirmButtonProperty = DependencyProperty.Register(
            "ShowConfirmButton", typeof(bool), typeof(Clock), new PropertyMetadata(ValueBoxes.FalseBox));

        internal bool ShowConfirmButton
        {
            get => (bool)GetValue(ShowConfirmButtonProperty);
            set => SetValue(ShowConfirmButtonProperty, value);
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
            _appliedTemplate = false;
            if (_buttonAm != null)
            {
                _buttonAm.Click -= ButtonAm_OnClick;
            }

            if (_buttonPm != null)
            {
                _buttonPm.Click -= ButtonPm_OnClick;
            }

            if (_buttonConfirm != null)
            {
                _buttonConfirm.Click -= ButtonConfirm_OnClick;
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
            _buttonConfirm = GetTemplateChild(ElementButtonConfirm) as Button;
            _borderTitle = GetTemplateChild(ElementBorderTitle) as Border;
            _canvas = GetTemplateChild(ElementCanvas) as Canvas;
            _borderClock = GetTemplateChild(ElementBorderClock) as Border;
            _circlePanel = GetTemplateChild(ElementPanelNum) as CirclePanel;
            _blockTime = GetTemplateChild(ElementTimeStr) as TextBlock;

            CheckNull();

            _buttonAm.Click += ButtonAm_OnClick;
            _buttonPm.Click += ButtonPm_OnClick;
            _buttonConfirm.Click += ButtonConfirm_OnClick;
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

            _appliedTemplate = true;
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

        #region Protected Methods

        protected virtual void OnSelectedTimeChanged(FunctionEventArgs<DateTime?> e) => RaiseEvent(e);

        protected virtual void OnDisplayTimeChanged(FunctionEventArgs<DateTime> e)
        {
            var handler = DisplayTimeChanged;
            handler?.Invoke(this, e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void CheckNull()
        {

            if (_buttonPm == null || _buttonAm == null || _buttonConfirm == null || _canvas == null ||
                _borderTitle == null || _borderClock == null || _circlePanel == null ||
                _blockTime == null) throw new Exception();
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
            var value = (int)_rotateTransformClock.Angle;
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
            if (!_appliedTemplate) return;
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
        internal void Update(DateTime time)
        {
            if (!_appliedTemplate) return;
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
            var ctl = _radioButtonList[hRest-1];
            ctl.IsChecked = true;
            ctl.RaiseEvent(new RoutedEventArgs { RoutedEvent = ButtonBase.ClickEvent });

            _secValue = time.Second;
            Update();
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedTime = DisplayTime;
            Confirmed?.Invoke();
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
            return new DateTime(now.Year, now.Month, now.Day, hValue, (int)Math.Abs(_rotateTransformClock.Angle) % 360 / 6, _secValue);
        }

        private void ButtonAm_OnClick(object sender, RoutedEventArgs e) => Update();

        private void ButtonPm_OnClick(object sender, RoutedEventArgs e) => Update();

        #endregion Private Methods       
    }
}
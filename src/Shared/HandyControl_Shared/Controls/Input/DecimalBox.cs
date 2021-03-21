using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <summary>
    ///     数值选择控件
    /// </summary>
    [TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementErrorTip, Type = typeof(UIElement))]
    public class DecimalBox : Control, IDataInput
    {
        #region Constants

        private const string ElementTextBox = "PART_TextBox";

        private const string ElementErrorTip = "PART_ErrorTip";

        #endregion Constants

        #region Data

        private TextBox _textBox;

        private UIElement _errorTip;

        private bool _updateText;

        #endregion Data

        public DecimalBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Prev, (s, e) =>
            {
                if (IsReadOnly) return;

                try
                {
                    Value += Increment;
                }
                catch { }
            }));
            CommandBindings.Add(new CommandBinding(ControlCommands.Next, (s, e) =>
            {
                if (IsReadOnly) return;

                try
                {
                    Value -= Increment;
                }
                catch { }
            }));
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                if (IsReadOnly) return;

                SetCurrentValue(ValueProperty, ValueBoxes.Decimal0Box);
            }));

            Loaded += (s, e) => OnApplyTemplate();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (_textBox != null)
            {
                _textBox?.Focus();
                _textBox.Select(_textBox.Text.Length, 0);
            }
        }

        public override void OnApplyTemplate()
        {
            if (_textBox != null)
            {
                TextCompositionManager.RemovePreviewTextInputHandler(_textBox, PreviewTextInputHandler);
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                _textBox.LostFocus -= TextBox_LostFocus;
            }

            base.OnApplyTemplate();

            _textBox = GetTemplateChild(ElementTextBox) as TextBox;
            _errorTip = GetTemplateChild(ElementErrorTip) as UIElement;

            if (_textBox != null)
            {
                _textBox.SetBinding(SelectionBrushProperty, new Binding(SelectionBrushProperty.Name) { Source = this });
#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)
                _textBox.SetBinding(SelectionTextBrushProperty, new Binding(SelectionTextBrushProperty.Name) { Source = this });
#endif
                _textBox.SetBinding(SelectionOpacityProperty, new Binding(SelectionOpacityProperty.Name) { Source = this });
                _textBox.SetBinding(CaretBrushProperty, new Binding(CaretBrushProperty.Name) { Source = this });

                TextCompositionManager.AddPreviewTextInputHandler(_textBox, PreviewTextInputHandler);
                _textBox.TextChanged += TextBox_TextChanged;
                _textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                _textBox.LostFocus += TextBox_LostFocus;
                _textBox.Text = CurrentText;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateData();

        private void UpdateData()
        {
            if (!VerifyData())
            {
                _updateText = true;
                return;
            }
            if (decimal.TryParse(_textBox.Text, out var value))
            {
                _updateText = false;
                Value = value;
                _updateText = true;
            }
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e) => UpdateData();

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsError && _errorTip != null) return;
            _textBox.Text = CurrentText;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;

            try
            {
                if (e.Key == Key.Up)
                {
                    Value += Increment;
                }
                else if (e.Key == Key.Down)
                {
                    Value -= Increment;
                }
            }
            catch { }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_textBox.IsFocused && !IsReadOnly)
            {
                try
                {
                    Value += e.Delta > 0 ? Increment : -Increment;
                }
                catch { }
                e.Handled = true;
            }
        }

        private string CurrentText => string.IsNullOrWhiteSpace(ValueFormat)
            ? DecimalPlaces.HasValue
                ? Value.ToString($"#0.{new string('0', DecimalPlaces.Value)}")
                : Value.ToString()
            : Value.ToString(ValueFormat);

        protected virtual void OnValueChanged(FunctionEventArgs<decimal> e) => RaiseEvent(e);

        /// <summary>
        ///     值改变事件
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<decimal>>), typeof(DecimalBox));

        /// <summary>
        ///     值改变事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<decimal>> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        /// <summary>
        ///     当前值
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(decimal), typeof(DecimalBox),
            new FrameworkPropertyMetadata(ValueBoxes.Decimal0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged, CoerceValue));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (DecimalBox) d;
            var v = (decimal) e.NewValue;
            ctl.SetText();

            ctl.OnValueChanged(new FunctionEventArgs<decimal>(ValueChangedEvent, ctl)
            {
                Info = v
            });
        }

        private void SetText()
        {
            if (_updateText && _textBox != null)
            {
                _textBox.Text = CurrentText;
                _textBox.Select(_textBox.Text.Length, 0);
            }
        }

        private static object CoerceValue(DependencyObject d, object basevalue)
        {
            var ctl = (DecimalBox) d;
            var minimum = ctl.Minimum;
            var num = (decimal) basevalue;
            if (num < minimum)
            {
                ctl.Value = minimum;
                return minimum;
            }
            var maximum = ctl.Maximum;
            if (num > maximum)
            {
                ctl.Value = maximum;
            }
            ctl.SetText();
            return num > maximum ? maximum : num;
        }

        /// <summary>
        ///     当前值
        /// </summary>
        public decimal Value
        {
            get => (decimal) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(decimal), typeof(DecimalBox), new PropertyMetadata(decimal.MaxValue, OnMaximumChanged, CoerceMaximum));

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (DecimalBox) d;
            ctl.CoerceValue(MinimumProperty);
            ctl.CoerceValue(ValueProperty);
        }

        private static object CoerceMaximum(DependencyObject d, object basevalue)
        {
            var minimum = ((DecimalBox) d).Minimum;
            return (decimal) basevalue < minimum ? minimum : basevalue;
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public decimal Maximum
        {
            get => (decimal) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        ///     最小值
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(decimal), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.Decimal0Box, OnMinimumChanged, CoerceMinimum));

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (DecimalBox) d;
            ctl.CoerceValue(MaximumProperty);
            ctl.CoerceValue(ValueProperty);
        }

        private static object CoerceMinimum(DependencyObject d, object basevalue)
        {
            var maximum = ((DecimalBox) d).Maximum;
            return (decimal) basevalue > maximum ? maximum : basevalue;
        }

        /// <summary>
        ///     最小值
        /// </summary>
        public decimal Minimum
        {
            get => (decimal) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        ///     指示每单击一下按钮时增加或减少的数量
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment", typeof(decimal), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.Decimal1Box));

        /// <summary>
        ///     指示每单击一下按钮时增加或减少的数量
        /// </summary>
        public decimal Increment
        {
            get => (decimal) GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        /// <summary>
        ///     指示要显示的小数位数
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
            "DecimalPlaces", typeof(int?), typeof(DecimalBox), new PropertyMetadata(default(int?)));

        /// <summary>
        ///     指示要显示的小数位数
        /// </summary>
        public int? DecimalPlaces
        {
            get => (int?) GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        /// <summary>
        ///     指示要显示的数字的格式
        /// </summary>
        public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(
            "ValueFormat", typeof(string), typeof(DecimalBox), new PropertyMetadata(default(string)));

        /// <summary>
        ///     指示要显示的数字的格式，这将会覆盖 <see cref="DecimalPlaces"/> 属性
        /// </summary>
        public string ValueFormat
        {
            get => (string) GetValue(ValueFormatProperty);
            set => SetValue(ValueFormatProperty, value);
        }

        /// <summary>
        ///     是否显示上下调值按钮
        /// </summary>
        internal static readonly DependencyProperty ShowUpDownButtonProperty = DependencyProperty.Register(
            "ShowUpDownButton", typeof(bool), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.TrueBox));

        /// <summary>
        ///     是否显示上下调值按钮
        /// </summary>
        internal bool ShowUpDownButton
        {
            get => (bool) GetValue(ShowUpDownButtonProperty);
            set => SetValue(ShowUpDownButtonProperty, ValueBoxes.BooleanBox(value));
        }

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, ValueBoxes.BooleanBox(value));
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(DecimalBox), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public static readonly DependencyPropertyKey TextTypePropertyKey =
            DependencyProperty.RegisterReadOnly("TextType", typeof(TextType), typeof(DecimalBox),
                new PropertyMetadata(default(TextType)));

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyProperty TextTypeProperty = TextTypePropertyKey.DependencyProperty;

        public TextType TextType
        {
            get => (TextType) GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool) GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
        }

        /// <summary>
        ///     标识 IsReadOnly 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(DecimalBox), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     获取或设置一个值，该值指示DecimalBox是否只读。
        /// </summary>
        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            TextBoxBase.SelectionBrushProperty.AddOwner(typeof(DecimalBox));

        public Brush SelectionBrush
        {
            get => (Brush) GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

        public static readonly DependencyProperty SelectionTextBrushProperty =
            TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(DecimalBox));

        public Brush SelectionTextBrush
        {
            get => (Brush) GetValue(SelectionTextBrushProperty);
            set => SetValue(SelectionTextBrushProperty, value);
        }

#endif

        public static readonly DependencyProperty SelectionOpacityProperty =
            TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(DecimalBox));

        public double SelectionOpacity
        {
            get => (double) GetValue(SelectionOpacityProperty);
            set => SetValue(SelectionOpacityProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
            TextBoxBase.CaretBrushProperty.AddOwner(typeof(DecimalBox));

        public Brush CaretBrush
        {
            get => (Brush) GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        public virtual bool VerifyData()
        {
            OperationResult<bool> result;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(_textBox.Text);
            }
            else
            {
                if (!string.IsNullOrEmpty(_textBox.Text))
                {
                    if (decimal.TryParse(_textBox.Text, out var value))
                    {
                        if (value < Minimum || value > Maximum)
                        {
                            result = OperationResult.Failed(Properties.Langs.Lang.OutOfRange);
                        }
                        else
                        {
                            result = OperationResult.Success();
                        }
                    }
                    else
                    {
                        result = OperationResult.Failed(Properties.Langs.Lang.FormatError);
                    }
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
    }
}

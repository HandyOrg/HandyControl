using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
    public class SearchBar : Control, IDataInput
    {
        private const string ElementTextBox = "PART_TextBox";

        private WatermarkTextBox _textBox;

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        public SearchBar()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) => _textBox.Text = string.Empty));
            CommandBindings.Add(new CommandBinding(ControlCommands.Search, (s, e) => OnSearchStarted()));
        }

        public static readonly RoutedEvent SearchStartedEvent =
            EventManager.RegisterRoutedEvent("SearchStarted", RoutingStrategy.Direct,
                typeof(EventHandler<FunctionEventArgs<string>>), typeof(SearchBar));

        public event EventHandler<FunctionEventArgs<string>> SearchStarted
        {
            add => AddHandler(SearchStartedEvent, value);
            remove => RemoveHandler(SearchStartedEvent, value);
        }

        public override void OnApplyTemplate()
        {
            if (_textBox != null)
            {
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.KeyDown -= TextBox_KeyDown;
            }

            base.OnApplyTemplate();

            _textBox = GetTemplateChild(ElementTextBox) as WatermarkTextBox;
            if (_textBox != null)
            {
                SetBinding(TextProperty, new Binding(TextProperty.Name)
                {
                    Source = _textBox,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                _textBox.TextChanged += TextBox_TextChanged;
                _textBox.KeyDown += TextBox_KeyDown;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSearchStarted();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsRealTime)
            {
                OnSearchStarted();
            }
            VerifyData();
        }

        private void OnSearchStarted()
        {
            RaiseEvent(new FunctionEventArgs<string>(SearchStartedEvent, this)
            {
                Info = Text
            });
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SearchBar), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(SearchBar), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(SearchBar), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string)GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(SearchBar), new PropertyMetadata(default(TextType)));

        public TextType TextType
        {
            get => (TextType)GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(SearchBar), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public static readonly DependencyProperty IsRealTimeProperty = DependencyProperty.Register(
            "IsRealTime", typeof(bool), typeof(SearchBar), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public bool IsRealTime
        {
            get => (bool)GetValue(IsRealTimeProperty);
            set => SetValue(IsRealTimeProperty, value);
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
                    if (TextType != TextType.Common)
                    {
                        result = Text.IsKindOf(TextType) ? OperationResult.Success() : OperationResult.Failed(Properties.Langs.Lang.FormatError);
                    }
                    else
                    {
                        result = OperationResult.Success();
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
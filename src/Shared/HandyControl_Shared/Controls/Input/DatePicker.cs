using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
    public class DatePicker : System.Windows.Controls.DatePicker, IDataInput
    {
        private const string ElementTextBox = "PART_TextBox";

        private System.Windows.Controls.TextBox _textBox;

        public DatePicker()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                SetCurrentValue(SelectedDateProperty, null);
                SetCurrentValue(TextProperty, "");
            }));
        }

        public override void OnApplyTemplate()
        {
            if (_textBox != null)
            {
                _textBox.TextChanged -= TextBox_TextChanged;
            }

            base.OnApplyTemplate();

            _textBox = GetTemplateChild(ElementTextBox) as System.Windows.Controls.TextBox;
            if (_textBox != null)
            {
                _textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => VerifyData();

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(DatePicker), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(DatePicker), new PropertyMetadata(default(TextType)));

        public TextType TextType
        {
            get => (TextType) GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(DatePicker), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool) GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
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
    }
}
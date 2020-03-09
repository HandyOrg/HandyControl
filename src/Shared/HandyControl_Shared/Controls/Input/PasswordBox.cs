using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;

namespace HandyControl.Controls
{
    /// <inheritdoc cref="IDataInput" />
    [TemplatePart(Name = ElementPasswordBox, Type = typeof(System.Windows.Controls.PasswordBox))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(System.Windows.Controls.TextBox))]
    public class PasswordBox : Control, IDataInput
    {
        private const string ElementPasswordBox = "PART_PasswordBox";

        private const string ElementTextBox = "PART_TextBox";

        /// <summary>
        ///     掩码字符
        /// </summary>
        public static readonly DependencyProperty PasswordCharProperty =
            System.Windows.Controls.PasswordBox.PasswordCharProperty.AddOwner(typeof(PasswordBox),
                new FrameworkPropertyMetadata('●'));

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(PasswordBox), new PropertyMetadata(default(string)));

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(PasswordBox), new PropertyMetadata(default(TextType)));

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public static readonly DependencyProperty ShowEyeButtonProperty = DependencyProperty.Register(
            "ShowEyeButton", typeof(bool), typeof(PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public static readonly DependencyProperty ShowPasswordProperty = DependencyProperty.Register(
            "ShowPassword", typeof(bool), typeof(PasswordBox),
            new PropertyMetadata(ValueBoxes.FalseBox, OnShowPasswordChanged));

        private SecureString _password;

        private System.Windows.Controls.TextBox _textBox;

        public PasswordBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) => Clear()));
        }

        public System.Windows.Controls.PasswordBox ActualPasswordBox { get; set; }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Password
        {
            get
            {
                if (ShowEyeButton && ShowPassword)
                {
                    return _textBox.Text;
                }
                return ActualPasswordBox?.Password;
            }
            set
            {
                if (ActualPasswordBox == null)
                {
                    _password = new SecureString();
                    if (value == null)
                        value = string.Empty;
                    foreach (var item in value)
                        _password.AppendChar(item);

                    return;
                }

                ActualPasswordBox.Password = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SecureString SecurePassword => ActualPasswordBox?.SecurePassword;

        public char PasswordChar
        {
            get => (char) GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public bool ShowEyeButton
        {
            get => (bool) GetValue(ShowEyeButtonProperty);
            set => SetValue(ShowEyeButtonProperty, value);
        }

        public bool ShowPassword
        {
            get => (bool) GetValue(ShowPasswordProperty);
            set => SetValue(ShowPasswordProperty, value);
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public TextType TextType
        {
            get => (TextType) GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        public bool ShowClearButton
        {
            get => (bool) GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        public virtual bool VerifyData()
        {
            OperationResult<bool> result;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(ShowEyeButton && ShowPassword
                    ? _textBox.Text
                    : ActualPasswordBox.Password);
            }
            else
            {
                if (!string.IsNullOrEmpty(ShowEyeButton && ShowPassword
                    ? _textBox.Text
                    : ActualPasswordBox.Password))
                    result = OperationResult.Success();
                else if (InfoElement.GetNecessary(this))
                    result = OperationResult.Failed(Lang.IsNecessary);
                else
                    result = OperationResult.Success();
            }

            IsError = !result.Data;
            ErrorStr = result.Message;
            return result.Data;
        }

        private static void OnShowPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (PasswordBox) d;
            if (!ctl.ShowEyeButton) return;
            if ((bool) e.NewValue)
            {
                ctl._textBox.Text = ctl.ActualPasswordBox.Password;
                ctl._textBox.Select(string.IsNullOrEmpty(ctl._textBox.Text) ? 0 : ctl._textBox.Text.Length, 0);
            }
            else
            {
                ctl.ActualPasswordBox.Password = ctl._textBox.Text;
                ctl._textBox.Clear();
            }
        }

        public override void OnApplyTemplate()
        {
            if (ActualPasswordBox != null)
                ActualPasswordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if (_textBox != null)
                _textBox.TextChanged -= TextBox_TextChanged;

            base.OnApplyTemplate();

            ActualPasswordBox = GetTemplateChild(ElementPasswordBox) as System.Windows.Controls.PasswordBox;
            _textBox = GetTemplateChild(ElementTextBox) as System.Windows.Controls.TextBox;
            if (ActualPasswordBox != null)
            {
                ActualPasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

                if (_password != null)
                {
                    if (_password.Length > 0)
                    {
                        var valuePtr = IntPtr.Zero;
                        try
                        {
                            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(_password);
                            ActualPasswordBox.Password = Marshal.PtrToStringUni(valuePtr) ?? throw new InvalidOperationException();
                        }
                        finally
                        {
                            Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                            _password.Clear();
                        }
                    }
                }
            }

            if (_textBox != null)
            {
                _textBox.TextChanged += TextBox_TextChanged;
            }
        }

        public void Paste()
        {
            ActualPasswordBox.Paste();
            if (ShowEyeButton && ShowPassword)
            {
                _textBox.Text = ActualPasswordBox.Password;
            }
        }

        public void SelectAll()
        {
            ActualPasswordBox.SelectAll();
            if (ShowEyeButton && ShowPassword)
            {
                _textBox.SelectAll();
            }
        }

        public void Clear()
        {
            ActualPasswordBox.Clear();
            _textBox.Clear();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => VerifyData();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => VerifyData();
    }
}
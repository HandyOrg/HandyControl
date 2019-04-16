using System;
using System.ComponentModel;
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
    public class PasswordBox : Control, IDataInput
    {
        private const string ElementPasswordBox = "PART_PasswordBox";

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

        private string _password;

        private char _passwordChar;

        public PasswordBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) => ActualPasswordBox.Clear()));
        }

        public System.Windows.Controls.PasswordBox ActualPasswordBox { get; set; }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Password
        {
            get => ActualPasswordBox?.Password;
            set
            {
                if (ActualPasswordBox == null)
                {
                    _password = value;
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
                result = VerifyFunc.Invoke(ActualPasswordBox.Password);
            }
            else
            {
                if (!string.IsNullOrEmpty(ActualPasswordBox.Password))
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

        public override void OnApplyTemplate()
        {
            if (ActualPasswordBox != null)
                ActualPasswordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            base.OnApplyTemplate();

            ActualPasswordBox = GetTemplateChild(ElementPasswordBox) as System.Windows.Controls.PasswordBox;
            if (ActualPasswordBox != null)
            {
                ActualPasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

                if (!string.IsNullOrEmpty(_password))
                {
                    ActualPasswordBox.Password = _password;
                    _password = null;
                }
                if (_passwordChar != default(char))
                {
                    ActualPasswordBox.PasswordChar = _passwordChar;
                    _passwordChar = default(char);
                }
            }
        }

        public void Paste() => ActualPasswordBox.Paste();

        public void SelectAll() => ActualPasswordBox.SelectAll();

        public void Clear() => ActualPasswordBox.SelectAll();



        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => VerifyData();
    }
}
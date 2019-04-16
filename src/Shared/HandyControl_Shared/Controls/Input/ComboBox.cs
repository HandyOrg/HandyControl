using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    /// <inheritdoc cref="IDataInput" />
    public class ComboBox : System.Windows.Controls.ComboBox, IDataInput
    {
        public ComboBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                ClearValue(SelectedValueProperty);
                ClearValue(SelectedItemProperty);
                ClearValue(SelectedIndexProperty);
                ClearValue(TextProperty);
            }));
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            VerifyData();
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(ComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(ComboBox), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public static readonly DependencyPropertyKey TextTypePropertyKey =
            DependencyProperty.RegisterReadOnly("TextType", typeof(TextType), typeof(ComboBox),
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
            "ShowClearButton", typeof(bool), typeof(ComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

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
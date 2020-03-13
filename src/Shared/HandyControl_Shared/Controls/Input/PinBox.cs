using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    public class PinBox : Control
    {
        private const string ElementPanel = "PART_Panel";

        private static readonly object MinLength = 4;

        private Panel _panel;

        private int _inputIndex;

        private bool _changed;

        private bool _isInternalAction;

        private List<SecureString> _passwordList;

        private RoutedEventHandler _passwordBoxsGotFocusEventHandler;

        private RoutedEventHandler _passwordBoxsPasswordChangedEventHandler;

        public PinBox()
        {
            Loaded += PinBox_Loaded;
            Unloaded += PinBox_Unloaded;
        }

        private void PinBox_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, _passwordBoxsPasswordChangedEventHandler);
            RemoveHandler(GotFocusEvent, _passwordBoxsGotFocusEventHandler);
            
            Loaded -= PinBox_Loaded;
            Unloaded -= PinBox_Unloaded;
        }

        private void PinBox_Loaded(object sender, RoutedEventArgs e)
        {
            _passwordBoxsPasswordChangedEventHandler = PasswordBoxsPasswordChanged;
            AddHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, _passwordBoxsPasswordChangedEventHandler);
            
            _passwordBoxsGotFocusEventHandler = PasswordBoxsGotFocus;
            AddHandler(GotFocusEvent, _passwordBoxsGotFocusEventHandler);
        }

        private void PasswordBoxsPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isInternalAction) return;

            if (e.OriginalSource is System.Windows.Controls.PasswordBox passwordBox)
            {
                if (passwordBox.Password.Length > 0)
                {
                    if (++_inputIndex >= Length)
                    {
                        _inputIndex = Length - 1;

                        if (_panel.Children.OfType<System.Windows.Controls.PasswordBox>()
                            .All(item => item.Password.Any()))
                        {
                            FocusManager.SetFocusedElement(this, null);
                            Keyboard.ClearFocus();
                            RaiseEvent(new RoutedEventArgs(CompletedEvent, this));
                        }
                        return;
                    }
                }
                else
                {
                    if (--_inputIndex < 0)
                    {
                        _inputIndex = 0;
                        return;
                    }
                }

                _changed = true;
                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
        }

        private void PasswordBoxsGotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.PasswordBox passwordBox)
            {
                _inputIndex = _panel.Children.IndexOf(passwordBox);
                passwordBox.SelectAll();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            if (e.Key == Key.Left)
            {
                if (--_inputIndex < 0)
                {
                    _inputIndex = 0;
                    return;
                }

                var passwordBox = _panel.Children[_inputIndex] as System.Windows.Controls.PasswordBox;
                passwordBox?.SelectAll();

                FocusManager.SetFocusedElement(this, passwordBox);
            }
            else if (e.Key == Key.Right)
            {
                if (++_inputIndex >= Length)
                {
                    _inputIndex = Length - 1;
                    return;
                }

                var passwordBox = _panel.Children[_inputIndex] as System.Windows.Controls.PasswordBox;
                passwordBox?.SelectAll();

                FocusManager.SetFocusedElement(this, passwordBox);
            }
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            if (_changed)
            {
                _changed = false;
                return;
            }

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (_panel.Children[_inputIndex] is System.Windows.Controls.PasswordBox passwordBox)
                {
                    _isInternalAction = true;
                    passwordBox.Clear();
                    _isInternalAction = false;
                }

                if (--_inputIndex < 0)
                {
                    _inputIndex = 0;
                    return;
                }

                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
        }

        public string Password
        {
            get
            {
                return string.Join("", _panel.Children.OfType<System.Windows.Controls.PasswordBox>().Select(item => item.Password));
            }
            set
            {
                if (_panel == null)
                {
                    _passwordList = new List<SecureString>();

                    if (value == null)
                    {
                        value = string.Empty;
                    }

                    foreach (var item in value)
                    {
                        var secureString = new SecureString();
                        secureString.AppendChar(item);
                        _passwordList.Add(secureString);
                    }

                    return;
                }

                var length = Length;
                if (string.IsNullOrEmpty(value) || value.Length != length || value.Length != _panel.Children.Count)
                {
                    _panel.Children.OfType<System.Windows.Controls.PasswordBox>().Do(item => item.Clear());
                }
                else
                {
                    _panel.Children.OfType<System.Windows.Controls.PasswordBox>().DoWithIndex((item, index) => item.Password = value[index].ToString());
                }
            }
        }

        /// <summary>
        ///     掩码字符
        /// </summary>
        public static readonly DependencyProperty PasswordCharProperty =
            System.Windows.Controls.PasswordBox.PasswordCharProperty.AddOwner(typeof(PinBox),
                new FrameworkPropertyMetadata('●'));

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length", typeof(int), typeof(PinBox), new PropertyMetadata(MinLength, OnLengthChanged, CoerceLength), ValidateHelper.IsInRangeOfPosInt);

        private static void OnLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (PinBox) d;
            ctl.UpdateItems();
        }

        private static object CoerceLength(DependencyObject d, object basevalue) => (int)basevalue < 4 ? MinLength : basevalue;

        public int Length
        {
            get => (int) GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof(Thickness), typeof(PinBox), new PropertyMetadata(default(Thickness)));

        public Thickness ItemMargin
        {
            get => (Thickness) GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            "ItemWidth", typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

        public double ItemWidth
        {
            get => (double) GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight", typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

        public double ItemHeight
        {
            get => (double) GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(PinBox));

        public event RoutedEventHandler Completed
        {
            add => AddHandler(CompletedEvent, value);
            remove => RemoveHandler(CompletedEvent, value);
        }

        private void UpdateItems()
        {
            if (_panel == null) return;

            _panel.Children.Clear();
            var length = Length;

            for (var i = 0; i < length; i++)
            {
                _panel.Children.Add(CreatePasswordBox());
            }
        }

        private System.Windows.Controls.PasswordBox CreatePasswordBox()
        {
            return new System.Windows.Controls.PasswordBox
            {
                MaxLength = 1,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = ItemMargin,
                Width = ItemWidth,
                Height = ItemHeight,
                Padding = default,
                PasswordChar = PasswordChar
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panel = GetTemplateChild(ElementPanel) as Panel;
            if (_panel != null)
            {
                UpdateItems();

                var length = Length;
                if (_passwordList != null && _passwordList.Count == length && _panel.Children.Count == length)
                {
                    for (var i = 0; i < length; i++)
                    {
                        var password = _passwordList[i];
                        if (password.Length > 0)
                        {
                            var valuePtr = IntPtr.Zero;
                            try
                            {
                                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(password);
                                if (_panel.Children[i] is System.Windows.Controls.PasswordBox passwordBox)
                                {
                                    passwordBox.Password = Marshal.PtrToStringUni(valuePtr) ?? throw new InvalidOperationException();
                                }
                            }
                            finally
                            {
                                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                                password.Clear();
                            }
                        }
                    }

                    _passwordList.Clear();
                }
            }
        }
    }
}

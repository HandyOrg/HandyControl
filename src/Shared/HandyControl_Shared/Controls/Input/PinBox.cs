using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

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

        FocusPasswordBox();
    }

    private void FocusPasswordBox()
    {
        if (!IsFocused)
        {
            return;
        }

        if (_panel.Children.Count == 0)
        {
            return;
        }

        if (_panel.Children.OfType<System.Windows.Controls.PasswordBox>().Any(box => box.IsFocused))
        {
            return;
        }

        FocusManager.SetFocusedElement(this, _panel.Children[0]);
    }

    private void PasswordBoxsPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_isInternalAction) return;

        if (e.OriginalSource is System.Windows.Controls.PasswordBox passwordBox)
        {
            if (!IsSafeEnabled)
            {
                SetCurrentValue(UnsafePasswordProperty, Password);
            }

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
            return _panel == null
                ? string.Empty
                : string.Join(string.Empty, _panel.Children.OfType<System.Windows.Controls.PasswordBox>().Select(item => item.Password));
        }
        set
        {
            if (_panel == null)
            {
                _passwordList = new List<SecureString>();

                value ??= string.Empty;

                foreach (var item in value)
                {
                    var secureString = new SecureString();
                    secureString.AppendChar(item);
                    _passwordList.Add(secureString);
                }

                return;
            }

            _isInternalAction = true;

            if (string.IsNullOrEmpty(value))
            {
                _panel.Children.OfType<System.Windows.Controls.PasswordBox>().Do(item => item.Clear());
            }
            else
            {
                _panel.Children
                    .OfType<System.Windows.Controls.PasswordBox>()
                    .Take(Math.Min(Length, value.Length))
                    .DoWithIndex((item, index) => item.Password = value[index].ToString());

                _panel.Children
                    .OfType<System.Windows.Controls.PasswordBox>()
                    .Skip(value.Length)
                    .Take(Length - value.Length)
                    .Do(item => item.Clear());
            }

            _isInternalAction = false;

            if (!IsSafeEnabled)
            {
                SetCurrentValue(UnsafePasswordProperty, Password);
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
        get => (char) GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
        nameof(Length), typeof(int), typeof(PinBox), new PropertyMetadata(MinLength, OnLengthChanged, CoerceLength), ValidateHelper.IsInRangeOfPosInt);

    private static void OnLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (PinBox) d;
        ctl.UpdateItems();
    }

    private static object CoerceLength(DependencyObject d, object basevalue) => (int) basevalue < 4 ? MinLength : basevalue;

    public int Length
    {
        get => (int) GetValue(LengthProperty);
        set => SetValue(LengthProperty, value);
    }

    public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
        nameof(ItemMargin), typeof(Thickness), typeof(PinBox), new PropertyMetadata(default(Thickness)));

    public Thickness ItemMargin
    {
        get => (Thickness) GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }

    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
        nameof(ItemWidth), typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

    public double ItemWidth
    {
        get => (double) GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
        nameof(ItemHeight), typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

    public double ItemHeight
    {
        get => (double) GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        TextBoxBase.SelectionBrushProperty.AddOwner(typeof(PinBox));

    public Brush SelectionBrush
    {
        get => (Brush) GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

    public static readonly DependencyProperty SelectionTextBrushProperty =
        TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(PinBox));

    public Brush SelectionTextBrush
    {
        get => (Brush) GetValue(SelectionTextBrushProperty);
        set => SetValue(SelectionTextBrushProperty, value);
    }

#endif

    public static readonly DependencyProperty SelectionOpacityProperty =
        TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(PinBox));

    public double SelectionOpacity
    {
        get => (double) GetValue(SelectionOpacityProperty);
        set => SetValue(SelectionOpacityProperty, value);
    }

    public static readonly DependencyProperty CaretBrushProperty =
        TextBoxBase.CaretBrushProperty.AddOwner(typeof(PinBox));

    public Brush CaretBrush
    {
        get => (Brush) GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

    public static readonly DependencyProperty IsSafeEnabledProperty = PasswordBox.IsSafeEnabledProperty
        .AddOwner(typeof(PinBox), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, OnIsSafeEnabledChanged));

    private static void OnIsSafeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var p = (PinBox) d;
        p.OnIsSafeEnabledChanged((bool) e.NewValue);
    }

    private void OnIsSafeEnabledChanged(bool newValue)
    {
        if (_panel == null)
        {
            return;
        }

        SetCurrentValue(UnsafePasswordProperty, !newValue ? Password : string.Empty);
    }

    public bool IsSafeEnabled
    {
        get => (bool) GetValue(IsSafeEnabledProperty);
        set => SetValue(IsSafeEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty UnsafePasswordProperty = PasswordBox.UnsafePasswordProperty
        .AddOwner(typeof(PinBox), new FrameworkPropertyMetadata(default(string),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUnsafePasswordChanged));

    private static void OnUnsafePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var p = (PinBox) d;
        if (!p.IsSafeEnabled)
        {
            p.Password = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
        }
    }

    public string UnsafePassword
    {
        get => (string) GetValue(UnsafePasswordProperty);
        set => SetValue(UnsafePasswordProperty, value);
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
        var passwordBox = new System.Windows.Controls.PasswordBox
        {
            MaxLength = 1,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = ItemMargin,
            Width = ItemWidth,
            Height = ItemHeight,
            Padding = default,
            PasswordChar = PasswordChar,
            Foreground = Foreground
        };

        passwordBox.SetBinding(SelectionBrushProperty, new Binding(SelectionBrushProperty.Name) { Source = this });
#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)
        passwordBox.SetBinding(SelectionTextBrushProperty, new Binding(SelectionTextBrushProperty.Name) { Source = this });
#endif
        passwordBox.SetBinding(SelectionOpacityProperty, new Binding(SelectionOpacityProperty.Name) { Source = this });
        passwordBox.SetBinding(CaretBrushProperty, new Binding(CaretBrushProperty.Name) { Source = this });

        return passwordBox;
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

            OnIsSafeEnabledChanged(IsSafeEnabled);
        }
    }
}

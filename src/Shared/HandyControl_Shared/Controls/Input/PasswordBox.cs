﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementPasswordBox, Type = typeof(System.Windows.Controls.PasswordBox))]
[TemplatePart(Name = ElementTextBox, Type = typeof(System.Windows.Controls.TextBox))]
public class PasswordBox : Control
{
    private const string ElementPasswordBox = "PART_PasswordBox";

    private const string ElementTextBox = "PART_TextBox";

    private SecureString _password;

    private System.Windows.Controls.TextBox _textBox;

    /// <summary>
    ///     掩码字符
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty =
        System.Windows.Controls.PasswordBox.PasswordCharProperty.AddOwner(typeof(PasswordBox),
            new FrameworkPropertyMetadata('●'));

    public char PasswordChar
    {
        get => (char) GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public static readonly DependencyProperty ShowEyeButtonProperty = DependencyProperty.Register(
        nameof(ShowEyeButton), typeof(bool), typeof(PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowEyeButton
    {
        get => (bool) GetValue(ShowEyeButtonProperty);
        set => SetValue(ShowEyeButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ShowPasswordProperty = DependencyProperty.Register(
        nameof(ShowPassword), typeof(bool), typeof(PasswordBox),
        new PropertyMetadata(ValueBoxes.FalseBox, OnShowPasswordChanged));

    public bool ShowPassword
    {
        get => (bool) GetValue(ShowPasswordProperty);
        set => SetValue(ShowPasswordProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty IsSafeEnabledProperty = DependencyProperty.Register(
        nameof(IsSafeEnabled), typeof(bool), typeof(PasswordBox), new PropertyMetadata(ValueBoxes.TrueBox, OnIsSafeEnabledChanged));

    private static void OnIsSafeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var p = (PasswordBox) d;
        p.SetCurrentValue(UnsafePasswordProperty, !(bool) e.NewValue ? p.Password : string.Empty);
    }

    public bool IsSafeEnabled
    {
        get => (bool) GetValue(IsSafeEnabledProperty);
        set => SetValue(IsSafeEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty UnsafePasswordProperty = DependencyProperty.Register(
        nameof(UnsafePassword), typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(default(string),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUnsafePasswordChanged));

    private static void OnUnsafePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var p = (PasswordBox) d;
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

    public static readonly DependencyProperty MaxLengthProperty =
        System.Windows.Controls.TextBox.MaxLengthProperty.AddOwner(typeof(PasswordBox));

    public int MaxLength
    {
        get => (int) GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        TextBoxBase.SelectionBrushProperty.AddOwner(typeof(PasswordBox));

    public Brush SelectionBrush
    {
        get => (Brush) GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

    public static readonly DependencyProperty SelectionTextBrushProperty =
        TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(PasswordBox));

    public Brush SelectionTextBrush
    {
        get => (Brush) GetValue(SelectionTextBrushProperty);
        set => SetValue(SelectionTextBrushProperty, value);
    }

#endif

    public static readonly DependencyProperty SelectionOpacityProperty =
        TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(PasswordBox));

    public double SelectionOpacity
    {
        get => (double) GetValue(SelectionOpacityProperty);
        set => SetValue(SelectionOpacityProperty, value);
    }

    public static readonly DependencyProperty CaretBrushProperty =
        TextBoxBase.CaretBrushProperty.AddOwner(typeof(PasswordBox));

    public Brush CaretBrush
    {
        get => (Brush) GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

#if !NET40

    public static readonly DependencyProperty IsSelectionActiveProperty =
        TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(PasswordBox));

    public bool IsSelectionActive => ActualPasswordBox != null && (bool) ActualPasswordBox.GetValue(IsSelectionActiveProperty);

#endif

    public PasswordBox()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) => Clear()));
        CommandBindings.Add(new CommandBinding(ControlCommands.Focus, (s, e) => Focus()));
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
                value ??= string.Empty;
                foreach (var item in value)
                    _password.AppendChar(item);

                return;
            }

            if (Equals(ActualPasswordBox.Password, value)) return;
            ActualPasswordBox.Password = value;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SecureString SecurePassword => ActualPasswordBox?.SecurePassword;

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
            ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.MaxLengthProperty, new Binding(MaxLengthProperty.Name) { Source = this });
            ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.SelectionBrushProperty, new Binding(SelectionBrushProperty.Name) { Source = this });
#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)
            ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.SelectionTextBrushProperty, new Binding(SelectionTextBrushProperty.Name) { Source = this });
#endif
            ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.SelectionOpacityProperty, new Binding(SelectionOpacityProperty.Name) { Source = this });
            ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.CaretBrushProperty, new Binding(CaretBrushProperty.Name) { Source = this });

            if (_password is { Length: > 0 })
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

#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
    public void Focus()
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
    {
        ActualPasswordBox.Focus();
        _textBox.Focus();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (!IsSafeEnabled)
        {
            SetCurrentValue(UnsafePasswordProperty, ActualPasswordBox.Password);

            if (ShowPassword)
            {
                _textBox.Text = ActualPasswordBox.Password;
            }
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsSafeEnabled && ShowPassword)
        {
            Password = _textBox.Text;
            SetCurrentValue(UnsafePasswordProperty, Password);
        }
    }
}

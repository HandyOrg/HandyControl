using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class PinBox : TemplatedControl
{
    private const string ElementPanel = "PART_Panel";
    private const int MinLength = 4;

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<PinBox, char>(nameof(PasswordChar), '●');

    public static readonly StyledProperty<int> LengthProperty =
        AvaloniaProperty.Register<PinBox, int>(nameof(Length), MinLength);

    public static readonly StyledProperty<Thickness> ItemMarginProperty =
        AvaloniaProperty.Register<PinBox, Thickness>(nameof(ItemMargin), new Thickness(4, 0));

    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<PinBox, double>(nameof(ItemWidth), 32d);

    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<PinBox, double>(nameof(ItemHeight), 32d);

    public static readonly StyledProperty<IBrush?> SelectionBrushProperty =
        AvaloniaProperty.Register<PinBox, IBrush?>(nameof(SelectionBrush));

    public static readonly StyledProperty<IBrush?> CaretBrushProperty =
        AvaloniaProperty.Register<PinBox, IBrush?>(nameof(CaretBrush));

    public static readonly StyledProperty<bool> IsSafeEnabledProperty =
        AvaloniaProperty.Register<PinBox, bool>(nameof(IsSafeEnabled), true);

    public static readonly StyledProperty<string?> UnsafePasswordProperty =
        AvaloniaProperty.Register<PinBox, string?>(nameof(UnsafePassword), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string?> PasswordProperty =
        AvaloniaProperty.Register<PinBox, string?>(nameof(Password));

    public static readonly RoutedEvent<RoutedEventArgs> CompletedEvent =
        RoutedEvent.Register<PinBox, RoutedEventArgs>(nameof(Completed), RoutingStrategies.Bubble);

    private Panel? _panel;
    private readonly List<TextBox> _textBoxes = [];
    private bool _isInternalAction;
    private int _inputIndex;
    private string _cachedPassword = string.Empty;

    static PinBox()
    {
        FocusableProperty.OverrideDefaultValue<PinBox>(false);

        LengthProperty.Changed.AddClassHandler<PinBox>((pinBox, e) =>
        {
            var newLength = e.GetNewValue<int>();
            if (newLength < MinLength)
            {
                pinBox.SetCurrentValue(LengthProperty, MinLength);
                return;
            }

            pinBox.UpdateItems();
        });

        ItemMarginProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        ItemWidthProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        ItemHeightProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        PasswordCharProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        ForegroundProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        SelectionBrushProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());
        CaretBrushProperty.Changed.AddClassHandler<PinBox>((pinBox, _) => pinBox.ApplyItemVisualProperties());

        PasswordProperty.Changed.AddClassHandler<PinBox>((pinBox, e) =>
        {
            if (pinBox._isInternalAction)
            {
                return;
            }

            pinBox.ApplyPasswordToItems(e.GetNewValue<string?>() ?? string.Empty);
            pinBox.UpdateUnsafePassword();
        });

        UnsafePasswordProperty.Changed.AddClassHandler<PinBox>((pinBox, e) =>
        {
            if (pinBox._isInternalAction || pinBox.IsSafeEnabled)
            {
                return;
            }

            pinBox.SetCurrentValue(PasswordProperty, e.GetNewValue<string?>() ?? string.Empty);
        });

        IsSafeEnabledProperty.Changed.AddClassHandler<PinBox>((pinBox, e) => pinBox.OnIsSafeEnabledChanged(e.GetNewValue<bool>()));
    }

    public event EventHandler<RoutedEventArgs> Completed
    {
        add => AddHandler(CompletedEvent, value);
        remove => RemoveHandler(CompletedEvent, value);
    }

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public int Length
    {
        get => GetValue(LengthProperty);
        set => SetValue(LengthProperty, value < MinLength ? MinLength : value);
    }

    public Thickness ItemMargin
    {
        get => GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }

    public double ItemWidth
    {
        get => GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public IBrush? SelectionBrush
    {
        get => GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

    public IBrush? CaretBrush
    {
        get => GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

    public bool IsSafeEnabled
    {
        get => GetValue(IsSafeEnabledProperty);
        set => SetValue(IsSafeEnabledProperty, value);
    }

    public string UnsafePassword
    {
        get => GetValue(UnsafePasswordProperty) ?? string.Empty;
        set => SetValue(UnsafePasswordProperty, value);
    }

    public string Password
    {
        get => GetValue(PasswordProperty) ?? string.Empty;
        set => SetValue(PasswordProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _panel = e.NameScope.Find<Panel>(ElementPanel);
        UpdateItems();

        var initialPassword = string.IsNullOrEmpty(Password) ? _cachedPassword : Password;
        if (!string.IsNullOrEmpty(initialPassword))
        {
            ApplyPasswordToItems(initialPassword);
        }

        OnIsSafeEnabledChanged(IsSafeEnabled);
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        FocusPasswordBox();
    }

    private void FocusPasswordBox()
    {
        if (_textBoxes.Count == 0)
        {
            return;
        }

        if (_textBoxes.Any(box => box.IsFocused))
        {
            return;
        }

        var index = Math.Clamp(_inputIndex, 0, _textBoxes.Count - 1);
        _textBoxes[index].Focus();
        _textBoxes[index].SelectAll();
    }

    private void UpdateItems()
    {
        if (_panel == null)
        {
            return;
        }

        _panel.Children.Clear();
        _textBoxes.Clear();
        _inputIndex = 0;

        var length = Math.Max(Length, MinLength);
        for (var i = 0; i < length; i++)
        {
            var textBox = CreateTextBox();
            _textBoxes.Add(textBox);
            _panel.Children.Add(textBox);
        }

        ApplyItemVisualProperties();
    }

    private TextBox CreateTextBox()
    {
        var textBox = new TextBox
        {
            MaxLength = 1,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Padding = default
        };

        textBox.TextChanged += TextBoxOnTextChanged;
        textBox.GotFocus += TextBoxOnGotFocus;
        textBox.AddHandler(InputElement.KeyDownEvent, TextBoxOnKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);

        return textBox;
    }

    private void ApplyItemVisualProperties()
    {
        foreach (var textBox in _textBoxes)
        {
            textBox.Margin = ItemMargin;
            textBox.Width = ItemWidth;
            textBox.Height = ItemHeight;
            textBox.PasswordChar = PasswordChar;
            textBox.Foreground = Foreground;
            textBox.SelectionBrush = SelectionBrush;
            textBox.CaretBrush = CaretBrush;
        }
    }

    private void TextBoxOnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        _inputIndex = _textBoxes.IndexOf(textBox);
        textBox.SelectAll();
    }

    private void TextBoxOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isInternalAction || sender is not TextBox textBox)
        {
            return;
        }

        var index = _textBoxes.IndexOf(textBox);
        if (index < 0)
        {
            return;
        }

        if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text!.Length > 1)
        {
            _isInternalAction = true;
            textBox.Text = textBox.Text[^1].ToString();
            _isInternalAction = false;
        }

        _inputIndex = index;

        if (!string.IsNullOrEmpty(textBox.Text))
        {
            MoveToNext(index);
        }

        SyncPasswordFromItems();

        if (_textBoxes.Count > 0 && _textBoxes.All(item => !string.IsNullOrEmpty(item.Text)))
        {
            RaiseEvent(new RoutedEventArgs(CompletedEvent, this));
            Dispatcher.UIThread.Post(() => TopLevel.GetTopLevel(this)?.FocusManager?.ClearFocus(), DispatcherPriority.Background);
        }
    }

    private void TextBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        var index = _textBoxes.IndexOf(textBox);
        if (index < 0)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Left:
                MoveFocus(Math.Max(index - 1, 0));
                e.Handled = true;
                break;
            case Key.Right:
                MoveFocus(Math.Min(index + 1, _textBoxes.Count - 1));
                e.Handled = true;
                break;
            case Key.Back:
                HandleBackspace(index, textBox);
                e.Handled = true;
                break;
            case Key.Delete:
                HandleDelete(index, textBox);
                e.Handled = true;
                break;
        }
    }

    private void HandleBackspace(int index, TextBox textBox)
    {
        var target = Math.Max(index - 1, 0);

        _isInternalAction = true;
        if (string.IsNullOrEmpty(textBox.Text) && index > 0)
        {
            _textBoxes[target].Clear();
        }
        else
        {
            textBox.Clear();
        }
        _isInternalAction = false;

        MoveFocusDeferred(target);
        SyncPasswordFromItems();
    }

    private void HandleDelete(int index, TextBox textBox)
    {
        var target = Math.Max(index - 1, 0);

        _isInternalAction = true;
        if (string.IsNullOrEmpty(textBox.Text) && index > 0)
        {
            _textBoxes[target].Clear();
        }
        else
        {
            textBox.Clear();
        }
        _isInternalAction = false;

        MoveFocusDeferred(target);
        SyncPasswordFromItems();
    }

    private void MoveToNext(int index)
    {
        if (index >= _textBoxes.Count - 1)
        {
            _inputIndex = _textBoxes.Count - 1;
            return;
        }

        MoveFocus(index + 1);
    }

    private void MoveFocus(int index)
    {
        if (_textBoxes.Count == 0)
        {
            return;
        }

        _inputIndex = Math.Clamp(index, 0, _textBoxes.Count - 1);
        var textBox = _textBoxes[_inputIndex];
        textBox.Focus();
        textBox.SelectAll();
    }

    private void MoveFocusDeferred(int index)
    {
        _inputIndex = Math.Clamp(index, 0, Math.Max(_textBoxes.Count - 1, 0));
        Dispatcher.UIThread.Post(() => MoveFocus(_inputIndex), DispatcherPriority.Input);
    }

    private void SyncPasswordFromItems()
    {
        var password = string.Concat(_textBoxes.Select(box => box.Text ?? string.Empty));
        _cachedPassword = password;

        _isInternalAction = true;
        SetCurrentValue(PasswordProperty, password);
        _isInternalAction = false;

        UpdateUnsafePassword();
    }

    private void ApplyPasswordToItems(string password)
    {
        _cachedPassword = password;

        if (_textBoxes.Count == 0)
        {
            return;
        }

        _isInternalAction = true;

        var limited = password.Length > _textBoxes.Count ? password[.._textBoxes.Count] : password;
        for (var i = 0; i < _textBoxes.Count; i++)
        {
            _textBoxes[i].Text = i < limited.Length ? limited[i].ToString() : string.Empty;
        }

        _isInternalAction = false;
        _inputIndex = Math.Min(limited.Length, _textBoxes.Count - 1);
    }

    private void OnIsSafeEnabledChanged(bool isSafeEnabled)
    {
        if (isSafeEnabled)
        {
            _isInternalAction = true;
            SetCurrentValue(UnsafePasswordProperty, string.Empty);
            _isInternalAction = false;
            return;
        }

        UpdateUnsafePassword();
    }

    private void UpdateUnsafePassword()
    {
        _isInternalAction = true;
        SetCurrentValue(UnsafePasswordProperty, IsSafeEnabled ? string.Empty : (_textBoxes.Count == 0 ? _cachedPassword : string.Concat(_textBoxes.Select(item => item.Text ?? string.Empty))));
        _isInternalAction = false;
    }
}

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace HandyControl.Controls;

[TemplatePart(ElementIcon, typeof(Control))]
public class RateItem : TemplatedControl
{
    private const string ElementIcon = "PART_Icon";

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(AllowClear), true);

    public static readonly StyledProperty<bool> AllowHalfProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(AllowHalf));

    public static readonly StyledProperty<Geometry?> IconProperty =
        AvaloniaProperty.Register<RateItem, Geometry?>(nameof(Icon));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(IsReadOnly));

    internal static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(IsSelected));

    private Control? _icon;
    private bool _isHalf;
    private bool _isPointerPressed;
    private bool _isSentValue;

    public event EventHandler? SelectedChanged;

    public event EventHandler? ValueChanged;

    static RateItem()
    {
        FocusableProperty.OverrideDefaultValue<RateItem>(false);
        IsSelectedProperty.Changed.AddClassHandler<RateItem>((item, _) => item.UpdateIcon());
    }

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    internal bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    internal bool IsHalf
    {
        get => _isHalf;
        set
        {
            if (_isHalf == value)
            {
                return;
            }

            _isHalf = value;
            UpdateIcon();
        }
    }

    internal int Index { get; set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _icon = e.NameScope.Find<Control>(ElementIcon);
        UpdateIcon();
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (IsReadOnly)
        {
            return;
        }

        _isSentValue = false;
        IsSelected = true;
        SelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (IsReadOnly || !AllowHalf)
        {
            return;
        }

        var point = e.GetPosition(this);
        IsHalf = point.X < Bounds.Width / 2;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (IsReadOnly || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        _isPointerPressed = true;
        e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (IsReadOnly || !_isPointerPressed)
        {
            return;
        }

        if (Index == 1 && AllowClear)
        {
            if (IsSelected)
            {
                if (!_isSentValue)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                    _isPointerPressed = false;
                    _isSentValue = true;
                    e.Handled = true;
                    return;
                }

                _isSentValue = false;
                IsSelected = false;
                IsHalf = false;
            }
            else
            {
                IsSelected = true;
                if (AllowHalf)
                {
                    var point = e.GetPosition(this);
                    IsHalf = point.X < Bounds.Width / 2;
                }
            }
        }

        ValueChanged?.Invoke(this, EventArgs.Empty);
        _isPointerPressed = false;
        e.Handled = true;
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        if (IsReadOnly)
        {
            return;
        }

        _isPointerPressed = false;
    }

    private void UpdateIcon()
    {
        if (_icon == null)
        {
            return;
        }

        _icon.IsVisible = IsSelected;
        var width = double.IsNaN(Width) ? Bounds.Width : Width;
        _icon.Width = IsHalf ? width / 2 : double.NaN;
    }
}

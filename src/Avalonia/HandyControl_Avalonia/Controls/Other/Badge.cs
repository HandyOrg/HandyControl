using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HandyControl.Data;

namespace HandyControl.Controls;

/// <summary>
///     标记
/// </summary>
public class Badge : ContentControl
{
    private int? _originalValue;

    public static readonly RoutedEvent<FunctionEventArgs<int>> ValueChangedEvent =
        RoutedEvent.Register<Badge, FunctionEventArgs<int>>(nameof(ValueChanged), RoutingStrategies.Bubble);

    public event EventHandler<FunctionEventArgs<int>> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Badge, string?>(nameof(Text), "0");

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(Value));

    public int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<BadgeStatus> StatusProperty =
        AvaloniaProperty.Register<Badge, BadgeStatus>(nameof(Status));

    public BadgeStatus Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static readonly StyledProperty<int> MaximumProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(Maximum), 99);

    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<Thickness> BadgeMarginProperty =
        AvaloniaProperty.Register<Badge, Thickness>(nameof(BadgeMargin));

    public Thickness BadgeMargin
    {
        get => GetValue(BadgeMarginProperty);
        set => SetValue(BadgeMarginProperty, value);
    }

    public static readonly StyledProperty<bool> ShowBadgeProperty =
        AvaloniaProperty.Register<Badge, bool>(nameof(ShowBadge), true);

    public bool ShowBadge
    {
        get => GetValue(ShowBadgeProperty);
        set => SetValue(ShowBadgeProperty, value);
    }

    static Badge()
    {
        ValueProperty.Changed.AddClassHandler<Badge>((badge, e) => badge.OnValueChanged(e));
        MaximumProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateText(badge.Value));
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var v = e.GetNewValue<int>();
        UpdateText(v);

        if (IsLoaded)
        {
            RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, this) { Info = v });
        }
        else
        {
            _originalValue = v;
        }
    }

    private void UpdateText(int v)
    {
        SetCurrentValue(TextProperty, v <= Maximum ? v.ToString() : $"{Maximum}+");
    }

    protected override void OnApplyTemplate(Avalonia.Controls.Primitives.TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_originalValue != null)
        {
            RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, this) { Info = _originalValue.Value });
            _originalValue = null;
        }
    }
}

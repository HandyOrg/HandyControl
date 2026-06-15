using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

public class Rate : ItemsControl
{
    public static readonly StyledProperty<bool> AllowHalfProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowHalf), inherits: true);

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowClear), true, inherits: true);

    public static readonly StyledProperty<Geometry?> IconProperty =
        AvaloniaProperty.Register<Rate, Geometry?>(nameof(Icon), inherits: true);

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Count), 5);

    public static readonly StyledProperty<double> DefaultValueProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(DefaultValue));

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Value));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Rate, string?>(nameof(Text));

    public static readonly StyledProperty<bool> ShowTextProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(ShowText));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(ItemWidth), 20);

    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(ItemHeight), 20);

    public static readonly StyledProperty<Thickness> ItemMarginProperty =
        AvaloniaProperty.Register<Rate, Thickness>(nameof(ItemMargin));

    public static readonly RoutedEvent<FunctionEventArgs<double>> ValueChangedEvent =
        RoutedEvent.Register<Rate, FunctionEventArgs<double>>(nameof(ValueChanged), RoutingStrategies.Bubble);

    private readonly List<RateItem> _rateItems = [];
    private bool _isLoaded;
    private bool _isUpdatingItems;

    static Rate()
    {
        FocusableProperty.OverrideDefaultValue<Rate>(false);
        CountProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.RebuildItems());
        AllowHalfProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        AllowClearProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        IconProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        IsReadOnlyProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        BackgroundProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        ItemWidthProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        ItemHeightProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        ItemMarginProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        ForegroundProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.UpdateItemProperties());
        DefaultValueProperty.Changed.AddClassHandler<Rate>((rate, _) => rate.ApplyDefaultValue());
        ValueProperty.Changed.AddClassHandler<Rate>((rate, e) => rate.OnValueChanged(e));
    }

    public event EventHandler<FunctionEventArgs<double>> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public double DefaultValue
    {
        get => GetValue(DefaultValueProperty);
        set => SetValue(DefaultValueProperty, value);
    }

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
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

    public Thickness ItemMargin
    {
        get => GetValue(ItemMarginProperty);
        set => SetValue(ItemMarginProperty, value);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        RebuildItems();
        _isLoaded = true;
        ApplyDefaultValue();
        UpdateItems();
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        UpdateItems();
    }

    public void Reset() => Value = DefaultValue;

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var value = CoerceValue(e.GetNewValue<double>());
        if (!value.Equals(Value))
        {
            SetCurrentValue(ValueProperty, value);
            return;
        }

        SetCurrentValue(TextProperty, value.ToString("0.0", CultureInfo.CurrentCulture));
        RaiseEvent(new FunctionEventArgs<double>(ValueChangedEvent, this) { Info = value });
        UpdateItems();
    }

    private double CoerceValue(double value)
    {
        if (double.IsNaN(value))
        {
            return 0;
        }

        var max = Math.Max(0, Count);
        return Math.Clamp(value, 0, max);
    }

    private void ApplyDefaultValue()
    {
        if (!_isLoaded || Value > 0 || DefaultValue <= 0)
        {
            return;
        }

        Value = DefaultValue;
    }

    private void RebuildItems()
    {
        foreach (var item in _rateItems)
        {
            item.SelectedChanged -= RateItemSelectedChanged;
            item.ValueChanged -= RateItemValueChanged;
        }

        _rateItems.Clear();
        Items.Clear();

        for (var i = 1; i <= Count; i++)
        {
            var item = new RateItem { Index = i };
            item.SelectedChanged += RateItemSelectedChanged;
            item.ValueChanged += RateItemValueChanged;
            _rateItems.Add(item);
            Items.Add(item);
        }

        UpdateItemProperties();
        UpdateItems();
    }

    private void UpdateItemProperties()
    {
        foreach (var item in _rateItems)
        {
            item.Width = ItemWidth;
            item.Height = ItemHeight;
            item.Margin = ItemMargin;
            item.AllowHalf = AllowHalf;
            item.AllowClear = AllowClear;
            item.Icon = Icon;
            item.IsReadOnly = IsReadOnly;
            item.Background = Background;
            item.Foreground = Foreground;
        }
    }

    private void RateItemValueChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingItems)
        {
            return;
        }

        Value = _rateItems.Where(item => item.IsSelected).Sum(item => item.IsHalf ? 0.5 : 1);
    }

    private void RateItemSelectedChanged(object? sender, EventArgs e)
    {
        if (sender is not RateItem rateItem)
        {
            return;
        }

        var index = rateItem.Index;
        _isUpdatingItems = true;

        for (var i = 0; i < index - 1 && i < _rateItems.Count; i++)
        {
            _rateItems[i].IsSelected = true;
            _rateItems[i].IsHalf = false;
        }

        for (var i = index; i < _rateItems.Count; i++)
        {
            _rateItems[i].IsSelected = false;
            _rateItems[i].IsHalf = false;
        }

        _isUpdatingItems = false;
    }

    private void UpdateItems()
    {
        if (!_isLoaded)
        {
            return;
        }

        _isUpdatingItems = true;
        var value = CoerceValue(Value);
        var wholeCount = (int)value;

        for (var i = 0; i < wholeCount && i < _rateItems.Count; i++)
        {
            _rateItems[i].IsSelected = true;
            _rateItems[i].IsHalf = false;
        }

        if (value > wholeCount && wholeCount < _rateItems.Count)
        {
            _rateItems[wholeCount].IsSelected = true;
            _rateItems[wholeCount].IsHalf = true;
            wholeCount++;
        }

        for (var i = wholeCount; i < _rateItems.Count; i++)
        {
            _rateItems[i].IsSelected = false;
            _rateItems[i].IsHalf = false;
        }

        _isUpdatingItems = false;
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class Rate : RegularItemsControl
{
    static Rate()
    {
        ItemWidthProperty.OverrideMetadata(typeof(Rate), new PropertyMetadata(ValueBoxes.Double20Box));
        ItemHeightProperty.OverrideMetadata(typeof(Rate), new PropertyMetadata(ValueBoxes.Double20Box));
    }

    public static readonly DependencyProperty AllowHalfProperty = DependencyProperty.Register(
        nameof(AllowHalf), typeof(bool), typeof(Rate), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty AllowClearProperty = DependencyProperty.Register(
        nameof(AllowClear), typeof(bool), typeof(Rate), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(Geometry), typeof(Rate), new FrameworkPropertyMetadata(default(Geometry), FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
        nameof(Count), typeof(int), typeof(Rate), new PropertyMetadata(ValueBoxes.Int5Box));

    public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
        nameof(DefaultValue), typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double0Box));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double0Box, OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((Rate) d).OnValueChanged(new FunctionEventArgs<double>(ValueChangedEvent, d)
        {
            Info = (double) e.NewValue
        });

    protected virtual void OnValueChanged(FunctionEventArgs<double> e)
    {
        RaiseEvent(e);
        UpdateItems();
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(Rate), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
        nameof(ShowText), typeof(bool), typeof(Rate), new PropertyMetadata(ValueBoxes.FalseBox));

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly), typeof(bool), typeof(Rate), new PropertyMetadata(ValueBoxes.FalseBox));

    private bool _isLoaded;

    private bool _updateItems;

    #region Public Events

    public static readonly RoutedEvent ValueChangedEvent =
        EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<double>>), typeof(Rate));

    public event EventHandler<FunctionEventArgs<double>> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    #endregion Public Events

    public Rate()
    {
        AddHandler(RateItem.SelectedChangedEvent, new RoutedEventHandler(RateItemSelectedChanged));
        AddHandler(RateItem.ValueChangedEvent, new RoutedEventHandler(RateItemValueChanged));

        Loaded += (s, e) =>
        {
            if (DesignerHelper.IsInDesignMode) return;

            _updateItems = false;
            OnApplyTemplateInternal();
            _updateItems = true;
            UpdateItems();

            if (_isLoaded) return;
            _isLoaded = true;

            if (Value <= 0)
            {
                if (DefaultValue > 0)
                {
                    Value = DefaultValue;
                }
            }
            else
            {
                UpdateItems();
            }
        };
    }

    public bool AllowHalf
    {
        get => (bool) GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, ValueBoxes.BooleanBox(value));
    }

    public bool AllowClear
    {
        get => (bool) GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, ValueBoxes.BooleanBox(value));
    }

    public Geometry Icon
    {
        get => (Geometry) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public int Count
    {
        get => (int) GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public double DefaultValue
    {
        get => (double) GetValue(DefaultValueProperty);
        set => SetValue(DefaultValueProperty, value);
    }

    public double Value
    {
        get => (double) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowText
    {
        get => (bool) GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, ValueBoxes.BooleanBox(value));
    }

    public bool IsReadOnly
    {
        get => (bool) GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
    }

    private void RateItemValueChanged(object sender, RoutedEventArgs e) =>
        Value = (from RateItem item in Items where item.IsSelected select item.IsHalf ? 0.5 : 1).Sum();

    private void RateItemSelectedChanged(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is RateItem rateItem)
        {
            var index = rateItem.Index;
            for (var i = 0; i < index; i++)
            {
                if (Items[i] is RateItem item)
                {
                    item.IsSelected = true;
                    item.IsHalf = false;
                }
            }

            for (var i = index; i < Count; i++)
            {
                if (Items[i] is RateItem item)
                {
                    item.IsSelected = false;
                    item.IsHalf = false;
                }
            }
        }
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is RateItem;

    protected override DependencyObject GetContainerForItemOverride() => new RateItem();

    private void OnApplyTemplateInternal()
    {
        Items.Clear();

        for (var i = 1; i <= Count; i++)
        {
            var item = new RateItem
            {
                Index = i,
                Width = ItemWidth,
                Height = ItemHeight,
                Margin = ItemMargin,
                AllowHalf = AllowHalf,
                AllowClear = AllowClear,
                Icon = Icon,
                IsReadOnly = IsReadOnly,
                Background = Background
            };

            Items.Add(item);
        }
    }

    public override void OnApplyTemplate()
    {
        if (!_isLoaded)
        {
            _updateItems = true;
            OnApplyTemplateInternal();
            _updateItems = false;
        }

        base.OnApplyTemplate();
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);
        UpdateItems();
    }

    protected override void UpdateItems()
    {
        if (!_isLoaded || !_updateItems) return;
        var count = (int) Value;

        for (var i = 0; i < count; i++)
        {
            if (Items[i] is RateItem rateItem)
            {
                rateItem.IsSelected = true;
                rateItem.IsHalf = false;
            }
        }

        if (Value > count)
        {
            if (Items[count] is RateItem rateItem)
            {
                rateItem.IsSelected = true;
                rateItem.IsHalf = true;
            }

            count += 1;
        }

        for (var i = count; i < Count; i++)
        {
            if (Items[i] is RateItem rateItem)
            {
                rateItem.IsSelected = false;
                rateItem.IsHalf = false;
            }
        }
    }

    public void Reset() => Value = DefaultValue;
}

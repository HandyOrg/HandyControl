using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls;

/// <summary>
///     标记
/// </summary>
public class Badge : ContentControl
{
    private int? _originalValue;

    public static readonly RoutedEvent ValueChangedEvent =
        EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<int>>), typeof(Badge));

    public event EventHandler<FunctionEventArgs<int>> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(Badge), new PropertyMetadata("0"));

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int0Box, OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Badge) d;
        var v = (int) e.NewValue;
        ctl.SetCurrentValue(TextProperty, v <= ctl.Maximum ? v.ToString() : $"{ctl.Maximum}+");
        if (ctl.IsLoaded)
        {
            ctl.RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, ctl)
            {
                Info = v
            });
        }
        else
        {
            ctl._originalValue = v;
        }
    }

    public int Value
    {
        get => (int) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
        nameof(Status), typeof(BadgeStatus), typeof(Badge), new PropertyMetadata(default(BadgeStatus)));

    public BadgeStatus Status
    {
        get => (BadgeStatus) GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int99Box, OnMaximumChanged));

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Badge) d;
        var v = ctl.Value;
        ctl.SetCurrentValue(TextProperty, v <= ctl.Maximum ? v.ToString() : $"{ctl.Maximum}+");
    }

    public int Maximum
    {
        get => (int) GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty BadgeMarginProperty = DependencyProperty.Register(
        nameof(BadgeMargin), typeof(Thickness), typeof(Badge), new PropertyMetadata(default(Thickness)));

    public Thickness BadgeMargin
    {
        get => (Thickness) GetValue(BadgeMarginProperty);
        set => SetValue(BadgeMarginProperty, value);
    }

    public static readonly DependencyProperty ShowBadgeProperty = DependencyProperty.Register(
        nameof(ShowBadge), typeof(bool), typeof(Badge), new PropertyMetadata(ValueBoxes.TrueBox));

    public bool ShowBadge
    {
        get => (bool) GetValue(ShowBadgeProperty);
        set => SetValue(ShowBadgeProperty, ValueBoxes.BooleanBox(value));
    }

    protected override Geometry GetLayoutClip(Size layoutSlotSize) => null;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_originalValue != null)
        {
            RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, this)
            {
                Info = _originalValue.Value
            });

            _originalValue = null;
        }
    }
}

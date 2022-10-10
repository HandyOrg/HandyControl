// Adapted from https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Primitives/RangeBase.cs

using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class TwoWayRangeBase : Control
{
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum), typeof(double), typeof(TwoWayRangeBase),
        new PropertyMetadata(ValueBoxes.Double0Box, OnMinimumChanged), ValidateHelper.IsInRangeOfDouble);

    private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TwoWayRangeBase) d;

        ctrl.CoerceValue(MaximumProperty);
        ctrl.CoerceValue(ValueStartProperty);
        ctrl.CoerceValue(ValueEndProperty);
        ctrl.OnMinimumChanged((double) e.OldValue, (double) e.NewValue);
    }

    protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
    }

    public double Minimum
    {
        get => (double) GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(double), typeof(TwoWayRangeBase),
        new PropertyMetadata(ValueBoxes.Double10Box, OnMaximumChanged, CoerceMaximum),
        ValidateHelper.IsInRangeOfDouble);

    private static object CoerceMaximum(DependencyObject d, object basevalue)
    {
        var ctrl = (TwoWayRangeBase) d;
        var min = ctrl.Minimum;
        if ((double) basevalue < min)
        {
            return min;
        }
        return basevalue;
    }

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TwoWayRangeBase) d;

        ctrl.CoerceValue(ValueStartProperty);
        ctrl.CoerceValue(ValueEndProperty);
        ctrl.OnMaximumChanged((double) e.OldValue, (double) e.NewValue);
    }

    protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
    }

    public double Maximum
    {
        get => (double) GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty ValueStartProperty = DependencyProperty.Register(
        nameof(ValueStart), typeof(double), typeof(TwoWayRangeBase),
        new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
            OnValueStartChanged, ConstrainToRange), ValidateHelper.IsInRangeOfDouble);

    private static void OnValueStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TwoWayRangeBase) d;
        ctrl.OnValueChanged(new DoubleRange
        {
            Start = (double) e.OldValue,
            End = ctrl.ValueEnd
        }, new DoubleRange
        {
            Start = (double) e.NewValue,
            End = ctrl.ValueEnd
        });
    }

    protected virtual void OnValueChanged(DoubleRange oldValue, DoubleRange newValue) => RaiseEvent(
        new RoutedPropertyChangedEventArgs<DoubleRange>(oldValue, newValue) { RoutedEvent = ValueChangedEvent });

    public double ValueStart
    {
        get => (double) GetValue(ValueStartProperty);
        set => SetValue(ValueStartProperty, value);
    }

    public static readonly DependencyProperty ValueEndProperty = DependencyProperty.Register(
        nameof(ValueEnd), typeof(double), typeof(TwoWayRangeBase),
        new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
            OnValueEndChanged, ConstrainToRange), ValidateHelper.IsInRangeOfDouble);

    private static void OnValueEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TwoWayRangeBase) d;
        ctrl.OnValueChanged(new DoubleRange
        {
            Start = ctrl.ValueStart,
            End = (double) e.OldValue
        }, new DoubleRange
        {
            Start = ctrl.ValueStart,
            End = (double) e.NewValue
        });
    }

    public double ValueEnd
    {
        get => (double) GetValue(ValueEndProperty);
        set => SetValue(ValueEndProperty, value);
    }

    internal static object ConstrainToRange(DependencyObject d, object value)
    {
        var ctrl = (TwoWayRangeBase) d;
        var min = ctrl.Minimum;
        var v = (double) value;
        if (v < min)
        {
            return min;
        }

        var max = ctrl.Maximum;
        if (v > max)
        {
            return max;
        }

        return value;
    }

    public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
        nameof(LargeChange), typeof(double), typeof(TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double1Box),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    public double LargeChange
    {
        get => (double) GetValue(LargeChangeProperty);
        set => SetValue(LargeChangeProperty, value);
    }

    public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
        nameof(SmallChange), typeof(double), typeof(TwoWayRangeBase), new PropertyMetadata(ValueBoxes.Double01Box),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    public double SmallChange
    {
        get => (double) GetValue(SmallChangeProperty);
        set => SetValue(SmallChangeProperty, value);
    }

    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
        RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DoubleRange>), typeof(TwoWayRangeBase));

    public event RoutedPropertyChangedEventHandler<DoubleRange> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }
}

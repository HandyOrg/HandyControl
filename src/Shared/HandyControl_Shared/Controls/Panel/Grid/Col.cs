using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Col : ContentControl
{
    public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
        nameof(Layout), typeof(ColLayout), typeof(Col), new FrameworkPropertyMetadata(default(ColLayout), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public ColLayout Layout
    {
        get => (ColLayout) GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset), typeof(int), typeof(Col), new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public int Offset
    {
        get => (int) GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly DependencyProperty SpanProperty = DependencyProperty.Register(
        nameof(Span), typeof(int), typeof(Col), new FrameworkPropertyMetadata(ColLayout.ColMaxCellCount, FrameworkPropertyMetadataOptions.AffectsParentMeasure), OnSpanValidate);

    private static bool OnSpanValidate(object value)
    {
        var v = (int) value;
        return v is >= 1 and <= ColLayout.ColMaxCellCount;
    }

    public int Span
    {
        get => (int) GetValue(SpanProperty);
        set => SetValue(SpanProperty, value);
    }

    public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register(
        nameof(IsFixed), typeof(bool), typeof(Col), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public bool IsFixed
    {
        get => (bool) GetValue(IsFixedProperty);
        set => SetValue(IsFixedProperty, ValueBoxes.BooleanBox(value));
    }

    internal int GetLayoutCellCount(ColLayoutStatus status)
    {
        if (Layout is not null)
        {
            return status switch
            {
                ColLayoutStatus.Xs => Layout.Xs,
                ColLayoutStatus.Sm => Layout.Sm,
                ColLayoutStatus.Md => Layout.Md,
                ColLayoutStatus.Lg => Layout.Lg,
                ColLayoutStatus.Xl => Layout.Xl,
                ColLayoutStatus.Xxl => Layout.Xxl,
                ColLayoutStatus.Auto => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
            };
        }

        if (IsFixed)
        {
            return 0;
        }

        return Span;
    }
}

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Col : ContentControl
{
    public static readonly StyledProperty<ColLayout?> LayoutProperty =
        AvaloniaProperty.Register<Col, ColLayout?>(nameof(Layout));

    public ColLayout? Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public static readonly StyledProperty<int> OffsetProperty =
        AvaloniaProperty.Register<Col, int>(nameof(Offset));

    public int Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly StyledProperty<int> SpanProperty =
        AvaloniaProperty.Register<Col, int>(nameof(Span), defaultValue: ColLayout.ColMaxCellCount,
            validate: v => v is >= 1 and <= ColLayout.ColMaxCellCount);

    public int Span
    {
        get => GetValue(SpanProperty);
        set => SetValue(SpanProperty, value);
    }

    public static readonly StyledProperty<bool> IsFixedProperty =
        AvaloniaProperty.Register<Col, bool>(nameof(IsFixed));

    public bool IsFixed
    {
        get => GetValue(IsFixedProperty);
        set => SetValue(IsFixedProperty, value);
    }

    static Col()
    {
        LayoutProperty.Changed.AddClassHandler<Col>(InvalidateParentMeasure);
        OffsetProperty.Changed.AddClassHandler<Col>(InvalidateParentMeasure);
        SpanProperty.Changed.AddClassHandler<Col>(InvalidateParentMeasure);
        IsFixedProperty.Changed.AddClassHandler<Col>(InvalidateParentMeasure);
    }

    private static void InvalidateParentMeasure(Col col, AvaloniaPropertyChangedEventArgs e)
    {
        if (col.GetVisualParent() is Control parent)
        {
            parent.InvalidateMeasure();
        }
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

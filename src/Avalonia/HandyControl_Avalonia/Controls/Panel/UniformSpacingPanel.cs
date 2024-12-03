using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class UniformSpacingPanel : Panel
{
    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<UniformSpacingPanel>(new StyledPropertyMetadata<Orientation>(
            defaultValue: Orientation.Horizontal));

    public static readonly StyledProperty<VisualWrapping> ChildWrappingProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, VisualWrapping>(nameof(ChildWrapping));

    public static readonly StyledProperty<double> SpacingProperty =
        StackPanel.SpacingProperty.AddOwner<UniformSpacingPanel>(
            new StyledPropertyMetadata<double>(defaultValue: double.NaN, coerce: CoerceLength));

    public static readonly StyledProperty<double> HorizontalSpacingProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, double>(nameof(HorizontalSpacing), defaultValue: double.NaN,
            coerce: CoerceLength);

    public static readonly StyledProperty<double> VerticalSpacingProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, double>(nameof(VerticalSpacing), defaultValue: double.NaN,
            coerce: CoerceLength);

    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, double>(nameof(ItemWidth), defaultValue: double.NaN,
            coerce: CoerceLength);

    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, double>(nameof(ItemHeight), defaultValue: double.NaN,
            coerce: CoerceLength);

    public static readonly StyledProperty<HorizontalAlignment?> ItemHorizontalAlignmentProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, HorizontalAlignment?>(nameof(ItemHorizontalAlignment),
            defaultValue: HorizontalAlignment.Stretch);

    public static readonly StyledProperty<VerticalAlignment?> ItemVerticalAlignmentProperty =
        AvaloniaProperty.Register<UniformSpacingPanel, VerticalAlignment?>(nameof(ItemVerticalAlignment),
            defaultValue: VerticalAlignment.Stretch);

    private Orientation _orientation;

    static UniformSpacingPanel()
    {
        AffectsMeasure<StackPanel>(
            OrientationProperty,
            ChildWrappingProperty,
            SpacingProperty,
            HorizontalSpacingProperty,
            VerticalSpacingProperty,
            ItemWidthProperty,
            ItemHeightProperty,
            ItemHorizontalAlignmentProperty,
            ItemVerticalAlignmentProperty
        );

        OrientationProperty.Changed.AddClassHandler<UniformSpacingPanel>(OnOrientationChanged);
    }

    private static void OnOrientationChanged(AvaloniaObject element, AvaloniaPropertyChangedEventArgs e)
    {
        var p = (UniformSpacingPanel)element;
        p._orientation = e.GetNewValue<Orientation>();
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public VisualWrapping ChildWrapping
    {
        get => GetValue(ChildWrappingProperty);
        set => SetValue(ChildWrappingProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public double HorizontalSpacing
    {
        get => GetValue(HorizontalSpacingProperty);
        set => SetValue(HorizontalSpacingProperty, value);
    }

    public double VerticalSpacing
    {
        get => GetValue(VerticalSpacingProperty);
        set => SetValue(VerticalSpacingProperty, value);
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

    public HorizontalAlignment? ItemHorizontalAlignment
    {
        get => GetValue(ItemHorizontalAlignmentProperty);
        set => SetValue(ItemHorizontalAlignmentProperty, value);
    }

    public VerticalAlignment? ItemVerticalAlignment
    {
        get => GetValue(ItemVerticalAlignmentProperty);
        set => SetValue(ItemVerticalAlignmentProperty, value);
    }

    private static double CoerceLength(AvaloniaObject _, double length) => length < 0 ? 0 : length;

    protected override Size MeasureOverride(Size availableSize)
    {
        var curLineSize = new PanelUvSize(_orientation);
        var panelSize = new PanelUvSize(_orientation);
        var uvConstraint = new PanelUvSize(_orientation, availableSize);
        double itemWidth = ItemWidth;
        double itemHeight = ItemHeight;
        bool itemWidthSet = !double.IsNaN(itemWidth);
        bool itemHeightSet = !double.IsNaN(itemHeight);
        var itemHorizontalAlignment = ItemHorizontalAlignment;
        var itemVerticalAlignment = ItemVerticalAlignment;
        bool itemHorizontalAlignmentSet = itemHorizontalAlignment != null;
        bool itemVerticalAlignmentSet = itemVerticalAlignment != null;
        var childWrapping = ChildWrapping;
        var spacingSize = GetSpacingSize();

        var childConstraint = new Size(
            itemWidthSet ? itemWidth : availableSize.Width,
            itemHeightSet ? itemHeight : availableSize.Height);

        bool isFirst = true;

        if (childWrapping == VisualWrapping.NoWrap)
        {
            var layoutSlotSize = new PanelUvSize(_orientation, availableSize);

            if (_orientation == Orientation.Horizontal)
            {
                layoutSlotSize.V = double.PositiveInfinity;
            }
            else
            {
                layoutSlotSize.U = double.PositiveInfinity;
            }

            for (int i = 0, count = Children.Count; i < count; ++i)
            {
                var child = Children[i];

                if (itemHorizontalAlignmentSet)
                {
                    child.SetCurrentValue(HorizontalAlignmentProperty, itemHorizontalAlignment);
                }

                if (itemVerticalAlignmentSet)
                {
                    child.SetCurrentValue(VerticalAlignmentProperty, itemVerticalAlignment);
                }

                child.Measure(new Size(layoutSlotSize.Width, layoutSlotSize.Height));

                var sz = new PanelUvSize(
                    _orientation,
                    itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    itemHeightSet ? itemHeight : child.DesiredSize.Height);

                curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
                curLineSize.V = Math.Max(sz.V, curLineSize.V);

                isFirst = false;
            }
        }
        else
        {
            for (int i = 0, count = Children.Count; i < count; i++)
            {
                var child = Children[i];

                if (itemHorizontalAlignmentSet)
                {
                    child.SetCurrentValue(HorizontalAlignmentProperty, itemHorizontalAlignment);
                }

                if (itemVerticalAlignmentSet)
                {
                    child.SetCurrentValue(VerticalAlignmentProperty, itemVerticalAlignment);
                }

                child.Measure(childConstraint);

                var sz = new PanelUvSize(
                    _orientation,
                    itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (!isFirst && MathHelper.GreaterThan(curLineSize.U + sz.U + spacingSize.U, uvConstraint.U))
                {
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V + spacingSize.V;
                    curLineSize = sz;
                }
                else
                {
                    curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }

                isFirst = false;
            }
        }

        panelSize.U = Math.Max(curLineSize.U, panelSize.U);
        panelSize.V += curLineSize.V;

        return new Size(panelSize.Width, panelSize.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        int firstInLine = 0;
        double itemWidth = ItemWidth;
        double itemHeight = ItemHeight;
        double accumulatedV = 0;
        double itemU = _orientation == Orientation.Horizontal ? itemWidth : itemHeight;
        var curLineSize = new PanelUvSize(_orientation);
        var uvFinalSize = new PanelUvSize(_orientation, finalSize);
        bool itemWidthSet = !double.IsNaN(itemWidth);
        bool itemHeightSet = !double.IsNaN(itemHeight);
        bool useItemU = _orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet;
        var childWrapping = ChildWrapping;
        var spacingSize = GetSpacingSize();

        bool isFirst = true;

        if (childWrapping == VisualWrapping.NoWrap)
        {
            ArrangeLine(uvFinalSize.V, useItemU, itemU, spacingSize.U);
        }
        else
        {
            for (int i = 0, count = Children.Count; i < count; i++)
            {
                var child = Children[i];

                var sz = new PanelUvSize(
                    _orientation,
                    itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (!isFirst && MathHelper.GreaterThan(curLineSize.U + sz.U + spacingSize.U, uvFinalSize.U))
                {
                    ArrangeWrapLine(accumulatedV, curLineSize.V, firstInLine, i, useItemU, itemU, spacingSize.U);

                    accumulatedV += curLineSize.V + spacingSize.V;
                    curLineSize = sz;

                    firstInLine = i;
                }
                else
                {
                    curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }

                isFirst = false;
            }

            if (firstInLine < Children.Count)
            {
                ArrangeWrapLine(accumulatedV, curLineSize.V, firstInLine, Children.Count, useItemU, itemU,
                    spacingSize.U);
            }
        }

        return finalSize;
    }

    private PanelUvSize GetSpacingSize()
    {
        var orientation = Orientation;
        double spacing = Spacing;

        if (!double.IsNaN(spacing))
        {
            return new PanelUvSize(orientation, spacing, spacing);
        }

        double horizontalSpacing = HorizontalSpacing;
        if (double.IsNaN(horizontalSpacing))
        {
            horizontalSpacing = 0;
        }

        double verticalSpacing = VerticalSpacing;
        if (double.IsNaN(verticalSpacing))
        {
            verticalSpacing = 0;
        }

        return new PanelUvSize(orientation, horizontalSpacing, verticalSpacing);
    }

    private void ArrangeLine(double lineV, bool useItemU, double itemU, double spacing)
    {
        var orientation = Orientation;
        double u = 0;
        bool isHorizontal = orientation == Orientation.Horizontal;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < Children.Count; i++)
        {
            Control child = Children[i];
            var childSize = new PanelUvSize(orientation, child.DesiredSize);
            double layoutSlotU = useItemU ? itemU : childSize.U;

            child.Arrange(isHorizontal ? new Rect(u, 0, layoutSlotU, lineV) : new Rect(0, u, lineV, layoutSlotU));

            if (layoutSlotU > 0)
            {
                u += layoutSlotU + spacing;
            }
        }
    }

    private void ArrangeWrapLine(
        double v,
        double lineV,
        int start,
        int end,
        bool useItemU,
        double itemU,
        double spacing)
    {
        double u = 0;
        bool isHorizontal = _orientation == Orientation.Horizontal;

        for (int i = start; i < end; i++)
        {
            var child = Children[i];

            var childSize = new PanelUvSize(_orientation, child.DesiredSize);
            double layoutSlotU = useItemU ? itemU : childSize.U;

            child.Arrange(isHorizontal ? new Rect(u, v, layoutSlotU, lineV) : new Rect(v, u, lineV, layoutSlotU));

            if (layoutSlotU > 0)
            {
                u += layoutSlotU + spacing;
            }
        }
    }
}

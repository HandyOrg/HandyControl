using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Row : Panel
{
    private ColLayoutStatus _layoutStatus;

    private double _maxChildDesiredHeight;

    private double _fixedWidth;

    public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
        nameof(Gutter), typeof(double), typeof(Row), new FrameworkPropertyMetadata(
            ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure, null, OnGutterCoerce),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    private static object OnGutterCoerce(DependencyObject d, object basevalue) =>
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero(basevalue) ? basevalue : .0;

    public double Gutter
    {
        get => (double) GetValue(GutterProperty);
        set => SetValue(GutterProperty, value);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var gutter = Gutter;
        var totalCellCount = 0;
        var totalRowCount = 1;
        _fixedWidth = 0;
        _maxChildDesiredHeight = 0;
        var cols = InternalChildren.OfType<Col>().ToList();

        foreach (var child in cols)
        {
            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            if (cellCount == 0 || child.IsFixed)
            {
                child.Measure(constraint);
                _maxChildDesiredHeight = Math.Max(_maxChildDesiredHeight, child.DesiredSize.Height);
                _fixedWidth += child.DesiredSize.Width + gutter;
            }
        }

        var itemWidth = (constraint.Width - _fixedWidth + gutter) / ColLayout.ColMaxCellCount;
        itemWidth = Math.Max(0, itemWidth);

        foreach (var child in cols)
        {
            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            if (cellCount > 0 && !child.IsFixed)
            {
                totalCellCount += cellCount;
                var availableWidth = Math.Max(0, cellCount * itemWidth - gutter);

                child.Measure(new Size(availableWidth, constraint.Height));
                _maxChildDesiredHeight = Math.Max(_maxChildDesiredHeight, child.DesiredSize.Height);

                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    totalCellCount = cellCount;
                    totalRowCount++;
                }
            }
        }

        return new Size(0, _maxChildDesiredHeight * totalRowCount);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var gutter = Gutter;
        var totalCellCount = 0;
        var cols = InternalChildren.OfType<Col>().ToList();
        var itemWidth = (finalSize.Width - _fixedWidth + gutter) / ColLayout.ColMaxCellCount;
        itemWidth = Math.Max(0, itemWidth);

        var childBounds = new Rect(0, 0, 0, _maxChildDesiredHeight);
        _layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);

        foreach (var child in cols)
        {
            if (!child.IsVisible)
            {
                continue;
            }

            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            totalCellCount += cellCount;

            var childWidth = (cellCount > 0 && !child.IsFixed) ? Math.Max(0, cellCount * itemWidth - gutter) : child.DesiredSize.Width;
            childBounds.Width = childWidth;
            childBounds.X += child.Offset * itemWidth;

            if (totalCellCount > ColLayout.ColMaxCellCount)
            {
                childBounds.X = 0;
                childBounds.Y += _maxChildDesiredHeight;
                totalCellCount = cellCount;
            }

            child.Arrange(childBounds);
            childBounds.X += childWidth + gutter;
        }

        return finalSize;
    }
}

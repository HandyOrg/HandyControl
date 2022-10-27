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

    private double _totalAutoWidth;

    public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
        nameof(Gutter), typeof(double), typeof(Row), new PropertyMetadata(ValueBoxes.Double0Box, null, OnGutterCoerce), ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    private static object OnGutterCoerce(DependencyObject d, object basevalue) => ValidateHelper.IsInRangeOfPosDoubleIncludeZero(basevalue) ? basevalue : .0;

    public double Gutter
    {
        get => (double) GetValue(GutterProperty);
        set => SetValue(GutterProperty, value);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var totalCellCount = 0;
        var totalRowCount = 1;
        var gutterHalf = Gutter / 2;
        _totalAutoWidth = 0;

        foreach (var child in InternalChildren.OfType<Col>())
        {
            child.Margin = new Thickness(gutterHalf);
            child.Measure(constraint);
            var childDesiredSize = child.DesiredSize;

            if (_maxChildDesiredHeight < childDesiredSize.Height)
            {
                _maxChildDesiredHeight = childDesiredSize.Height;
            }

            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            totalCellCount += cellCount;

            if (totalCellCount > ColLayout.ColMaxCellCount)
            {
                totalCellCount = cellCount;
                totalRowCount++;
            }

            if (cellCount == 0 || child.IsFixed)
            {
                _totalAutoWidth += childDesiredSize.Width;
            }
        }

        return new Size(0, _maxChildDesiredHeight * totalRowCount - Gutter);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var totalCellCount = 0;
        var gutterHalf = Gutter / 2;
        var itemWidth = (finalSize.Width - _totalAutoWidth + Gutter) / ColLayout.ColMaxCellCount;
        itemWidth = Math.Max(0, itemWidth);

        var childBounds = new Rect(-gutterHalf, -gutterHalf, 0, _maxChildDesiredHeight);
        _layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);

        foreach (var child in InternalChildren.OfType<Col>())
        {
            if (!child.IsVisible)
            {
                continue;
            }

            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            totalCellCount += cellCount;

            var childWidth = cellCount > 0 ? cellCount * itemWidth : child.DesiredSize.Width;

            childBounds.Width = childWidth;
            childBounds.X += child.Offset * itemWidth;
            if (totalCellCount > ColLayout.ColMaxCellCount)
            {
                childBounds.X = -gutterHalf;
                childBounds.Y += _maxChildDesiredHeight;
                totalCellCount = cellCount;
            }

            child.Arrange(childBounds);
            childBounds.X += childWidth;
        }

        return finalSize;
    }
}

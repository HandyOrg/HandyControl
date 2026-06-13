using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Row : Panel
{
    private ColLayoutStatus _layoutStatus;

    private double _maxChildDesiredHeight;

    private double _fixedWidth;

    public static readonly StyledProperty<double> GutterProperty =
        AvaloniaProperty.Register<Row, double>(nameof(Gutter), coerce: CoerceGutter);

    private static double CoerceGutter(AvaloniaObject d, double value)
        => double.IsNaN(value) || value < 0 ? 0 : value;

    public double Gutter
    {
        get => GetValue(GutterProperty);
        set => SetValue(GutterProperty, value);
    }

    static Row()
    {
        AffectsMeasure<Row>(GutterProperty);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var gutter = Gutter;
        var totalCellCount = 0;
        var totalRowCount = 1;
        _fixedWidth = 0;
        _maxChildDesiredHeight = 0;
        var cols = Children.OfType<Col>().ToList();

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
        var cols = Children.OfType<Col>().ToList();
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
            childBounds = childBounds.WithWidth(childWidth);
            childBounds = childBounds.WithX(childBounds.X + child.Offset * itemWidth);

            if (totalCellCount > ColLayout.ColMaxCellCount)
            {
                childBounds = childBounds.WithX(0);
                childBounds = childBounds.WithY(childBounds.Y + _maxChildDesiredHeight);
                totalCellCount = cellCount;
            }

            child.Arrange(childBounds);
            childBounds = childBounds.WithX(childBounds.X + childWidth + gutter);
        }

        return finalSize;
    }
}

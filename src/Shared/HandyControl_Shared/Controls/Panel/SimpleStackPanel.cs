using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

public class SimpleStackPanel : Panel
{
    public static readonly DependencyProperty OrientationProperty =
        StackPanel.OrientationProperty.AddOwner(typeof(SimpleStackPanel),
            new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var stackDesiredSize = new Size();
        var layoutSlotSize = constraint;

        if (Orientation == Orientation.Horizontal)
        {
            layoutSlotSize.Width = double.PositiveInfinity;

            foreach (var child in GetVisibleChildren())
            {
                child.Measure(layoutSlotSize);
                var childDesiredSize = child.DesiredSize;

                stackDesiredSize.Width += childDesiredSize.Width;
                stackDesiredSize.Height = Math.Max(stackDesiredSize.Height, childDesiredSize.Height);
            }
        }
        else
        {
            layoutSlotSize.Height = double.PositiveInfinity;

            foreach (var child in GetVisibleChildren())
            {
                child.Measure(layoutSlotSize);
                var childDesiredSize = child.DesiredSize;

                stackDesiredSize.Width = Math.Max(stackDesiredSize.Width, childDesiredSize.Width);
                stackDesiredSize.Height += childDesiredSize.Height;
            }
        }

        return stackDesiredSize;
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        var rcChild = new Rect(arrangeSize);
        var previousChildSize = 0.0;

        if (Orientation == Orientation.Horizontal)
        {
            foreach (var child in GetVisibleChildren())
            {
                rcChild.X += previousChildSize;
                previousChildSize = child.DesiredSize.Width;
                rcChild.Width = previousChildSize;
                rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);

                child.Arrange(rcChild);
            }
        }
        else
        {
            foreach (var child in GetVisibleChildren())
            {
                rcChild.Y += previousChildSize;
                previousChildSize = child.DesiredSize.Height;
                rcChild.Height = previousChildSize;
                rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

                child.Arrange(rcChild);
            }
        }

        return arrangeSize;
    }

    public virtual IEnumerable<UIElement> GetVisibleChildren()
    {
        var children = InternalChildren;

        for (int i = 0, count = children.Count; i < count; ++i)
        {
            var child = children[i];
            if (child == null)
            {
                continue;
            }

            if (child.Visibility != Visibility.Collapsed)
            {
                yield return child;
            }
        }
    }
}

using System;
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
        var children = InternalChildren;
        var layoutSlotSize = constraint;

        if (Orientation == Orientation.Horizontal)
        {
            layoutSlotSize.Width = double.PositiveInfinity;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null) continue;

                child.Measure(layoutSlotSize);
                var childDesiredSize = child.DesiredSize;

                stackDesiredSize.Width += childDesiredSize.Width;
                stackDesiredSize.Height = Math.Max(stackDesiredSize.Height, childDesiredSize.Height);
            }
        }
        else
        {
            layoutSlotSize.Height = double.PositiveInfinity;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null) continue;

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
        var children = InternalChildren;
        var rcChild = new Rect(arrangeSize);
        var previousChildSize = 0.0;

        if (Orientation == Orientation.Horizontal)
        {
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null) continue;

                rcChild.X += previousChildSize;
                previousChildSize = child.DesiredSize.Width;
                rcChild.Width = previousChildSize;
                rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);

                child.Arrange(rcChild);
            }
        }
        else
        {
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null) continue;

                rcChild.Y += previousChildSize;
                previousChildSize = child.DesiredSize.Height;
                rcChild.Height = previousChildSize;
                rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

                child.Arrange(rcChild);
            }
        }

        return arrangeSize;
    }
}

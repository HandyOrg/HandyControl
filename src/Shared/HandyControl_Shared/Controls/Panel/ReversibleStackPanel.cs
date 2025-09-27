using System;
using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ReversibleStackPanel : SimpleStackPanel
{
    public static readonly DependencyProperty ReverseOrderProperty = DependencyProperty.Register(
        nameof(ReverseOrder), typeof(bool), typeof(ReversibleStackPanel),
        new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public bool ReverseOrder
    {
        get => (bool) GetValue(ReverseOrderProperty);
        set => SetValue(ReverseOrderProperty, ValueBoxes.BooleanBox(value));
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        if (!ReverseOrder)
        {
            return base.ArrangeOverride(arrangeSize);
        }

        var rcChild = new Rect(arrangeSize);
        double previousChildSize;

        if (Orientation == System.Windows.Controls.Orientation.Horizontal)
        {
            rcChild.X = arrangeSize.Width;

            foreach (var child in GetVisibleChildren())
            {
                previousChildSize = child.DesiredSize.Width;
                rcChild.X -= previousChildSize;
                rcChild.Width = previousChildSize;
                rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);

                child.Arrange(rcChild);
            }
        }
        else
        {
            rcChild.Y = arrangeSize.Height;

            foreach (var child in GetVisibleChildren())
            {
                previousChildSize = child.DesiredSize.Height;
                rcChild.Y -= previousChildSize;
                rcChild.Height = previousChildSize;
                rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

                child.Arrange(rcChild);
            }
        }

        return arrangeSize;
    }
}

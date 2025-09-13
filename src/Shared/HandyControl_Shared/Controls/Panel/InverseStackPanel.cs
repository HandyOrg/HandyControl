using System;
using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class InverseStackPanel : SimpleStackPanel
{
    public static readonly DependencyProperty IsInverseEnabledProperty = DependencyProperty.Register(
        nameof(IsInversed), typeof(bool), typeof(InverseStackPanel),
        new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public bool IsInversed
    {
        get => (bool) GetValue(IsInverseEnabledProperty);
        set => SetValue(IsInverseEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        if (!IsInversed)
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

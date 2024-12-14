using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace HandyControl.Controls;

public class AxleCanvas : Canvas
{
    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<AxleCanvas>(new StyledPropertyMetadata<Orientation>(
            defaultValue: Orientation.Horizontal));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (Control? internalChild in Children)
        {
            if (internalChild == null)
            {
                continue;
            }

            double x = 0.0;
            double y = 0.0;

            if (Orientation == Orientation.Horizontal)
            {
                x = (finalSize.Width - internalChild.DesiredSize.Width) / 2;

                double top = GetTop(internalChild);
                if (!double.IsNaN(top))
                {
                    y = top;
                }
                else
                {
                    double bottom = GetBottom(internalChild);
                    if (!double.IsNaN(bottom))
                    {
                        y = finalSize.Height - internalChild.DesiredSize.Height - bottom;
                    }
                }
            }
            else
            {
                y = (finalSize.Height - internalChild.DesiredSize.Height) / 2;

                double left = GetLeft(internalChild);
                if (!double.IsNaN(left))
                {
                    x = left;
                }
                else
                {
                    double right = GetRight(internalChild);
                    if (!double.IsNaN(right))
                    {
                        x = finalSize.Width - internalChild.DesiredSize.Width - right;
                    }
                }
            }

            internalChild.Arrange(new Rect(new Point(x, y), internalChild.DesiredSize));
        }

        return finalSize;
    }
}

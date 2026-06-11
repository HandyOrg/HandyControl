using System;
using Avalonia;
using Avalonia.Controls;

namespace HandyControl.Controls;

/// <summary>
///     用以代替Grid
/// </summary>
/// <remarks>
///     当不需要Grid的行、列分隔等功能时建议用此轻量级类代替
/// </remarks>
public class SimplePanel : Panel
{
    protected override Size MeasureOverride(Size constraint)
    {
        var maxWidth = 0d;
        var maxHeight = 0d;
        foreach (Control? child in Children)
        {
            if (child != null)
            {
                child.Measure(constraint);
                maxWidth = Math.Max(maxWidth, child.DesiredSize.Width);
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }
        }

        return new Size(maxWidth, maxHeight);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        foreach (Control? child in Children)
        {
            child?.Arrange(new Rect(arrangeSize));
        }

        return arrangeSize;
    }
}

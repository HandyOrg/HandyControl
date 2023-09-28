using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class WaterfallPanel : Panel
{
    public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
        nameof(Groups), typeof(int), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
            ValueBoxes.Int2Box, FrameworkPropertyMetadataOptions.AffectsMeasure), IsGroupsValid);

    public int Groups
    {
        get => (int) GetValue(GroupsProperty);
        set => SetValue(GroupsProperty, value);
    }

    private static bool IsGroupsValid(object value) => (int) value >= 1;

    public static readonly DependencyProperty AutoGroupProperty = DependencyProperty.Register(
        nameof(AutoGroup), typeof(bool), typeof(WaterfallPanel), new FrameworkPropertyMetadata(
            ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public bool AutoGroup
    {
        get => (bool) GetValue(AutoGroupProperty);
        set => SetValue(AutoGroupProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty DesiredLengthProperty = DependencyProperty.Register(
        nameof(DesiredLength), typeof(double), typeof(WaterfallPanel), new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure), ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    public double DesiredLength
    {
        get => (double) GetValue(DesiredLengthProperty);
        set => SetValue(DesiredLengthProperty, value);
    }

    public static readonly DependencyProperty OrientationProperty =
        StackPanel.OrientationProperty.AddOwner(typeof(WaterfallPanel),
            new FrameworkPropertyMetadata(Orientation.Horizontal,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    private int CaculateGroupCount(Orientation orientation, PanelUvSize size)
    {
        if (!AutoGroup)
        {
            return Groups;
        }

        var itemLength = DesiredLength;

        if (MathHelper.IsVerySmall(itemLength))
        {
            return Groups;
        }

        return (int) (size.U / itemLength);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, finalSize);

        var groups = CaculateGroupCount(orientation, uvConstraint);
        if (groups < 1)
        {
            return finalSize;
        }

        var vArr = new double[groups].ToList();
        var itemU = uvConstraint.U / groups;

        var children = InternalChildren;
        for (int i = 0, count = children.Count; i < count; i++)
        {
            var child = children[i];
            if (child == null)
            {
                continue;
            }

            var minIndex = vArr.IndexOf(vArr.Min());
            var minV = vArr[minIndex];
            var childUvSize = new PanelUvSize(orientation, child.DesiredSize);
            var childSize = new PanelUvSize(orientation, itemU, childUvSize.V);
            var childRectSize = new PanelUvSize(orientation, minIndex * itemU, minV);

            child.Arrange(new Rect(new Point(childRectSize.U, childRectSize.V), childSize.ScreenSize));
            vArr[minIndex] = minV + childUvSize.V;
        }

        return finalSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, constraint);

        var groups = CaculateGroupCount(orientation, uvConstraint);
        if (groups < 1)
        {
            return constraint;
        }

        var vArr = new double[groups].ToList();
        var itemU = uvConstraint.U / groups;
        if (double.IsNaN(itemU) || double.IsInfinity(itemU))
        {
            return constraint;
        }

        var children = InternalChildren;
        for (int i = 0, count = children.Count; i < count; i++)
        {
            var child = children[i];
            if (child == null)
            {
                continue;
            }

            child.Measure(constraint);

            var sz = new PanelUvSize(orientation, child.DesiredSize);
            var minIndex = vArr.IndexOf(vArr.Min());
            var minV = vArr[minIndex];

            vArr[minIndex] = minV + sz.V;
        }

        uvConstraint = new PanelUvSize(orientation, new Size(uvConstraint.ScreenSize.Width, vArr.Max()));

        return uvConstraint.ScreenSize;
    }
}

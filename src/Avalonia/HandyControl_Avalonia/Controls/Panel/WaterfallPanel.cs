using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class WaterfallPanel : Panel
{
    public static readonly StyledProperty<int> GroupsProperty =
        AvaloniaProperty.Register<WaterfallPanel, int>(nameof(Groups), defaultValue: 2,
            validate: IsGroupsValid);

    public int Groups
    {
        get => GetValue(GroupsProperty);
        set => SetValue(GroupsProperty, value);
    }

    private static bool IsGroupsValid(int value) => value >= 1;

    public static readonly StyledProperty<bool> AutoGroupProperty =
        AvaloniaProperty.Register<WaterfallPanel, bool>(nameof(AutoGroup));

    public bool AutoGroup
    {
        get => GetValue(AutoGroupProperty);
        set => SetValue(AutoGroupProperty, value);
    }

    public static readonly StyledProperty<double> DesiredLengthProperty =
        AvaloniaProperty.Register<WaterfallPanel, double>(nameof(DesiredLength), defaultValue: 0d,
            coerce: CoerceLength);

    public double DesiredLength
    {
        get => GetValue(DesiredLengthProperty);
        set => SetValue(DesiredLengthProperty, value);
    }

    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<WaterfallPanel>(new StyledPropertyMetadata<Orientation>(
            defaultValue: Orientation.Horizontal));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    static WaterfallPanel()
    {
        AffectsMeasure<WaterfallPanel>(
            GroupsProperty,
            AutoGroupProperty,
            DesiredLengthProperty,
            OrientationProperty
        );
    }

    private static double CoerceLength(AvaloniaObject _, double length) => length < 0 ? 0 : length;

    private int CalculateGroupCount(PanelUvSize size)
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

    protected override Size MeasureOverride(Size constraint)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, constraint);

        var groups = CalculateGroupCount(uvConstraint);
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

        var children = Children;
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

    protected override Size ArrangeOverride(Size finalSize)
    {
        var orientation = Orientation;
        var uvConstraint = new PanelUvSize(orientation, finalSize);

        var groups = CalculateGroupCount(uvConstraint);
        if (groups < 1)
        {
            return finalSize;
        }

        var vArr = new double[groups].ToList();
        var itemU = uvConstraint.U / groups;

        var children = Children;
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
}

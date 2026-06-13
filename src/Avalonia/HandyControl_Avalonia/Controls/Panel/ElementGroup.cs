using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ElementGroup : ItemsControl
{
    private static readonly Dictionary<Orientation, Dictionary<ChildLocation, CornerRadius>> CornerRadiusDict;

    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<ElementGroup>(
            new StyledPropertyMetadata<Orientation>(defaultValue: Orientation.Horizontal));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<LinearLayout> LayoutProperty =
        AvaloniaProperty.Register<ElementGroup, LinearLayout>(nameof(Layout), defaultValue: LinearLayout.Uniform);

    public LinearLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    static ElementGroup()
    {
        AffectsMeasure<ElementGroup>(OrientationProperty, LayoutProperty);

        var defaultCornerRadius = ResourceHelper.GetResource<CornerRadius>("DefaultCornerRadius");

        CornerRadiusDict = new Dictionary<Orientation, Dictionary<ChildLocation, CornerRadius>>
        {
            [Orientation.Horizontal] = new()
            {
                [ChildLocation.Single] = defaultCornerRadius,
                [ChildLocation.First] = new CornerRadius(defaultCornerRadius.TopLeft, 0, 0, defaultCornerRadius.BottomLeft),
                [ChildLocation.Middle] = new CornerRadius(),
                [ChildLocation.Last] = new CornerRadius(0, defaultCornerRadius.TopRight, defaultCornerRadius.BottomRight, 0)
            },
            [Orientation.Vertical] = new()
            {
                [ChildLocation.Single] = defaultCornerRadius,
                [ChildLocation.First] = new CornerRadius(defaultCornerRadius.TopLeft, defaultCornerRadius.TopRight, 0, 0),
                [ChildLocation.Middle] = new CornerRadius(),
                [ChildLocation.Last] = new CornerRadius(0, 0, defaultCornerRadius.BottomRight, defaultCornerRadius.BottomLeft)
            }
        };
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);

        container.GotFocus += ElementAdded_GotFocus;
        container.LostFocus += ElementAdded_LostFocus;
        container.PropertyChanged += Container_PropertyChanged;

        UpdateChildrenCornerRadius();
    }

    protected override void ClearContainerForItemOverride(Control container)
    {
        base.ClearContainerForItemOverride(container);

        container.GotFocus -= ElementAdded_GotFocus;
        container.LostFocus -= ElementAdded_LostFocus;
        container.PropertyChanged -= Container_PropertyChanged;

        UpdateChildrenCornerRadius();
    }

    private void Container_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == IsVisibleProperty)
        {
            UpdateChildrenCornerRadius();
        }
    }

    private void UpdateChildrenCornerRadius()
    {
        var visibleChildren = GetVisibleChildren();
        var count = visibleChildren.Count;
        if (count == 0)
        {
            return;
        }

        var orientation = Orientation;

        if (count == 1)
        {
            UpdateChildCornerRadius(visibleChildren[0], CornerRadiusDict[orientation][ChildLocation.Single]);
        }
        else
        {
            UpdateChildCornerRadius(visibleChildren[0], CornerRadiusDict[orientation][ChildLocation.First]);

            var childMargin = orientation == Orientation.Horizontal
                ? new Thickness(-1, 0, 0, 0)
                : new Thickness(0, -1, 0, 0);

            for (var childIndex = 1; childIndex < count; childIndex++)
            {
                var child = visibleChildren[childIndex];
                child.SetCurrentValue(MarginProperty, childMargin);
                UpdateChildCornerRadius(child, CornerRadiusDict[orientation][ChildLocation.Middle]);
            }

            var lastChild = visibleChildren[count - 1];
            lastChild.SetCurrentValue(MarginProperty, childMargin);
            UpdateChildCornerRadius(lastChild, CornerRadiusDict[orientation][ChildLocation.Last]);
        }
    }

    private List<Control> GetVisibleChildren()
    {
        return Items.OfType<Control>().Where(element => element.IsVisible).ToList();
    }

    private static void UpdateChildCornerRadius(AvaloniaObject child, CornerRadius cornerRadius)
    {
        if (child is Border border)
        {
            border.SetCurrentValue(Border.CornerRadiusProperty, cornerRadius);
        }
        else
        {
            BorderElement.SetCornerRadius(child, cornerRadius);
        }
    }

    private void ElementAdded_LostFocus(object? sender, RoutedEventArgs e) => ResetElementZIndex(e.Source);

    private void ElementAdded_GotFocus(object? sender, RoutedEventArgs e) => MaximizeElementZIndex(e.Source);

    private static void ResetElementZIndex(object? element)
    {
        if (element is Control control)
        {
            control.ZIndex = 0;
        }
    }

    private static void MaximizeElementZIndex(object? element)
    {
        if (element is Control control)
        {
            control.ZIndex = int.MaxValue;
        }
    }

    private enum ChildLocation
    {
        Single,
        First,
        Middle,
        Last
    }
}

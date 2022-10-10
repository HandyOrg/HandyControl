using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ElementGroup : ItemsControl
{
    private static readonly Dictionary<Orientation, Dictionary<ChildLocation, CornerRadius>> CornerRadiusDict;

    public static readonly DependencyProperty OrientationProperty =
        StackPanel.OrientationProperty.AddOwner(typeof(ElementGroup),
            new FrameworkPropertyMetadata(ValueBoxes.HorizontalBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public Orientation Orientation
    {
        get => (Orientation) GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, ValueBoxes.OrientationBox(value));
    }

    public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
        nameof(Layout), typeof(LinearLayout), typeof(ElementGroup),
        new FrameworkPropertyMetadata(LinearLayout.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public LinearLayout Layout
    {
        get => (LinearLayout) GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    static ElementGroup()
    {
        var defaultCornerRadius = ResourceHelper.GetResourceInternal<CornerRadius>("DefaultCornerRadius");

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

    protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
    {
        base.OnVisualChildrenChanged(visualAdded, visualRemoved);

        if (visualAdded is UIElement elementAdded)
        {
            elementAdded.GotFocus += ElementAdded_GotFocus;
            elementAdded.LostFocus += ElementAdded_LostFocus;
            elementAdded.IsVisibleChanged += ElementAdded_IsVisibleChanged;
        }

        if (visualRemoved is UIElement elementRemoved)
        {
            elementRemoved.GotFocus -= ElementAdded_GotFocus;
            elementRemoved.LostFocus -= ElementAdded_LostFocus;
            elementRemoved.IsVisibleChanged -= ElementAdded_IsVisibleChanged;
        }
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e) => UpdateChildrenCornerRadius();

    protected override void OnRender(DrawingContext drawingContext) => UpdateChildrenCornerRadius();

    private void UpdateChildrenCornerRadius()
    {
        var visibleChildren = GetVisibleChildrenCount();
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

    private List<UIElement> GetVisibleChildrenCount()
    {
        return Items.OfType<UIElement>().Where(element => element.Visibility == Visibility.Visible).ToList();
    }

    private static void UpdateChildCornerRadius(DependencyObject child, CornerRadius cornerRadius)
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

    private void ElementAdded_LostFocus(object sender, RoutedEventArgs e) => ResetElementZIndex(e.OriginalSource);

    private void ElementAdded_GotFocus(object sender, RoutedEventArgs e) => MaximizeElementZIndex(e.OriginalSource);

    private static void ResetElementZIndex(object element) => Panel.SetZIndex((UIElement) element, 0);

    private static void MaximizeElementZIndex(object element) => Panel.SetZIndex((UIElement) element, int.MaxValue);

    private void ElementAdded_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => UpdateChildrenCornerRadius();

    private enum ChildLocation
    {
        Single,
        First,
        Middle,
        Last
    }
}

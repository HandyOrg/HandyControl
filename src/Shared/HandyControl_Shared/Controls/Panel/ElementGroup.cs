using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class ElementGroup : SimpleStackPanel
    {
        private static readonly Dictionary<Orientation, Dictionary<ChildLocation, CornerRadius>> CornerRadiusDict;

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
            }

            if (visualRemoved is UIElement elementRemoved)
            {
                elementRemoved.GotFocus -= ElementAdded_GotFocus;
                elementRemoved.LostFocus -= ElementAdded_LostFocus;
            }

            var childrenCount = InternalChildren.Count;
            if (childrenCount == 0)
            {
                return;
            }

            var orientation = Orientation;

            if (childrenCount == 1)
            {
                UpdateChildCornerRadius(InternalChildren[0], CornerRadiusDict[orientation][ChildLocation.Single]);
            }
            else
            {
                UpdateChildCornerRadius(InternalChildren[0], CornerRadiusDict[orientation][ChildLocation.First]);

                var childMargin = orientation == Orientation.Horizontal
                    ? new Thickness(-1, 0, 0, 0)
                    : new Thickness(0, -1, 0, 0);

                for (var childIndex = 1; childIndex < childrenCount; childIndex++)
                {
                    var child = InternalChildren[childIndex];
                    child.SetCurrentValue(MarginProperty, childMargin);
                    UpdateChildCornerRadius(child, CornerRadiusDict[orientation][ChildLocation.Middle]);
                }

                var lastChild = InternalChildren[childrenCount - 1];
                lastChild.SetCurrentValue(MarginProperty, childMargin);
                UpdateChildCornerRadius(lastChild, CornerRadiusDict[orientation][ChildLocation.Last]);
            }
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

        private void ElementAdded_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetZIndex(element, 0);
            }
        }

        private void ElementAdded_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetZIndex(element, int.MaxValue);
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
}

using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Tools
{
    public class VisualStates
    {
        public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
        {
            var root = GetImplementationRoot(dependencyObject);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
        {
            return 1 == VisualTreeHelper.GetChildrenCount(dependencyObject)
                ? VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement
                : null;
        }
    }
}
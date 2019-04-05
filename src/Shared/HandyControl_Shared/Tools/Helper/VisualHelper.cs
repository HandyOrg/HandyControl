using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HandyControl.Tools
{
    public class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            var root = GetImplementationRoot(d);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d)
        {
            return 1 == VisualTreeHelper.GetChildrenCount(d)
                ? VisualTreeHelper.GetChild(d, 0) as FrameworkElement
                : null;
        }

        internal static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d is T t)
            {
                return t;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = GetChild<T>(child);
                if (result != null) return result;
            }

            return default(T);
        }

        /// <summary>
        ///     获取当前应用中处于激活的一个窗口
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
    }
}
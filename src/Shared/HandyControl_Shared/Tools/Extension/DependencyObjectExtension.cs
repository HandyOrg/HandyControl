using System.Windows;
using System.Windows.Media;

namespace HandyControl.Tools.Extension
{
    public static class DependencyObjectExtension
    {
        public static DependencyObject GetVisualOrLogicalParent(this DependencyObject sourceElement)
        {
            return sourceElement switch
            {
                null => null,
                Visual _ => (VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement)),
                _ => LogicalTreeHelper.GetParent(sourceElement)
            };
        }
    }
}

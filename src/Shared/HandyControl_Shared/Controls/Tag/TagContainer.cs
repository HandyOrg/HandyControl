using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class TagContainer : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride() => new Tag();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is Tag;
    }
}
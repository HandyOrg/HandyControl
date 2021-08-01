using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class RibbonTabHeaderItemsControl : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride() => new RibbonTabHeader();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonTabHeader;
    }
}

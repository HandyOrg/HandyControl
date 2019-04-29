using System.Windows;

namespace HandyControl.Controls
{
    public class SideMenu : HeaderedSimpleItemsControl
    {
        private SideMenuItem _selectedItem;

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;
    }
}

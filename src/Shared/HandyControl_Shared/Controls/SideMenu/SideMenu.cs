using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class SideMenu : HeaderedSimpleItemsControl
    {
        private SideMenuItem _selectedItem;

        private SideMenuItem _selectedHeader;

        public SideMenu()
        {
            AddHandler(SideMenuItem.SelectedEvent, new RoutedEventHandler(SideMenuItemSelected));
        }

        private void SideMenuItemSelected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is SideMenuItem item)
            {
                if (item.Role == SideMenuItemRole.Item)
                {
                    if (_selectedItem != null)
                    {
                        _selectedItem.IsSelected = false;
                    }

                    _selectedItem = item;
                    _selectedItem.IsSelected = true;
                }
                else
                {
                    if (_selectedHeader != null)
                    {
                        _selectedHeader.IsSelected = false;
                    }

                    _selectedHeader = item;
                    _selectedHeader.IsSelected = true;
                    _selectedHeader.SelectDefaultItem(_selectedItem);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;
    }
}

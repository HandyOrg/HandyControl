using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class SideMenu : HeaderedSimpleItemsControl
    {
        private SideMenuItem _selectedItem;

        private SideMenuItem _selectedHeader;

        private bool _isItemSelected;

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
                    _isItemSelected = true;
                }
                else
                {
                    if (!Equals(item, _selectedHeader))
                    {
                        if (_selectedHeader != null)
                        {
                            _selectedHeader.IsSelected = false;
                        }

                        _selectedHeader = item;
                        _selectedHeader.IsSelected = true;
                    }

                    if (_isItemSelected)
                    {
                        _isItemSelected = false;
                    }
                    else
                    {
                        _selectedHeader.SelectDefaultItem();
                        _isItemSelected = false;
                    }
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;
    }
}

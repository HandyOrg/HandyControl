using System.Windows;
using HandyControl.Data;
using HandyControl.Data.Enum;

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
                            SwitchPanelArea(_selectedHeader);
                        }

                        _selectedHeader = item;
                        _selectedHeader.IsSelected = true;
                        SwitchPanelArea(_selectedHeader);
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

        private void SwitchPanelArea(SideMenuItem oldItem)
        {
            switch (ExpandMode)
            {
                case ExpandMode.ShowAll:
                    return;
                case ExpandMode.ShowOne:
                case ExpandMode.Accordion:
                    oldItem.SwitchPanelArea(oldItem.IsSelected);
                    break;
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

        public static readonly DependencyProperty ExpandModeProperty = DependencyProperty.Register(
            "ExpandMode", typeof(ExpandMode), typeof(SideMenu), new PropertyMetadata(default(ExpandMode)));

        public ExpandMode ExpandMode
        {
            get => (ExpandMode) GetValue(ExpandModeProperty);
            set => SetValue(ExpandModeProperty, value);
        }
    }
}

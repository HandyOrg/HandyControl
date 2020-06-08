using System;
using System.Linq;
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

            Loaded += (s, e) => Init();
        }

        protected override void Refresh()
        {
            base.Refresh();

            Init();
        }

        private void Init()
        {
            if (ItemsHost == null) return;

            OnExpandModeChanged(ExpandMode);
        }

        private void SideMenuItemSelected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is SideMenuItem item)
            {
                if (item.Role == SideMenuItemRole.Item)
                {
                    _isItemSelected = true;

                    if (Equals(item, _selectedItem)) return;

                    if (_selectedItem != null)
                    {
                        _selectedItem.IsSelected = false;
                    }

                    _selectedItem = item;
                    _selectedItem.IsSelected = true;
                    RaiseEvent(new FunctionEventArgs<object>(SelectionChangedEvent, this)
                    {
                        Info = e.OriginalSource
                    });
                }
                else
                {
                    if (!Equals(item, _selectedHeader))
                    {
                        if (_selectedHeader != null)
                        {
                            if (ExpandMode == ExpandMode.Freedom && item.ItemsHost.IsVisible && !_isItemSelected)
                            {
                                item.IsSelected = false;
                                SwitchPanelArea(item);
                                return;
                            }

                            _selectedHeader.IsSelected = false;
                            if (ExpandMode != ExpandMode.Freedom)
                            {
                                SwitchPanelArea(_selectedHeader);
                            }
                        }

                        _selectedHeader = item;
                        _selectedHeader.IsSelected = true;
                        SwitchPanelArea(_selectedHeader);
                    }
                    else if (ExpandMode == ExpandMode.Freedom && !_isItemSelected)
                    {
                        _selectedHeader.IsSelected = false;
                        SwitchPanelArea(_selectedHeader);
                        _selectedHeader = null;
                    }

                    if (_isItemSelected)
                    {
                        _isItemSelected = false;
                    }
                    else if(_selectedHeader != null)
                    {
                        if (AutoSelect)
                        {
                            _selectedHeader.SelectDefaultItem();
                        }
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
                case ExpandMode.Freedom:
                case ExpandMode.Accordion:
                    oldItem.SwitchPanelArea(oldItem.IsSelected);
                    break;
            }
        }

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

        public static readonly DependencyProperty AutoSelectProperty = DependencyProperty.Register(
            "AutoSelect", typeof(bool), typeof(SideMenu), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool AutoSelect
        {
            get => (bool) GetValue(AutoSelectProperty);
            set => SetValue(AutoSelectProperty, value);
        }

        public static readonly DependencyProperty ExpandModeProperty = DependencyProperty.Register(
            "ExpandMode", typeof(ExpandMode), typeof(SideMenu), new PropertyMetadata(default(ExpandMode), OnExpandModeChanged));

        private static void OnExpandModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (SideMenu) d;
            var v = (ExpandMode) e.NewValue;

            if (ctl.ItemsHost == null)
            {
                return;
            }

            ctl.OnExpandModeChanged(v);
        }

        private void OnExpandModeChanged(ExpandMode mode)
        {
            if (mode == ExpandMode.ShowAll)
            {
                ShowAll();
            }
            else if (mode == ExpandMode.ShowOne)
            {
                SideMenuItem sideMenuItemSelected = null;
                foreach (var sideMenuItem in ItemsHost.Children.OfType<SideMenuItem>())
                {
                    if (sideMenuItemSelected != null)
                    {
                        sideMenuItem.IsSelected = false;
                        if (sideMenuItem.ItemsHost != null)
                        {
                            foreach (var sideMenuSubItem in sideMenuItem.ItemsHost.Children.OfType<SideMenuItem>())
                            {
                                sideMenuSubItem.IsSelected = false;
                            }
                        }
                    }
                    else if (sideMenuItem.IsSelected)
                    {
                        switch (sideMenuItem.Role)
                        {
                            case SideMenuItemRole.Header:
                                _selectedHeader = sideMenuItem;
                                break;
                            case SideMenuItemRole.Item:
                                _selectedItem = sideMenuItem;
                                break;
                        }

                        ShowSelectedOne(sideMenuItem);
                        sideMenuItemSelected = sideMenuItem;

                        if (sideMenuItem.ItemsHost != null)
                        {
                            foreach (var sideMenuSubItem in sideMenuItem.ItemsHost.Children.OfType<SideMenuItem>())
                            {
                                if (_selectedItem != null)
                                {
                                    sideMenuSubItem.IsSelected = false;
                                }
                                else if (sideMenuSubItem.IsSelected)
                                {
                                    _selectedItem = sideMenuSubItem;
                                }
                            }
                        }                          
                    }
                }
            }
        }

        public ExpandMode ExpandMode
        {
            get => (ExpandMode) GetValue(ExpandModeProperty);
            set => SetValue(ExpandModeProperty, value);
        }

        public static readonly DependencyProperty PanelAreaLengthProperty = DependencyProperty.Register(
            "PanelAreaLength", typeof(double), typeof(SideMenu), new PropertyMetadata(double.NaN));

        public double PanelAreaLength
        {
            get => (double) GetValue(PanelAreaLengthProperty);
            set => SetValue(PanelAreaLengthProperty, value);
        }

        private void ShowAll()
        {
            foreach (var sideMenuItem in ItemsHost.Children.OfType<SideMenuItem>())
            {
                sideMenuItem.SwitchPanelArea(true);
            }
        }

        private void ShowSelectedOne(SideMenuItem item)
        {
            foreach (var sideMenuItem in ItemsHost.Children.OfType<SideMenuItem>())
            {
                sideMenuItem.SwitchPanelArea(Equals(sideMenuItem, item));
            }
        }

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanged", RoutingStrategy.Bubble, typeof(EventHandler<FunctionEventArgs<object>>), typeof(SideMenu));

        public event EventHandler<FunctionEventArgs<object>> SelectionChanged
        {
            add => AddHandler(SelectionChangedEvent, value);
            remove => RemoveHandler(SelectionChangedEvent, value);
        }
    }
}

using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class SideMenuItem : HeaderedSimpleItemsControl, ISelectable
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(object), typeof(SideMenuItem), new PropertyMetadata(default(object)));

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        protected virtual void OnSelected(RoutedEventArgs e) => RaiseEvent(e);

        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SideMenuItem));

        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(SideMenuItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private static readonly DependencyPropertyKey RolePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Role), typeof(SideMenuItemRole), typeof(SideMenuItem),
                new PropertyMetadata(SideMenuItemRole.Header));

        public static readonly DependencyProperty RoleProperty = RolePropertyKey.DependencyProperty;

        public SideMenuItemRole Role => (SideMenuItemRole) GetValue(RoleProperty);

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

        private void UpdateRole()
        {
            
        }
    }
}
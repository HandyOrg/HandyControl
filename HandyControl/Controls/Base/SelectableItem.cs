using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class SelectableItem : ContentControl, ISelectable
    {
        private bool _isMouseLeftButtonDown;

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _isMouseLeftButtonDown = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _isMouseLeftButtonDown = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (_isMouseLeftButtonDown)
            {
                RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
                _isMouseLeftButtonDown = false;
            }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SelectableItem));

        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }
    }
}
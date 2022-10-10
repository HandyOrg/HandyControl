using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Controls;

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
            if (SelfManage)
            {
                if (!IsSelected)
                {
                    IsSelected = true;
                    OnSelected(new RoutedEventArgs(SelectedEvent, this));
                }
                else if (CanDeselect)
                {
                    IsSelected = false;
                    OnSelected(new RoutedEventArgs(DeselectedEvent, this));
                }
            }
            else
            {
                if (CanDeselect)
                {
                    OnSelected(IsSelected
                        ? new RoutedEventArgs(DeselectedEvent, this)
                        : new RoutedEventArgs(SelectedEvent, this));
                }
                else
                {
                    OnSelected(new RoutedEventArgs(SelectedEvent, this));
                }
            }
            _isMouseLeftButtonDown = false;
        }
    }

    protected virtual void OnSelected(RoutedEventArgs e) => RaiseEvent(e);

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected), typeof(bool), typeof(SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsSelected
    {
        get => (bool) GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty SelfManageProperty = DependencyProperty.Register(
        nameof(SelfManage), typeof(bool), typeof(SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool SelfManage
    {
        get => (bool) GetValue(SelfManageProperty);
        set => SetValue(SelfManageProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty CanDeselectProperty = DependencyProperty.Register(
        nameof(CanDeselect), typeof(bool), typeof(SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool CanDeselect
    {
        get => (bool) GetValue(CanDeselectProperty);
        set => SetValue(CanDeselectProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly RoutedEvent SelectedEvent =
        EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SelectableItem));

    public event RoutedEventHandler Selected
    {
        add => AddHandler(SelectedEvent, value);
        remove => RemoveHandler(SelectedEvent, value);
    }

    public static readonly RoutedEvent DeselectedEvent =
        EventManager.RegisterRoutedEvent("Deselected", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SelectableItem));

    public event RoutedEventHandler Deselected
    {
        add => AddHandler(DeselectedEvent, value);
        remove => RemoveHandler(DeselectedEvent, value);
    }
}

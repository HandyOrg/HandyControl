using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HandyControl.Controls;

public class RangeThumb : Thumb
{
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content), typeof(object), typeof(RangeThumb), new PropertyMetadata(default(object)));

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {

    }

    public void StartDrag()
    {
        IsDragging = true;
        Focus();
        CaptureMouse();
        RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
        {
            RoutedEvent = PreviewMouseLeftButtonDownEvent,
            Source = this
        });
    }

    public new void CancelDrag()
    {
        base.CancelDrag();

        RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
        {
            RoutedEvent = PreviewMouseLeftButtonUpEvent
        });
    }
}

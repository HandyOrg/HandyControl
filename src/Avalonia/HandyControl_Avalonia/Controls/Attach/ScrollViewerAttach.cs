using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace HandyControl.Controls;

public class ScrollViewerAttach
{
    public static readonly AttachedProperty<Orientation> OrientationProperty =
        AvaloniaProperty.RegisterAttached<ScrollViewerAttach, AvaloniaObject, Orientation>("Orientation",
            defaultValue: Orientation.Vertical, inherits: true);

    public static void SetOrientation(AvaloniaObject element, Orientation value) =>
        element.SetValue(OrientationProperty, value);

    public static Orientation GetOrientation(AvaloniaObject element) => element.GetValue(OrientationProperty);

    static ScrollViewerAttach()
    {
        OrientationProperty.Changed.AddClassHandler<AvaloniaObject>(OnOrientationChanged);
    }

    private static void OnOrientationChanged(AvaloniaObject element, AvaloniaPropertyChangedEventArgs e)
    {
        if (element is not ScrollViewer scrollViewer)
        {
            return;
        }

        if (e.GetNewValue<Orientation>() == Orientation.Horizontal)
        {
            scrollViewer.PointerWheelChanged += ScrollViewerPointerWheelChanged;
        }
        else
        {
            scrollViewer.PointerWheelChanged -= ScrollViewerPointerWheelChanged;
        }
    }

    private static void ScrollViewerPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        const int step = 50;

        if (sender is not ScrollViewer scrollViewer)
        {
            return;
        }

        scrollViewer.Offset = new Vector(
            scrollViewer.Offset.X - e.Delta.Y * step,
            scrollViewer.Offset.Y
        );

        e.Handled = true;
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace HandyControl.Tools;

public static class VisualHelper
{
    public static T? GetParent<T>(Visual? visual) where T : Visual =>
        visual switch
        {
            null => default,
            T t => t,
            Window _ => null,
            _ => GetParent<T>(visual.GetVisualParent())
        };
}

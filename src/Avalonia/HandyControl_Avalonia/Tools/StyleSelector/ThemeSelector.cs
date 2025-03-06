using Avalonia;
using Avalonia.Styling;

namespace HandyControl.Tools;

public class ThemeSelector
{
    public virtual ControlTheme? SelectTheme(object? item, AvaloniaObject? container) => null;
}

using Avalonia;

namespace HandyControl.Tools;

public class ResourceHelper
{
    public static T? GetResource<T>(string key)
    {
        if (Application.Current is null)
        {
            return default;
        }

        if (Application.Current.TryGetResource(key, Application.Current.ActualThemeVariant, out var value) &&
            value is T resource)
        {
            return resource;
        }

        return default;
    }
}

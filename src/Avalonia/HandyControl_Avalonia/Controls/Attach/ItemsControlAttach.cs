using Avalonia;
using Avalonia.Controls;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ItemsControlAttach
{
    public static readonly AttachedProperty<ThemeSelector?> ItemsContainerThemeSelectorProperty =
        AvaloniaProperty.RegisterAttached<ItemsControlAttach, AvaloniaObject, ThemeSelector?>(
            "ItemsContainerThemeSelector");

    public static void SetItemsContainerThemeSelector(AvaloniaObject element, ThemeSelector? value) =>
        element.SetValue(ItemsContainerThemeSelectorProperty, value);

    public static ThemeSelector? GetItemsContainerThemeSelector(AvaloniaObject element) =>
        element.GetValue(ItemsContainerThemeSelectorProperty);

    static ItemsControlAttach()
    {
        ItemsContainerThemeSelectorProperty.Changed.AddClassHandler<AvaloniaObject>(
            OnItemsContainerThemeSelectorChanged);
    }

    private static void OnItemsContainerThemeSelectorChanged(AvaloniaObject element, AvaloniaPropertyChangedEventArgs e)
    {
        if (element is not ItemsControl itemsControl)
        {
            return;
        }

        itemsControl.ContainerPrepared -= OnContainerPrepared;
        var themeSelector = e.GetNewValue<ThemeSelector?>();

        if (themeSelector is null)
        {
            return;
        }

        itemsControl.ContainerPrepared += OnContainerPrepared;
        ApplyTheme(itemsControl, themeSelector);
    }

    private static void OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        if (sender is not ItemsControl itemsControl)
        {
            return;
        }

        var themeSelector = GetItemsContainerThemeSelector(itemsControl);
        if (themeSelector is null)
        {
            return;
        }

        ApplyTheme(itemsControl, themeSelector);
    }

    private static void ApplyTheme(ItemsControl itemsControl, ThemeSelector themeSelector)
    {
        for (int i = 0; i < itemsControl.ItemCount; i++)
        {
            var container = itemsControl.ContainerFromIndex(i);
            if (container != null)
            {
                container.Theme = themeSelector.SelectTheme(itemsControl.Items[i], container);
            }
        }
    }
}

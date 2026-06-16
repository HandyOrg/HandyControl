using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HandyControl.Controls;

public class TagContainer : ItemsControl
{
    public static readonly AttachedProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.RegisterAttached<TagContainer, AvaloniaObject, bool>(
            "ShowCloseButton", defaultValue: true, inherits: true);

    public static void SetShowCloseButton(AvaloniaObject element, bool value) =>
        element.SetValue(ShowCloseButtonProperty, value);

    public static bool GetShowCloseButton(AvaloniaObject element) =>
        element.GetValue(ShowCloseButtonProperty);

    public TagContainer()
    {
        AddHandler(HandyControl.Controls.Tag.ClosedEvent, Tag_OnClosed);
    }

    private void Tag_OnClosed(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Tag tag)
        {
            return;
        }

        tag.Hide();

        if (ItemsSource == null)
        {
            Items.Remove(tag);
        }
        else if (ItemsSource is IList list)
        {
            var item = ItemFromContainer(tag);
            if (item != null && list.Contains(item))
            {
                list.Remove(item);
            }
        }
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        if (item is Tag)
        {
            recycleKey = null;
            return false;
        }

        recycleKey = typeof(Tag);
        return true;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new Tag();

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);

        // ItemsControl auto-binds HeaderedContentControl.Header to the item.
        // Tag is a HeaderedContentControl but, in TagContainer, the Header is meant
        // for an explicit avatar/icon. Clear the auto-assigned Header to prevent the
        // item's ToString() bleeding into the header circle. Skip when the item IS
        // the container (Tag declared directly in XAML) so an explicit Header is kept.
        if (container is Tag tag && !ReferenceEquals(container, item))
        {
            tag.ClearValue(Avalonia.Controls.Primitives.HeaderedContentControl.HeaderProperty);
        }
    }
}

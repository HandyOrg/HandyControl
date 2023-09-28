using System.Collections;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class TagContainer : ItemsControl
{
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.RegisterAttached(
        "ShowCloseButton", typeof(bool), typeof(TagContainer), new FrameworkPropertyMetadata(ValueBoxes.TrueBox,
            FrameworkPropertyMetadataOptions.Inherits));

    public TagContainer()
    {
        AddHandler(Controls.Tag.ClosedEvent, new RoutedEventHandler(Tag_OnClosed));
    }

    public static void SetShowCloseButton(DependencyObject element, bool value)
        => element.SetValue(ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));

    public static bool GetShowCloseButton(DependencyObject element)
        => (bool) element.GetValue(ShowCloseButtonProperty);

    private void Tag_OnClosed(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Tag tag)
        {
            tag.Hide();

            if (ItemsSource == null)
            {
                Items.Remove(tag);
            }
            else
            {
                var item = ItemContainerGenerator.ItemFromContainer(tag);
                GetActualList()?.Remove(item);
                Items.Refresh();
            }
        }
    }

    internal IList GetActualList()
    {
        IList list;
        if (ItemsSource != null)
        {
            list = ItemsSource as IList;
        }
        else
        {
            list = Items;
        }

        return list;
    }

    protected override DependencyObject GetContainerForItemOverride() => new Tag();

    protected override bool IsItemItsOwnContainerOverride(object item) => item is Tag;
}

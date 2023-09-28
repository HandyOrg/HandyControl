using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class MenuItemAttach
{
    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(MenuItemAttach),
            new PropertyMetadata(string.Empty, OnGroupNameChanged));

    [AttachedPropertyBrowsableForType(typeof(MenuItem))]
    public static string GetGroupName(DependencyObject obj) => (string) obj.GetValue(GroupNameProperty);

    [AttachedPropertyBrowsableForType(typeof(MenuItem))]
    public static void SetGroupName(DependencyObject obj, string value) => obj.SetValue(GroupNameProperty, value);

    private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MenuItem menuItem)
        {
            return;
        }

        menuItem.Checked -= MenuItem_Checked;
        menuItem.Click -= MenuItem_Click;

        if (string.IsNullOrWhiteSpace(e.NewValue.ToString()))
        {
            return;
        }

        menuItem.Checked += MenuItem_Checked;
        menuItem.Click += MenuItem_Click;
    }

    private static void MenuItem_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { Parent: MenuItem parent } menuItem)
        {
            return;
        }

        var groupName = GetGroupName(menuItem);
        parent
            .Items
            .OfType<MenuItem>()
            .Where(item => item != menuItem && item.IsCheckable && string.Equals(GetGroupName(item), groupName))
            .Do(item => item.IsChecked = false);
    }

    private static void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        // prevent uncheck when click the checked menu item
        if (e.OriginalSource is MenuItem { IsChecked: false } menuItem)
        {
            menuItem.IsChecked = true;
        }
    }
}

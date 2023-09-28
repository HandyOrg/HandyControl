using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ListBoxAttach
{
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
        "SelectedItems", typeof(IList), typeof(ListBoxAttach),
        new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnSelectedItemsChanged));

    public static void SetSelectedItems(DependencyObject element, IList value)
        => element.SetValue(SelectedItemsProperty, value);

    public static IList GetSelectedItems(DependencyObject element)
        => (IList) element.GetValue(SelectedItemsProperty);

    internal static readonly DependencyProperty InternalActionProperty = DependencyProperty.RegisterAttached(
        "InternalAction", typeof(bool), typeof(ListBoxAttach), new PropertyMetadata(ValueBoxes.FalseBox));

    internal static void SetInternalAction(DependencyObject element, bool value)
        => element.SetValue(InternalActionProperty, ValueBoxes.BooleanBox(value));

    internal static bool GetInternalAction(DependencyObject element)
        => (bool) element.GetValue(InternalActionProperty);

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ListBox listBox)
        {
            return;
        }

        if (GetInternalAction(listBox))
        {
            return;
        }

        listBox.SelectionChanged -= OnListBoxSelectionChanged;
        listBox.SelectedItems.Clear();

        if (e.NewValue is IList selectedItems)
        {
            foreach (object selectedItem in selectedItems)
            {
                listBox.SelectedItems.Add(selectedItem);
            }
        }

        listBox.SelectionChanged += OnListBoxSelectionChanged;
    }

    private static void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            SetInternalAction(listBox, true);
            SetSelectedItems(listBox, listBox.SelectedItems.Cast<object>().ToArray());
            SetInternalAction(listBox, false);
        }
    }
}

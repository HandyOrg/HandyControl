using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

public class SelectAllButtonAttach
{
    private static readonly ConditionalWeakTable<Selector, ToggleButton> SelectorButtonTable = new();

    public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
        "Target", typeof(Selector), typeof(SelectAllButtonAttach), new PropertyMetadata(null, OnTargetChanged));

    public static void SetTarget(DependencyObject element, Selector value)
        => element.SetValue(TargetProperty, value);

    public static Selector GetTarget(DependencyObject element)
        => (Selector) element.GetValue(TargetProperty);

    private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ToggleButton button)
        {
            return;
        }

        button.Click -= OnButtonOnClick;

        if (e.OldValue is Selector oldSelector)
        {
            oldSelector.SelectionChanged -= OnSelectorSelectionChanged;
            SelectorButtonTable.Remove(oldSelector);
        }

        if (e.NewValue is not Selector newSelector)
        {
            return;
        }

        button.Click += OnButtonOnClick;
        newSelector.SelectionChanged += OnSelectorSelectionChanged;

        if (!SelectorButtonTable.TryGetValue(newSelector, out _))
        {
            SelectorButtonTable.Add(newSelector, button);
        }
    }

    private static void OnButtonOnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton button)
        {
            return;
        }

        if (button.IsChecked == true)
        {
            SelectAll(GetTarget(button));
        }
        else
        {
            UnselectAll(GetTarget(button));
        }
    }

    private static void OnSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not Selector selector)
        {
            return;
        }

        if (!SelectorButtonTable.TryGetValue(selector, out var button))
        {
            return;
        }

        var selectedCount = GetSelectedItemsCount(selector);
        var totalCount = selector.Items.Count;
        button.IsChecked = selectedCount switch
        {
            0 => false,
            _ when selectedCount == totalCount => true,
            _ => null // Indeterminate
        };
    }

    private static void SelectAll(Selector selector)
    {
        if (selector is MultiSelector multiSelector)
        {
            multiSelector.SelectAll();
        }
        else if (selector is ListBox listBox)
        {
            listBox.SelectAll();
        }
    }

    private static void UnselectAll(Selector selector)
    {
        if (selector is MultiSelector multiSelector)
        {
            multiSelector.UnselectAll();
        }
        else if (selector is ListBox listBox)
        {
            listBox.UnselectAll();
        }
    }

    private static int GetSelectedItemsCount(Selector selector)
    {
        if (selector is MultiSelector multiSelector)
        {
            return multiSelector.SelectedItems.Count;
        }

        if (selector is ListBox listBox)
        {
            return listBox.SelectedItems.Count;
        }

        return 0;
    }
}

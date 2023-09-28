using System;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

public class GridAttach
{
    public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
        "Name", typeof(string), typeof(GridAttach), new PropertyMetadata(default(string)));

    public static void SetName(DependencyObject element, string value)
        => element.SetValue(NameProperty, value);

    public static string GetName(DependencyObject element)
        => (string) element.GetValue(NameProperty);

    public static readonly DependencyProperty RowNameProperty = DependencyProperty.RegisterAttached(
        "RowName", typeof(string), typeof(GridAttach), new PropertyMetadata(default(string), OnRowNameChanged));

    private static void OnRowNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement frameworkElement)
        {
            if (e.NewValue is string rowName)
            {
                if (string.IsNullOrEmpty(rowName))
                {
                    return;
                }

                if (frameworkElement.Parent is Grid grid)
                {
                    for (var i = 0; i < grid.RowDefinitions.Count; i++)
                    {
                        var gridRowDefinition = grid.RowDefinitions[i];
                        var gridRowName = GetName(gridRowDefinition);

                        if (!string.IsNullOrEmpty(gridRowName) &&
                            gridRowName.Equals(rowName, StringComparison.Ordinal))
                        {
                            Grid.SetRow(frameworkElement, i);
                            return;
                        }
                    }
                }
            }
        }
    }

    public static void SetRowName(DependencyObject element, string value)
        => element.SetValue(RowNameProperty, value);

    public static string GetRowName(DependencyObject element)
        => (string) element.GetValue(RowNameProperty);

    public static readonly DependencyProperty ColumnNameProperty = DependencyProperty.RegisterAttached(
        "ColumnName", typeof(string), typeof(GridAttach), new PropertyMetadata(default(string), OnColumnNameChanged));

    private static void OnColumnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement frameworkElement)
        {
            if (e.NewValue is string columnName)
            {
                if (string.IsNullOrEmpty(columnName))
                {
                    return;
                }

                if (frameworkElement.Parent is Grid grid)
                {
                    for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
                    {
                        var gridColumnDefinition = grid.ColumnDefinitions[i];
                        var gridColumnName = GetName(gridColumnDefinition);

                        if (!string.IsNullOrEmpty(gridColumnName) &&
                            gridColumnName.Equals(columnName, StringComparison.Ordinal))
                        {
                            Grid.SetColumn(frameworkElement, i);
                            return;
                        }
                    }
                }
            }
        }
    }

    public static void SetColumnName(DependencyObject element, string value)
        => element.SetValue(ColumnNameProperty, value);

    public static string GetColumnName(DependencyObject element)
        => (string) element.GetValue(ColumnNameProperty);
}

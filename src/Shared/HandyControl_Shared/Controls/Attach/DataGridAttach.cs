using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class DataGridAttach
    {
        public static readonly DependencyProperty TextColumnStyleProperty = DependencyProperty.RegisterAttached(
            "TextColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnTextColumnStyleChanged));

        private static void OnTextColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)d;
            if (e.OldValue == null && e.NewValue != null)
            {
                UpdateTextColumnStyles(grid);
            }
        }

        public static void SetTextColumnStyle(DependencyObject element, Style value) => element.SetValue(TextColumnStyleProperty, value);

        public static Style GetTextColumnStyle(DependencyObject element) => (Style)element.GetValue(TextColumnStyleProperty);

        public static readonly DependencyProperty EditingTextColumnStyleProperty = DependencyProperty.RegisterAttached(
            "EditingTextColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnTextColumnStyleChanged));

        public static void SetEditingTextColumnStyle(DependencyObject element, Style value) => element.SetValue(EditingTextColumnStyleProperty, value);

        public static Style GetEditingTextColumnStyle(DependencyObject element) => (Style) element.GetValue(EditingTextColumnStyleProperty);

        private static void UpdateTextColumnStyles(DataGrid grid)
        {
            var textColumnStyle = GetTextColumnStyle(grid);
            var editingTextColumnStyle = GetEditingTextColumnStyle(grid);

            if (textColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridTextColumn>())
                {
                    var elementStyle = new Style
                    {
                        BasedOn = column.ElementStyle,
                        TargetType = textColumnStyle.TargetType
                    };

                    foreach (var setter in textColumnStyle.Setters.OfType<Setter>())
                    {
                        elementStyle.Setters.Add(setter);
                    }

                    column.ElementStyle = elementStyle;
                }
            }

            if (editingTextColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridTextColumn>())
                {
                    var editingElementStyle = new Style
                    {
                        BasedOn = column.EditingElementStyle,
                        TargetType = editingTextColumnStyle.TargetType
                    };

                    foreach (var setter in editingTextColumnStyle.Setters.OfType<Setter>())
                    {
                        editingElementStyle.Setters.Add(setter);
                    }

                    column.EditingElementStyle = editingElementStyle;
                }
            }
        }

        public static readonly DependencyProperty ComboBoxColumnStyleProperty = DependencyProperty.RegisterAttached(
            "ComboBoxColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnComboBoxColumnStyleChanged));

        private static void OnComboBoxColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)d;
            if (e.OldValue == null && e.NewValue != null)
            {
                UpdateComboBoxColumnStyles(grid);
            }
        }

        public static void SetComboBoxColumnStyle(DependencyObject element, Style value) => element.SetValue(ComboBoxColumnStyleProperty, value);

        public static Style GetComboBoxColumnStyle(DependencyObject element) => (Style) element.GetValue(ComboBoxColumnStyleProperty);

        public static readonly DependencyProperty EditingComboBoxColumnStyleProperty = DependencyProperty.RegisterAttached(
            "EditingComboBoxColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnComboBoxColumnStyleChanged));

        public static void SetEditingComboBoxColumnStyle(DependencyObject element, Style value) => element.SetValue(EditingComboBoxColumnStyleProperty, value);

        public static Style GetEditingComboBoxColumnStyle(DependencyObject element) => (Style) element.GetValue(EditingComboBoxColumnStyleProperty);

        private static void UpdateComboBoxColumnStyles(DataGrid grid)
        {
            var comboBoxColumnStyle = GetComboBoxColumnStyle(grid);
            var editingComboBoxColumnStyle = GetEditingComboBoxColumnStyle(grid);

            if (comboBoxColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridComboBoxColumn>())
                {
                    var elementStyle = new Style
                    {
                        BasedOn = column.ElementStyle,
                        TargetType = comboBoxColumnStyle.TargetType
                    };

                    foreach (var setter in comboBoxColumnStyle.Setters.OfType<Setter>())
                    {
                        elementStyle.Setters.Add(setter);
                    }

                    column.ElementStyle = elementStyle;
                }
            }

            if (editingComboBoxColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridComboBoxColumn>())
                {
                    var editingElementStyle = new Style
                    {
                        BasedOn = column.EditingElementStyle,
                        TargetType = editingComboBoxColumnStyle.TargetType
                    };

                    foreach (var setter in editingComboBoxColumnStyle.Setters.OfType<Setter>())
                    {
                        editingElementStyle.Setters.Add(setter);
                    }

                    column.EditingElementStyle = editingElementStyle;
                }
            }
        }

        public static readonly DependencyProperty CheckBoxColumnStyleProperty = DependencyProperty.RegisterAttached(
            "CheckBoxColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnCheckBoxColumnStyleChanged));

        private static void OnCheckBoxColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)d;
            if (e.OldValue == null && e.NewValue != null)
            {
                UpdateCheckBoxColumnStyles(grid);
            }
        }

        public static void SetCheckBoxColumnStyle(DependencyObject element, Style value) => element.SetValue(CheckBoxColumnStyleProperty, value);

        public static Style GetCheckBoxColumnStyle(DependencyObject element) => (Style)element.GetValue(CheckBoxColumnStyleProperty);

        public static readonly DependencyProperty EditingCheckBoxColumnStyleProperty = DependencyProperty.RegisterAttached(
            "EditingCheckBoxColumnStyle", typeof(Style), typeof(DataGridAttach), new PropertyMetadata(default(Style), OnCheckBoxColumnStyleChanged));

        public static void SetEditingCheckBoxColumnStyle(DependencyObject element, Style value) => element.SetValue(EditingCheckBoxColumnStyleProperty, value);

        public static Style GetEditingCheckBoxColumnStyle(DependencyObject element) => (Style) element.GetValue(EditingCheckBoxColumnStyleProperty);

        private static void UpdateCheckBoxColumnStyles(DataGrid grid)
        {
            var checkBoxColumnStyle = GetCheckBoxColumnStyle(grid);
            var editingCheckBoxColumnStyle = GetEditingCheckBoxColumnStyle(grid);

            if (checkBoxColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridCheckBoxColumn>())
                {
                    var elementStyle = new Style
                    {
                        BasedOn = column.ElementStyle,
                        TargetType = checkBoxColumnStyle.TargetType
                    };

                    foreach (var setter in checkBoxColumnStyle.Setters.OfType<Setter>())
                    {
                        elementStyle.Setters.Add(setter);
                    }

                    column.ElementStyle = elementStyle;
                }
            }

            if (editingCheckBoxColumnStyle != null)
            {
                foreach (var column in grid.Columns.OfType<DataGridCheckBoxColumn>())
                {
                    var editingElementStyle = new Style
                    {
                        BasedOn = column.EditingElementStyle,
                        TargetType = editingCheckBoxColumnStyle.TargetType
                    };

                    foreach (var setter in editingCheckBoxColumnStyle.Setters.OfType<Setter>())
                    {
                        editingElementStyle.Setters.Add(setter);
                    }

                    column.EditingElementStyle = editingElementStyle;
                }
            }
        }

        public static DependencyProperty ShowRowNumberProperty = DependencyProperty.RegisterAttached("ShowRowNumber",
            typeof(bool), typeof(DataGridAttach),
            new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, OnShowRowNumberChanged));

        public static bool GetShowRowNumber(DependencyObject target) => (bool)target.GetValue(ShowRowNumberProperty);

        public static void SetShowRowNumber(DependencyObject target, bool value) => target.SetValue(ShowRowNumberProperty, value);

        private static void OnShowRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is DataGrid dataGrid)) return;
            var show = (bool)e.NewValue;

            if (show)
            {
                dataGrid.LoadingRow += DataGrid_LoadingRow;
                dataGrid.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
            }
            else
            {
                dataGrid.LoadingRow -= DataGrid_LoadingRow;
                dataGrid.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
            }

            UpdateItems(dataGrid.ItemContainerGenerator, dataGrid.Items.Count, show);
        }

        private static void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (!(sender is ItemContainerGenerator generator)) return;
            UpdateItems(generator, e.ItemCount);
        }

        private static void UpdateItems(ItemContainerGenerator generator, int itemsCount, bool show = true)
        {
            if (show)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    var row = (DataGridRow)generator.ContainerFromIndex(i);
                    if (row != null)
                    {
                        row.Header = (i + 1).ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    var row = (DataGridRow)generator.ContainerFromIndex(i);
                    if (row != null)
                    {
                        row.Header = null;
                    }
                }
            }
        }

        private static void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e) => e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }
}

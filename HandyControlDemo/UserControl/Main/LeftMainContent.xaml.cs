using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;


namespace HandyControlDemo.UserControl
{
    /// <summary>
    ///     左侧主内容
    /// </summary>
    public partial class LeftMainContent
    {
        public LeftMainContent()
        {
            InitializeComponent();
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is TabItem tabItem && tabItem.Content is ListBox listBox)
            {
                if (listBox.SelectedItem != null)
                {
                    var item = listBox.SelectedItem;
                    listBox.SelectedIndex = -1;
                    listBox.SelectedItem = item;
                }
            }
        }

        private void ButtonStyleAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonStyleAscending.IsChecked == true)
            {
                listStyle.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            }
            else
            {
                listStyle.Items.SortDescriptions.Clear();
            }
        }

        private void ButtonControlAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonControlAscending.IsChecked == true)
            {
                listControl.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            }
            else
            {
                listControl.Items.SortDescriptions.Clear();
            }
        }

        private void SearchBarStyle_OnSearchStarted(object sender, FunctionEventArgs<string> e)
        {
            if (e.Info == null) return;

            foreach (var listBoxItem in listStyle.Items.OfType<ListBoxItem>())
            {
                listBoxItem.Show(listBoxItem.Content.ToString().ToLower().Contains(e.Info.ToLower()));
            }
        }

        private void SearchBarControl_OnSearchStarted(object sender, FunctionEventArgs<string> e)
        {
            if (e.Info == null) return;

            foreach (var listBoxItem in listControl.Items.OfType<ListBoxItem>())
            {
                listBoxItem.Show(listBoxItem.Content.ToString().ToLower().Contains(e.Info.ToLower()));
            }
        }
    }
}

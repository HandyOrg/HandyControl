using System.ComponentModel;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
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
            listStyle.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            listControl.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
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
    }
}

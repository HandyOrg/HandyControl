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

            /*You have to ask me why I made Border special.
             What drives me crazy is that if you don't do this, 
             the content of ListBoxItemBorder will always be 1 ! 
             How amazing!
             I wish someone could tell me why. */
            ListBoxItemBorder.Content = Properties.Langs.Lang.Border;
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
                ListBoxStyle.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            }
            else
            {
                ListBoxStyle.Items.SortDescriptions.Clear();
            }
        }

        private void ButtonControlAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonControlAscending.IsChecked == true)
            {
                ListBoxControl.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
            }
            else
            {
                ListBoxControl.Items.SortDescriptions.Clear();
            }
        }

        private void SearchBar_OnSearchStarted(object sender, FunctionEventArgs<string> e)
        {
            if (e.Info == null) return;
            if (!(sender is FrameworkElement searchBar && searchBar.Tag is ListBox listBox)) return;
            foreach (var listBoxItem in listBox.Items.OfType<ListBoxItem>())
            {
                listBoxItem.Show(listBoxItem.Content.ToString().ToLower().Contains(e.Info.ToLower()) ||
                                 listBoxItem.Tag.ToString().Replace("DemoCtl", "").ToLower().Contains(e.Info.ToLower()));
            }
        }
    }
}

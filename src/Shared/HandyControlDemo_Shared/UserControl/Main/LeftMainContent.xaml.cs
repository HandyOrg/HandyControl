using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControl.Tools;
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
            if (e.AddedItems[0] is TabItem tabItem && 
                VisualHelper.GetChild<ListBox>(tabItem.Content as DependencyObject) is ListBox listBox)
            {
                if (listBox.SelectedItem != null)
                {
                    var item = listBox.SelectedItem;
                    listBox.SelectedIndex = -1;
                    listBox.SelectedItem = item;
                }
            }
        }

        private void ButtonAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton button && button.Tag is ItemsControl itemsControl)
            {
                if (button.IsChecked == true)
                {
                    itemsControl.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));
                }
                else
                {
                    itemsControl.Items.SortDescriptions.Clear();
                }
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

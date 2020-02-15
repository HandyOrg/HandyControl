using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;


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
            if (e.AddedItems[0] is DemoInfoModel demoInfo)
            {
                ViewModelLocator.Instance.Main.DemoInfoCurrent = demoInfo;
                var selectedIndex = demoInfo.SelectedIndex;
                demoInfo.SelectedIndex = -1;
                demoInfo.SelectedIndex = selectedIndex;
            }
        }

        private void ButtonAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton button && button.Tag is ItemsControl itemsControl)
            {
                if (button.IsChecked == true)
                {
                    itemsControl.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
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
            foreach (DemoItemModel item in listBox.Items)
            {
                var listBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                listBoxItem?.Show(item.Name.ToLower().Contains(e.Info.ToLower()) ||
                                  item.TargetCtlName.Replace("DemoCtl", "").ToLower().Contains(e.Info.ToLower()));
            }
        }
    }
}
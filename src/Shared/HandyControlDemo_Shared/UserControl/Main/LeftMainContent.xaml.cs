using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;


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

            //Messenger.Default.Register<object>(this, MessageToken.ClearLeftSelected, obj =>
            //{
            //    ListBoxStyle.SelectedItem = null;
            //    ListBoxControl.SelectedItem = null;
            //    ListBoxTool.SelectedItem = null;
            //});
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is DemoInfoModel demoInfo)
            {
                var selectedIndex = demoInfo.SelectedIndex;
                demoInfo.SelectedIndex = -1;
                demoInfo.SelectedIndex = selectedIndex;
            }

            //if (e.OriginalSource is ItemsControl itemsControl)
            //{
            //    if (!(itemsControl.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]) is ContentControl container)) return;
                
            //    var selector = VisualHelper.GetChild<Selector>(container as DependencyObject);
            //    if (selector.SelectedItem != null)
            //    {
            //        var item = selector.SelectedItem;
            //        selector.SelectedIndex = -1;
            //        selector.SelectedItem = item;
            //    }
            //}
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

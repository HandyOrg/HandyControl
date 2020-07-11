using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;


namespace HandyControlDemo.UserControl
{
    /// <summary>
    ///     左侧主内容
    /// </summary>
    public partial class LeftMainContent
    {
        private string _searchKey;

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
                FilterItems();
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
            _searchKey = e.Info;
            FilterItems();
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(_searchKey))
            {
                foreach (var item in ViewModelLocator.Instance.Main.DemoInfoCurrent.DemoItemList)
                {
                    item.IsVisible = true;
                }
            }
            else
            {
                var key = _searchKey.ToLower();
                foreach (var item in ViewModelLocator.Instance.Main.DemoInfoCurrent.DemoItemList)
                {
                    if (item.Name.ToLower().Contains(key))
                    {
                        item.IsVisible = true;
                    }
                    else if (item.TargetCtlName.Replace("DemoCtl", "").ToLower().Contains(key))
                    {
                        item.IsVisible = true;
                    }
                    else
                    {
                        var name = Properties.Langs.LangProvider.GetLang(item.Name);
                        if (!string.IsNullOrEmpty(name) && name.ToLower().Contains(key))
                        {
                            item.IsVisible = true;
                        }
                        else
                        {
                            item.IsVisible = false;
                        }
                    }
                }
            }
        }
    }
}
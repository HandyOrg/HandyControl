using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo.UserControl;

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
            GroupItems(sender as TabControl, demoInfo);
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
                item.QueriesText = string.Empty;
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
                    item.QueriesText = _searchKey;
                }
                else if (item.TargetCtlName.Replace("Demo", "").ToLower().Contains(key))
                {
                    item.IsVisible = true;
                    item.QueriesText = _searchKey;
                }
                else
                {
                    var name = Properties.Langs.LangProvider.GetLang(item.Name);
                    if (!string.IsNullOrEmpty(name) && name.ToLower().Contains(key))
                    {
                        item.IsVisible = true;
                        item.QueriesText = _searchKey;
                    }
                    else
                    {
                        item.IsVisible = false;
                        item.QueriesText = string.Empty;
                    }
                }
            }
        }
    }

    private void GroupItems(TabControl tabControl, DemoInfoModel demoInfo)
    {
        var listBox = VisualHelper.GetChild<ListBox>(tabControl);
        if (listBox == null) return;
        listBox.Items.GroupDescriptions?.Clear();

        if (demoInfo.IsGroupEnabled)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                listBox.Items.GroupDescriptions?.Add(new PropertyGroupDescription("GroupName"));
            }), DispatcherPriority.Background);
        }
    }
}

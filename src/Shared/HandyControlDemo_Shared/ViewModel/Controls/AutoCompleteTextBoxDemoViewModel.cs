using System.Collections.Generic;
using GalaSoft.MvvmLight;
using HandyControl.Collections;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class AutoCompleteTextBoxDemoViewModel : ViewModelBase
{
    private string _searchText;

    public string SearchText
    {
        get => _searchText;
#if NET40
        set
        {
            Set(nameof(SearchText), ref _searchText, value);
            FilterItems(value);
        }
#else
        set
        {
            Set(ref _searchText, value);
            FilterItems(value);
        }
#endif
    }

    public ManualObservableCollection<DemoDataModel> Items { get; set; } = new();

    private readonly List<DemoDataModel> _dataList;

    public AutoCompleteTextBoxDemoViewModel(DataService dataService)
    {
        _dataList = dataService.GetDemoDataList(10);
    }

    private void FilterItems(string key)
    {
        Items.CanNotify = false;

        Items.Clear();

        foreach (var data in _dataList)
        {
            if (data.Name.ToLower().Contains(key.ToLower()))
            {
                Items.Add(data);
            }
        }

        Items.CanNotify = true;
    }
}

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HandyControlDemo.ViewModel;

public class DemoViewModelBase<T> : ObservableObject
{
    private IList<T> _dataList = [];

    public IList<T> DataList
    {
        get => _dataList;
        set => SetProperty(ref _dataList, value);
    }
}

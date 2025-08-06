using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HandyControlDemo.ViewModel;

public partial class DemoViewModelBase<T> : ObservableObject
{
    [ObservableProperty] private IList<T> _dataList = [];
}

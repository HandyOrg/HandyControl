using System.Collections.Generic;
using GalaSoft.MvvmLight;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class AutoCompleteTextBoxDemoViewModel : ViewModelBase
{
    public List<DemoDataModel> Items { get; set; }

    public AutoCompleteTextBoxDemoViewModel(DataService dataService)
    {
        Items = dataService.GetDemoDataList(10);
    }
}

using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class CoverViewModel : DemoViewModelBase<CoverViewDemoModel>
{
    public CoverViewModel(DataService dataService) => DataList = dataService.GetCoverViewDemoDataList();
}

using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class TagDemoViewModel : DemoViewModelBase<DemoDataModel>
    {
        public TagDemoViewModel(DataService dataService)
        {
            DataList = dataService.GetDemoDataList(10);
        }
    }
}
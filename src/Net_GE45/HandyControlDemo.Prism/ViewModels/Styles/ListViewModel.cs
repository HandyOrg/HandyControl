using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ListViewModel : DemoViewModelBase<DemoDataModel>
    {

        public ListViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetDemoDataList());
        }
    }
}

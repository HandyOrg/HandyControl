using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class MenuDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public MenuDemoCtlViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetDemoDataList());
        }
    }
}

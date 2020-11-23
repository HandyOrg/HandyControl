using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class TreeViewDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public TreeViewDemoCtlViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetDemoDataList());
        }
    }
}

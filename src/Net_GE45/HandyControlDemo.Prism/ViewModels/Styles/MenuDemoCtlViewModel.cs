using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class MenuDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public MenuDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetDemoDataList());
        }
    }
}

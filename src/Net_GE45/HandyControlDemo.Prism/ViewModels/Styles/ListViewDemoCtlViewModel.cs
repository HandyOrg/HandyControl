using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ListViewDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {

        public ListViewDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetDemoDataList());
        }
    }
}

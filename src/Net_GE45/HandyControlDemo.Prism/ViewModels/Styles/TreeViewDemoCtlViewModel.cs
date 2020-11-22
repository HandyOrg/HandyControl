using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class TreeViewDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public TreeViewDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetDemoDataList());
        }
    }
}

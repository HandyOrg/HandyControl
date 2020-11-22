using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ListBoxDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public ListBoxDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetDemoDataList());
        }
    }
}

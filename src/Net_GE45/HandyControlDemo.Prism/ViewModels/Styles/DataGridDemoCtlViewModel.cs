using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class DataGridDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public DataGridDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetDemoDataList());
        }
    }
}

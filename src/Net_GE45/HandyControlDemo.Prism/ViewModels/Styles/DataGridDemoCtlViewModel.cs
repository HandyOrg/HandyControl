using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class DataGridDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
    {
        public DataGridDemoCtlViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetDemoDataList());
        }
    }
}

using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ToolBarDemoCtlViewModel : DemoViewModelBase<string>
    {
        public ToolBarDemoCtlViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetComboBoxDemoDataList());
        }
    }
}

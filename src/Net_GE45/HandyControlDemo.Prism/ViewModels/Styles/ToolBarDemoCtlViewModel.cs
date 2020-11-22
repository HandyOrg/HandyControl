using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ToolBarDemoCtlViewModel : DemoViewModelBase<string>
    {
        public ToolBarDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetComboBoxDemoDataList());
        }
    }
}

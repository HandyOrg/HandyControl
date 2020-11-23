using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class NativeComboBoxDemoCtlViewModel : DemoViewModelBase<string>
    {
        public NativeComboBoxDemoCtlViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetComboBoxDemoDataList());
        }
    }
}

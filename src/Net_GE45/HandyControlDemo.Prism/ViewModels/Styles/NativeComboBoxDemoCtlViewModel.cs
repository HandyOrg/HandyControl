using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class NativeComboBoxDemoCtlViewModel : DemoViewModelBase<string>
    {
        public NativeComboBoxDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetComboBoxDemoDataList());
        }
    }
}

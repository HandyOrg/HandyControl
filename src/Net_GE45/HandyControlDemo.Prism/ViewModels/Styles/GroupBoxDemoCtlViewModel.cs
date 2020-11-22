using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class GroupBoxDemoCtlViewModel : DemoViewModelBase<string>
    {

        public GroupBoxDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetComboBoxDemoDataList());
        }
    }
}

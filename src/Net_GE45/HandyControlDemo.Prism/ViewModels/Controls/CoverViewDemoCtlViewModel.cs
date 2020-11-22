using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class CoverViewDemoCtlViewModel : DemoViewModelBase<CoverViewDemoModel>
    {
        public CoverViewDemoCtlViewModel()
        {
            Task.Run(() => DataList = new DataService().GetCoverViewDemoDataList());
        }
    }
}

using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class WebsitesViewModel : DemoViewModelBase<AvatarModel>
    {
        public WebsitesViewModel()
        {
            Task.Run(() => DataList = new DataService().GetWebsiteDataList());
        }
    }
}

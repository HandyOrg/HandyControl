using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class WebsitesViewModel : DemoViewModelBase<AvatarModel>
    {
        public WebsitesViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetWebsiteDataList());
        }
    }
}

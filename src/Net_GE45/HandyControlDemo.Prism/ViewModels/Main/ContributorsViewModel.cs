using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ContributorsViewModel : DemoViewModelBase<AvatarModel>
    {


        public ContributorsViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetContributorDataList()).ContinueWith(obj => DataGot = true);
        }
    }
}

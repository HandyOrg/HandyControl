using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ContributorsViewModel : DemoViewModelBase<AvatarModel>
    {


        public ContributorsViewModel()
        {
            Task.Run(() => DataList = new DataService().GetContributorDataList()).ContinueWith(obj => DataGot = true);
        }
    }
}

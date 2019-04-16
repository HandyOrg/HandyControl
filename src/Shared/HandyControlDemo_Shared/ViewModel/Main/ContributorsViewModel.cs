using System.Threading.Tasks;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ContributorsViewModel : DemoViewModelBase<ContributorModel>
    {
        public ContributorsViewModel(DataService dataService)
        {
#if netle40
            Task.Factory.StartNew(() =>
            {
                DataList = dataService.GetContributorDataList();
            });
#else
            Task.Run(() =>
            {
                DataList = dataService.GetContributorDataList();
            });
#endif
        }
    }
}
using System.Threading;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ContributorsViewModel : DemoViewModelBase<ContributorModel>
    {
        public ContributorsViewModel(DataService dataService)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                DataList = dataService.GetContributorDataList();
            });
        }
    }
}
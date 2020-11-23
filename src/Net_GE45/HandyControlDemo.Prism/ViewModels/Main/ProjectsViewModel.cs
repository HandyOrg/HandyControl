using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ProjectsViewModel : DemoViewModelBase<AvatarModel>
    {
        public ProjectsViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetProjectDataList());
        }
    }
}

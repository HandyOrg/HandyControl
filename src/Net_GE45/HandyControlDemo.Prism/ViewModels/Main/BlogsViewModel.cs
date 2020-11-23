using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class BlogsViewModel : DemoViewModelBase<AvatarModel>
    {

        public BlogsViewModel(DataService dataService)
        {
            Task.Run(() => DataList = dataService.GetBlogDataList());
        }
    }
}

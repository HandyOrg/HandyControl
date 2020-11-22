using HandyControlDemo.Data;
using HandyControlDemo.Service;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class BlogsViewModel : DemoViewModelBase<AvatarModel>
    {

        public BlogsViewModel()
        {
            Task.Run(() => DataList = new DataService().GetBlogDataList());
        }
    }
}

using HandyControlDemo.Data;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class WebsitesViewModel : BindableBase
    {
        private ObservableCollection<AvatarModel> _DataList;
        public ObservableCollection<AvatarModel> DataList
        {
            get { return _DataList; }
            set { SetProperty(ref _DataList, value); }
        }

        public WebsitesViewModel()
        {
            Task.Run(() =>
            {
                DataList = new ObservableCollection<AvatarModel>(GetWebsiteDataList());
            });
        }

        internal List<AvatarModel> GetWebsiteDataList()
        {
            return new List<AvatarModel>
            {
                new AvatarModel
                {
                    DisplayName = "Dotnet9",
                    AvatarUri = "https://pic.cnblogs.com/avatar/1663243/20191124121029.png",
                    Link = "https://dotnet9.com/"
                }
            };
        }
    }
}

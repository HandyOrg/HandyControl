using HandyControlDemo.Data;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class BlogsViewModel : BindableBase
    {
        private ObservableCollection<AvatarModel> _DataList;
        public ObservableCollection<AvatarModel> DataList
        {
            get { return _DataList; }
            set { SetProperty(ref _DataList, value); }
        }

        public BlogsViewModel()
        {
            Task.Run(() =>
            {
                DataList = new ObservableCollection<AvatarModel>(GetBlogDataList());
            });
        }

        internal List<AvatarModel> GetBlogDataList()
        {
            return new List<AvatarModel>
            {
                new AvatarModel
                {
                    DisplayName = "林德熙",
                    AvatarUri = "https://avatars3.githubusercontent.com/u/16054566?s=400&v=4",
                    Link = "https://blog.lindexi.com/"
                },
                new AvatarModel
                {
                    DisplayName = "吕毅",
                    AvatarUri = "https://avatars2.githubusercontent.com/u/9959623?s=400&v=4",
                    Link = "https://blog.walterlv.com/"
                },
                new AvatarModel
                {
                    DisplayName = "DinoChan",
                    AvatarUri = "https://avatars1.githubusercontent.com/u/6076257?s=400&v=4",
                    Link = "https://www.cnblogs.com/dino623/"
                },
                new AvatarModel
                {
                    DisplayName = "noctwolf",
                    AvatarUri = "https://avatars3.githubusercontent.com/u/21022467?s=400&v=4",
                    Link = "https://www.cnblogs.com/noctwolf/"
                }
            };
        }
    }
}

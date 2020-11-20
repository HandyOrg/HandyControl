using HandyControlDemo.Data;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class ProjectsViewModel : BindableBase
    {
        private ObservableCollection<AvatarModel> _DataList;
        public ObservableCollection<AvatarModel> DataList
        {
            get { return _DataList; }
            set { SetProperty(ref _DataList, value); }
        }

        public ProjectsViewModel()
        {
            Task.Run(() =>
            {
                DataList = new ObservableCollection<AvatarModel>(GetProjectDataList());
            });
        }

        internal List<AvatarModel> GetProjectDataList()
        {
            return new List<AvatarModel>
            {
                new AvatarModel
                {
                    DisplayName = "phpEnv",
                    AvatarUri = "https://cdn.phpenv.cn:444/logo.png",
                    Link = "https://www.phpenv.cn/"
                },
                new AvatarModel
                {
                    DisplayName = "AutumnBox",
                    AvatarUri = "https://raw.githubusercontent.com/zsh2401/AutumnBox/master/src/AutumnBox.GUI/Resources/Images/icon.png",
                    Link = "https://github.com/zsh2401/AutumnBox"
                }
            };
        }
    }
}

using HandyControlDemo.Tools;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace HandyControlDemo.ViewModels
{
    public class NonClientAreaContentViewModel : BindableBase
    {
        IRegionManager region;
        private DelegateCommand<string> _OpenViewCmd;
        public DelegateCommand<string> OpenViewCmd =>
            _OpenViewCmd ?? (_OpenViewCmd = new DelegateCommand<string>(OpenView));

        void OpenView(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                region.RequestNavigate("ContentRegion", param);
            }
        }
        private string _VersionInfo;
        public string VersionInfo
        {
            get { return _VersionInfo; }
            set { SetProperty(ref _VersionInfo, value); }
        }

        public NonClientAreaContentViewModel(IRegionManager regionManager)
        {
            region = regionManager;
            VersionInfo = VersionHelper.GetVersion();
        }
    }
}

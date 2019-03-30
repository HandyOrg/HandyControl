using GalaSoft.MvvmLight;
using HandyControlDemo.Data;

namespace HandyControlDemo.ViewModel
{
    public class NotifyIconDemoViewModel : ViewModelBase
    {
        private bool _contextMenuIsShow;

        public bool ContextMenuIsShow
        {
            get => _contextMenuIsShow;
            set
            {
                Set(ref _contextMenuIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextMenuIsBlink;

        public bool ContextMenuIsBlink
        {
            get => _contextMenuIsBlink;
            set => Set(ref _contextMenuIsBlink, value);
        }

        private bool _contextContentIsShow;

        public bool ContextContentIsShow
        {
            get => _contextContentIsShow;
            set
            {
                Set(ref _contextContentIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextContentIsBlink;

        public bool ContextContentIsBlink
        {
            get => _contextContentIsBlink;
            set => Set(ref _contextContentIsBlink, value);
        }
    }
}
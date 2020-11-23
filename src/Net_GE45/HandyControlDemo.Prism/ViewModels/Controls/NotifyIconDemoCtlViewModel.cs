using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class NotifyIconDemoCtlViewModel : BindableBase
    {
        internal static NotifyIconDemoCtlViewModel Instance;
        private bool _isCleanup;

        private bool _reversed;

        private string _content = "Hello~~~";

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private bool _contextMenuIsShow;
        public bool ContextMenuIsShow
        {
            get => _contextMenuIsShow;
            set
            {
                SetProperty(ref _contextMenuIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
                if (!_isCleanup && !_reversed)
                {
                    _reversed = true;
                    ContextContentIsShow = !value;
                    _reversed = false;
                }
            }
        }

        private bool _contextMenuIsBlink;
        public bool ContextMenuIsBlink
        {
            get => _contextMenuIsBlink;
            set => SetProperty(ref _contextMenuIsBlink, value);
        }

        private bool _contextContentIsShow;
        public bool ContextContentIsShow
        {
            get => _contextContentIsShow;
            set
            {
                SetProperty(ref _contextContentIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
                if (!_isCleanup && !_reversed)
                {
                    _reversed = true;
                    ContextMenuIsShow = !value;
                    _reversed = false;
                }
            }
        }

        private bool _contextContentIsBlink;
        public bool ContextContentIsBlink
        {
            get => _contextContentIsBlink;
            set => SetProperty(ref _contextContentIsBlink, value);
        }

        private DelegateCommand<object> _MouseCmd;
        public DelegateCommand<object> MouseCmd =>
            _MouseCmd ?? (_MouseCmd = new DelegateCommand<object>(str => Growl.Info(str.ToString())));

        private DelegateCommand _SendNotificationCmd;
        public DelegateCommand SendNotificationCmd =>
            _SendNotificationCmd ?? (_SendNotificationCmd = new DelegateCommand(SendNotification));

        public NotifyIconDemoCtlViewModel()
        {
            Instance = this;
        }

        private void SendNotification()
        {
            NotifyIcon.ShowBalloonTip("HandyControl", Content, NotifyIconInfoType.None, ContextMenuIsShow ? "NotifyIconDemo" : "NotifyIconContextDemo");
        }

        public void Cleanup()
        {
            _isCleanup = true;
            ContextMenuIsShow = false;
            ContextMenuIsBlink = false;
            ContextContentIsShow = false;
            ContextContentIsBlink = false;
            GlobalData.NotifyIconIsShow = false;
            _isCleanup = false;
        }
    }
}

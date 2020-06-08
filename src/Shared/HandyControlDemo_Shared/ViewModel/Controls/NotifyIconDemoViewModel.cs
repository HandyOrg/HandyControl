using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.Data;

namespace HandyControlDemo.ViewModel
{
    public class NotifyIconDemoViewModel : ViewModelBase
    {
        private bool _isCleanup;

        private bool _reversed;

        private string _content = "Hello~~~";

        public string Content
        {
            get => _content;
#if NET40
            set => Set(nameof(Content), ref _content, value);
#else
            set => Set(ref _content, value);
#endif
        }

        private bool _contextMenuIsShow;

        public bool ContextMenuIsShow
        {
            get => _contextMenuIsShow;
            set
            {
#if NET40
                Set(nameof(ContextMenuIsShow), ref _contextMenuIsShow, value);
#else
                Set(ref _contextMenuIsShow, value);
#endif
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
#if NET40
            set => Set(nameof(ContextMenuIsBlink), ref _contextMenuIsBlink, value);
#else
            set => Set(ref _contextMenuIsBlink, value);
#endif
        }

        private bool _contextContentIsShow;

        public bool ContextContentIsShow
        {
            get => _contextContentIsShow;
            set
            {
#if NET40
                Set(nameof(ContextContentIsShow), ref _contextContentIsShow, value);
#else
                Set(ref _contextContentIsShow, value);
#endif
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
#if NET40
            set => Set(nameof(ContextContentIsBlink), ref _contextContentIsBlink, value);
#else
            set => Set(ref _contextContentIsBlink, value);
#endif
        }

        public RelayCommand<object> MouseCmd => new Lazy<RelayCommand<object>>(() =>
            new RelayCommand<object>(str=> Growl.Info(str.ToString()))).Value;

        public RelayCommand SendNotificationCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(SendNotification)).Value;

        private void SendNotification()
        {
            NotifyIcon.ShowBalloonTip("HandyControl", Content, NotifyIconInfoType.None, ContextMenuIsShow ? MessageToken.NotifyIconDemo : MessageToken.NotifyIconContextDemo);
        }

        public override void Cleanup()
        {
            base.Cleanup();

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
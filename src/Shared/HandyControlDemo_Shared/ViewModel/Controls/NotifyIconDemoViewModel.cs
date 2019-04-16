using System;
using GalaSoft.MvvmLight;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif
using HandyControl.Controls;
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
#if netle40
                Set(nameof(ContextMenuIsShow), ref _contextMenuIsShow, value);
#else
                Set(ref _contextMenuIsShow, value);
#endif
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextMenuIsBlink;

        public bool ContextMenuIsBlink
        {
            get => _contextMenuIsBlink;
#if netle40
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
#if netle40
                Set(nameof(ContextContentIsShow), ref _contextContentIsShow, value);
#else
                Set(ref _contextContentIsShow, value);
#endif
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextContentIsBlink;

        public bool ContextContentIsBlink
        {
            get => _contextContentIsBlink;
#if netle40
            set => Set(nameof(ContextContentIsBlink), ref _contextContentIsBlink, value);
#else
            set => Set(ref _contextContentIsBlink, value);
#endif
        }

        public RelayCommand<object> MouseCmd => new Lazy<RelayCommand<object>>(() =>
            new RelayCommand<object>(str=> Growl.Info(str.ToString()))).Value;
    }
}
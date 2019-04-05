using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
                Set(nameof(ContextMenuIsShow), ref _contextMenuIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextMenuIsBlink;

        public bool ContextMenuIsBlink
        {
            get => _contextMenuIsBlink;
            set => Set(nameof(ContextMenuIsBlink), ref _contextMenuIsBlink, value);
        }

        private bool _contextContentIsShow;

        public bool ContextContentIsShow
        {
            get => _contextContentIsShow;
            set
            {
                Set(nameof(ContextContentIsShow), ref _contextContentIsShow, value);
                GlobalData.NotifyIconIsShow = ContextMenuIsShow || ContextContentIsShow;
            }
        }

        private bool _contextContentIsBlink;

        public bool ContextContentIsBlink
        {
            get => _contextContentIsBlink;
            set => Set(nameof(ContextContentIsBlink), ref _contextContentIsBlink, value);
        }

        public RelayCommand<object> MouseCmd => new Lazy<RelayCommand<object>>(() =>
            new RelayCommand<object>(str=> Growl.Info(str.ToString()))).Value;
    }
}
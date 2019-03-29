using GalaSoft.MvvmLight;
using HandyControlDemo.Data;

namespace HandyControlDemo.ViewModel
{
    public class NotifyIconDemoViewModel : ViewModelBase
    {
        private bool _isShow;

        public bool IsShow
        {
            get => _isShow;
            set
            {
                Set(ref _isShow, value);
                GlobalData.NotifyIconIsShow = value;
            }
        }
    }
}
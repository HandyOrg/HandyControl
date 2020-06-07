using GalaSoft.MvvmLight;

namespace HandyControlDemo.Data
{
    public class DemoItemModel : ObservableObject
    {
        public string Name { get; set; }

        public string TargetCtlName { get; set; }

        public string ImageName { get; set; }

        public bool IsNew { get; set; }

        private bool _isVisible = true;

        public bool IsVisible
        {
            get => _isVisible;
#if NET40
            set => Set(nameof(IsVisible), ref _isVisible, value);
#else
            set => Set(ref _isVisible, value);
#endif
        }
    }
}

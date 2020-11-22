using Prism.Mvvm;

namespace HandyControlDemo.Data
{
    public class DemoItemModel : BindableBase
    {
        public string Name { get; set; }

        public string TargetCtlName { get; set; }

        public string ImageName { get; set; }

        public bool IsNew { get; set; }

        private bool _isVisible = true;

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
    }
}

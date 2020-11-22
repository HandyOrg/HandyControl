using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "HandyControl Demo";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public MainWindowViewModel()
        {
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class SearchBarDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> SearchCmd => new(Search);

        private void Search(string key)
        {
            Growl.Info(key);
        }
    }
}

using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class SearchBarDemoCtlViewModel : BindableBase
    {
        private DelegateCommand<string> _SearchCmd;
        public DelegateCommand<string> SearchCmd =>
            _SearchCmd ?? (_SearchCmd = new DelegateCommand<string>(Search));

        void Search(string key)
        {
            Growl.Info(key);
        }
    }
}

using HandyControl.Controls;
using HandyControlDemo.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace HandyControlDemo.ViewModels
{
    public class SpriteDemoCtlViewModel : BindableBase
    {
        private DelegateCommand _OpenCmd;
        public DelegateCommand OpenCmd =>
            _OpenCmd ?? (_OpenCmd = new DelegateCommand(() => Sprite.Show(new AppSprite())));
    }
}

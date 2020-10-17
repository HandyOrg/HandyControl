using HandyControl.Controls;
using HandyControlDemo.UserControl;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class SpriteDemoViewModel
    {
        public RelayCommand OpenCmd => new RelayCommand(() => Sprite.Show(new AppSprite()));
    }
}
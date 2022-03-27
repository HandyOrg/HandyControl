using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel;

public class SpriteDemoViewModel
{
    public RelayCommand OpenCmd => new(() => Sprite.Show(new AppSprite()));
}

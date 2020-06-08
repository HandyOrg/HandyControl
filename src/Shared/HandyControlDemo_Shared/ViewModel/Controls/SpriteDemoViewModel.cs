using System;
using HandyControl.Controls;
using HandyControlDemo.UserControl;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class SpriteDemoViewModel
    {
        public RelayCommand OpenCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(()=> Sprite.Show(new AppSprite()))).Value;
    }
}
using System;
using HandyControl.Controls;
using HandyControlDemo.UserControl;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif

namespace HandyControlDemo.ViewModel
{
    public class SpriteDemoViewModel
    {
        public RelayCommand OpenCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(()=> Sprite.Show(new AppSprite()))).Value;
    }
}
using System;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class ImageBrowserDemoViewModel
    {
        public RelayCommand OpenImgCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() =>
                new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show())).Value;
    }
}
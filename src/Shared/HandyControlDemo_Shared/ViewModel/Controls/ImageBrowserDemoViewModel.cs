using System;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel;

public class ImageBrowserDemoViewModel
{
    public RelayCommand OpenImgCmd => new(() =>
        new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show());
}

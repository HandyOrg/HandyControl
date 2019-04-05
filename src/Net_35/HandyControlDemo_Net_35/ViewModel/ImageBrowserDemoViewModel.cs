using System;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.ViewModel
{
    public class ImageBrowserDemoViewModel
    {
        private RelayCommand _openImgCmd;

        public RelayCommand OpenImgCmd =>
            _openImgCmd ?? (_openImgCmd = new RelayCommand(() =>
                new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show()));
    }
}
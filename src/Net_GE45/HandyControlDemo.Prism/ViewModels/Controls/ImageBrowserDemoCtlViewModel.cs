using HandyControl.Controls;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace HandyControlDemo.ViewModels
{
    public class ImageBrowserDemoCtlViewModel : BindableBase
    {
        private DelegateCommand _OpenImgCmd;
        public DelegateCommand OpenImgCmd =>
            _OpenImgCmd ?? (_OpenImgCmd = new DelegateCommand(() =>
                new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show()));
    }
}

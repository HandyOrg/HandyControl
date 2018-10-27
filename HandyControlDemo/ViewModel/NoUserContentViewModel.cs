using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace HandyControlDemo.ViewModel
{
    public class NoUserContentViewModel : ViewModelBase
    {
        public NoUserContentViewModel()
        {
            NugetDownloadCount = 11111;
        }

        private int _nugetDownloadCount;

        public int NugetDownloadCount
        {
            get => _nugetDownloadCount;
            set => Set(ref _nugetDownloadCount, value);
        }
    }
}
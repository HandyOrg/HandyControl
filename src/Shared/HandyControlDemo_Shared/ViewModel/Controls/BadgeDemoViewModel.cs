using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel
{
    public class BadgeDemoViewModel : ViewModelBase
    {
        private int _count = 1;

        public int Count
        {
            get => _count;
#if NET40
            set => Set(nameof(Count), ref _count, value);
#else
            set => Set(ref _count, value);
#endif
        }

        public RelayCommand CountCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Count++)).Value;
    }
}

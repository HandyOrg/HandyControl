using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace HandyControlDemo.ViewModel.Controls
{
    public class BadgeDemoViewModel : ViewModelBase
    {
        private int _count = 1;

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public RelayCommand CountCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Count++)).Value;
    }
}

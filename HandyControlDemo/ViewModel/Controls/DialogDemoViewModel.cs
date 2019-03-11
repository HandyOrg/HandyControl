using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HandyControl.Controls;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel
{
    public class DialogDemoViewModel : ViewModelBase
    {
        public RelayCommand ShowTextCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(ShowText)).Value;

        private void ShowText()
        {
            Dialog.Show(new TextDialog());
        }
    }
}
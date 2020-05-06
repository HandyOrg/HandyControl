using System;
using GalaSoft.MvvmLight.Command;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class CardDemoViewModel : DemoViewModelBase<CardModel>
    {
        private readonly DataService _dataService;

        public CardDemoViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = dataService.GetCardDataList();
        }

        public RelayCommand AddItemCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => DataList.Insert(0, _dataService.GetCardData()))).Value;

        public RelayCommand RemoveItemCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() =>
            {
                if (DataList.Count > 0)
                {
                    DataList.RemoveAt(0);
                }
            })).Value;
    }
}

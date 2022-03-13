using GalaSoft.MvvmLight.Command;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class CardDemoViewModel : DemoViewModelBase<CardModel>
{
    private readonly DataService _dataService;

    public CardDemoViewModel(DataService dataService)
    {
        _dataService = dataService;
        DataList = dataService.GetCardDataList();
    }

    public RelayCommand AddItemCmd => new(() => DataList.Insert(0, _dataService.GetCardData()));

    public RelayCommand RemoveItemCmd => new(() =>
    {
        if (DataList.Count > 0)
        {
            DataList.RemoveAt(0);
        }
    });
}

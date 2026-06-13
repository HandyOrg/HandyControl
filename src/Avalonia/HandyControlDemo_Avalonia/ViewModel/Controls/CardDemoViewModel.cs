using CommunityToolkit.Mvvm.Input;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public partial class CardDemoViewModel : DemoViewModelBase<CardModel>
{
    private readonly DataService _dataService;

    public CardDemoViewModel(DataService dataService)
    {
        _dataService = dataService;
        DataList = dataService.GetCardDataList();
    }

    [RelayCommand]
    private void AddItem() => DataList.Insert(0, _dataService.GetCardData());

    [RelayCommand]
    private void RemoveItem()
    {
        if (DataList.Count > 0)
        {
            DataList.RemoveAt(0);
        }
    }
}

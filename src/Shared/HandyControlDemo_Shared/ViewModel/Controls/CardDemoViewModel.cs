using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class CardDemoViewModel : DemoViewModelBase<CardModel>
    {
        public CardDemoViewModel(DataService dataService)
        {
            DataList = dataService.GetCardDataList();
        }
    }
}

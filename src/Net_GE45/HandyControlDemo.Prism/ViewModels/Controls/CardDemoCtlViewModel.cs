using HandyControlDemo.Data;
using HandyControlDemo.Service;
using Prism.Commands;
using System.Threading.Tasks;

namespace HandyControlDemo.ViewModels
{
    public class CardDemoCtlViewModel : DemoViewModelBase<CardModel>
    {
        DataService dataService;

        private DelegateCommand _AddItemCmd;
        public DelegateCommand AddItemCmd =>
            _AddItemCmd ?? (_AddItemCmd = new DelegateCommand(AddItem));

        private DelegateCommand _RemoveItemCmd;
        public DelegateCommand RemoveItemCmd =>
            _RemoveItemCmd ?? (_RemoveItemCmd = new DelegateCommand(RemoveItem));

        public CardDemoCtlViewModel(DataService _dataService)
        {
            dataService = _dataService;
            Task.Run(() => DataList = dataService.GetCardDataList());
        }

        void RemoveItem()
        {
            if (DataList.Count > 0)
            {
                DataList.RemoveAt(0);
            }
        }
        void AddItem()
        {
            DataList.Insert(0, dataService.GetCardData());
        }
    }
}

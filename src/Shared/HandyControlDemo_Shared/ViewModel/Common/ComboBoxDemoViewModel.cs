using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ComboBoxDemoViewModel : DemoViewModelBase<string>
    {
        public ComboBoxDemoViewModel(DataService dataService) => DataList = dataService.GetComboBoxDemoDataList();
    }
}

using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Service;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace HandyControlDemo.ViewModels
{
    public class TagDemoCtlViewModel : BindableBase
    {
        public TagDemoCtlViewModel(DataService dataService)
        {
            DataList = new ObservableCollection<DemoDataModel>(dataService.GetDemoDataList(10));
        }

        public ObservableCollection<DemoDataModel> DataList { get; set; }

        private string _tagName;
        public string TagName
        {
            get => _tagName;
            set => SetProperty(ref _tagName, value);
        }

        private DelegateCommand _AddItemCmd;
        public DelegateCommand AddItemCmd =>
            _AddItemCmd ?? (_AddItemCmd = new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(TagName))
                {
                    Growl.Warning(Lang.PlsEnterContent);
                    return;
                }

                DataList.Insert(0, new DemoDataModel
                {
                    IsSelected = DataList.Count % 2 == 0,
                    Name = TagName
                });
                TagName = string.Empty;
            }));
    }
}
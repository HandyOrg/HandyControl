using HandyControl.Data;
using HandyControlDemo.Data;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace HandyControlDemo.ViewModels
{
    public class LeftMainContentViewModel : BindableBase
    {
        internal static LeftMainContentViewModel Instance;
        public DemoInfoModel DemoInfoCurrent { get; set; }
        public DemoItemModel DemoItemCurrent { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _searchKey;

        private ObservableCollection<DemoInfoModel> _DemoInfoCollection;
        public ObservableCollection<DemoInfoModel> DemoInfoCollection
        {
            get { return _DemoInfoCollection; }
            set { SetProperty(ref _DemoInfoCollection, value); }
        }

        private DelegateCommand<SelectionChangedEventArgs> _SwitchCmd;
        public DelegateCommand<SelectionChangedEventArgs> SwitchCmd =>
            _SwitchCmd ?? (_SwitchCmd = new DelegateCommand<SelectionChangedEventArgs>(OnSwitch));

        private DelegateCommand<FunctionEventArgs<string>> _OnSearchStartedCmd;
        public DelegateCommand<FunctionEventArgs<string>> OnSearchStartedCmd =>
            _OnSearchStartedCmd ?? (_OnSearchStartedCmd = new DelegateCommand<FunctionEventArgs<string>>(OnSearchStarted));

        private DelegateCommand<SelectionChangedEventArgs> _TabItemChangedCmd;
        public DelegateCommand<SelectionChangedEventArgs> TabItemChangedCmd =>
            _TabItemChangedCmd ?? (_TabItemChangedCmd = new DelegateCommand<SelectionChangedEventArgs>(OnTabItemChanged));

        private void OnTabItemChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            if (e.AddedItems[0] is DemoInfoModel demoInfo)
            {
                DemoInfoCurrent = demoInfo;
                var selectedIndex = demoInfo.SelectedIndex;
                demoInfo.SelectedIndex = -1;
                demoInfo.SelectedIndex = selectedIndex;
                FilterItems();
            }
        }

        IRegionManager region;

        public LeftMainContentViewModel(IRegionManager regionManager)
        {
            Instance = this;
            region = regionManager;
            DemoInfoCollection = new ObservableCollection<DemoInfoModel>();
            foreach (var item in GetDemoInfo())
            {
                DemoInfoCollection.Add(item);
            }
        }
        private void OnSwitch(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DemoItemModel item)
            {
                if (item.TargetCtlName != null)
                {
                    DemoItemCurrent = item;
                    Name = item.Name;
                    region.RequestNavigate("ContentRegion", item.TargetCtlName);
                }
            }
        }

        internal List<DemoInfoModel> GetDemoInfo()
        {
            var infoList = new List<DemoInfoModel>();

            var byt = Properties.Resources.DemoInfo;
            Stream stream = new MemoryStream(byt);
            if (stream == null)
            {
                return infoList;
            }

            string jsonStr;
            using (var reader = new StreamReader(stream))
            {
                jsonStr = reader.ReadToEnd();
            }

            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            foreach (var item in jsonObj)
            {
                var titleKey = (string)item.title;
                var title = titleKey;
                var list = Convert2DemoItemList(item.demoItemList);
                infoList.Add(new DemoInfoModel
                {
                    Key = titleKey,
                    Title = title,
                    DemoItemList = list,
                    SelectedIndex = (int)item.selectedIndex
                });
            }

            return infoList;
        }

        private List<DemoItemModel> Convert2DemoItemList(dynamic list)
        {
            var resultList = new List<DemoItemModel>();

            foreach (var item in list)
            {
                var name = (string)item[0];
                string targetCtlName = item[1];
                string imageName = item[2];
                var isNew = !string.IsNullOrEmpty((string)item[3]);
                resultList.Add(new DemoItemModel
                {
                    Name = name,
                    TargetCtlName = targetCtlName,
                    ImageName = $"../../Resources/Img/LeftMainContent/{imageName}.png",
                    IsNew = isNew
                });
            }

            return resultList;
        }

        private void OnSearchStarted(FunctionEventArgs<string> e)
        {
            _searchKey = e.Info;
            FilterItems();
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(_searchKey))
            {
                foreach (var item in DemoInfoCollection)
                {
                    item.DemoItemList[0].IsVisible = true;
                }
            }
            else
            {
                var key = _searchKey.ToLower();
                foreach (var itemCollection in DemoInfoCollection)
                {
                    foreach (var item in itemCollection.DemoItemList)
                    {
                        if (item.Name.ToLower().Contains(key))
                        {
                            item.IsVisible = true;
                        }
                        else if (item.TargetCtlName.Replace("DemoCtl", "").ToLower().Contains(key))
                        {
                            item.IsVisible = true;
                        }
                        else
                        {
                            var name = item.Name;
                            if (!string.IsNullOrEmpty(name) && name.ToLower().Contains(key))
                            {
                                item.IsVisible = true;
                            }
                            else
                            {
                                item.IsVisible = false;
                            }
                        }
                    }
                }
            }
        }
    }
}

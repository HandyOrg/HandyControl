using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace HandyControlDemo.Data
{
    public class DemoInfoModel : ViewModelBase
    {
        public string Key { get; set; }

        private string _title;

        [JsonProperty("title")]
        public string Title
        {
            get => _title;
#if NET35 || NET40
            set => Set(nameof(Title), ref _title, value);
#else
            set => Set(ref _title, value);
#endif
        }

        private int _selectedIndex;

        [JsonProperty("selectedIndex")]
        public int SelectedIndex
        {
            get => _selectedIndex;
#if NET35 || NET40
            set => Set(nameof(SelectedIndex), ref _selectedIndex, value);
#else
            set => Set(ref _selectedIndex, value);
#endif   
        }

        [JsonIgnore]
        public IList<DemoItemModel> DemoItemList { get; set; }

        [JsonProperty("demoItemList")]
        public IList<List<string>> DemoItemStrList { get; set; }
    }
}
using Prism.Mvvm;
using System.Collections.Generic;

namespace HandyControlDemo.Data
{
    public class DemoInfoModel : BindableBase
    {
        public string Key { get; set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public IList<DemoItemModel> DemoItemList { get; set; }
    }
}
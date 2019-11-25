﻿using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace HandyControlDemo.Data
{
    public class DemoInfoModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;
#if netle40
            set => Set(nameof(Title), ref _title, value);
#else
            set => Set(ref _title, value);
#endif
        }

        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
#if netle40
            set => Set(nameof(SelectedIndex), ref _selectedIndex, value);
#else
            set => Set(ref _selectedIndex, value);
#endif   
        }

        public IList<DemoItemModel> DemoItemList { get; set; }
    }
}
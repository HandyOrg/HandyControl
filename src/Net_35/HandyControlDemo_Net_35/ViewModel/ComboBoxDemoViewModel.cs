using System.Collections.Generic;
using GalaSoft.MvvmLight;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ComboBoxDemoViewModel : ViewModelBase
    {
        /// <summary>
        ///     数据列表
        /// </summary>
        private List<string> _dataList;

        /// <summary>
        ///     数据列表
        /// </summary>
        public List<string> DataList
        {
            get => _dataList;
            set => Set(nameof(DataList), ref _dataList, value);
        }

        public ComboBoxDemoViewModel(DataService dataService) => DataList = dataService.GetComboBoxDemoDataList();
    }
}
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class PaginationDemoViewModel : ViewModelBase
    {
        /// <summary>
        ///     所有数据
        /// </summary>
        private readonly List<DemoDataModel> _totalDataList;

        /// <summary>
        ///     显示的数据
        /// </summary>
        private List<DemoDataModel> _dataList;

        /// <summary>
        ///     显示的数据
        /// </summary>
        public List<DemoDataModel> DataList
        {
            get => _dataList;
            set => Set(nameof(DataList), ref _dataList, value);
        }

        /// <summary>
        ///     页码
        /// </summary>
        private int _pageIndex = 1;

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => Set(nameof(PageIndex), ref _pageIndex, value);
        }

        public PaginationDemoViewModel(DataService dataService)
        {
            _totalDataList = dataService.GetDemoDataList(100);
            DataList = _totalDataList.Take(10).ToList();
        }

        private RelayCommand<FunctionEventArgs<int>> _pageUpdatedCmd;

        /// <summary>
        ///     页码改变命令
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCmd =>
            _pageUpdatedCmd ?? (_pageUpdatedCmd = new RelayCommand<FunctionEventArgs<int>>(PageUpdated));

        /// <summary>
        ///     页码改变
        /// </summary>
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            DataList = _totalDataList.Skip((info.Info - 1) * 10).Take(10).ToList();
        }
    }
}
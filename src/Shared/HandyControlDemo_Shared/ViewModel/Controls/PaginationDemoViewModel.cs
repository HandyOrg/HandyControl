using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class PaginationDemoViewModel : DemoViewModelBase<DemoDataModel>
    {
        /// <summary>
        ///     所有数据
        /// </summary>
        private readonly List<DemoDataModel> _totalDataList;

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
#if NET40
            set => Set(nameof(PageIndex), ref _pageIndex, value);
#else
            set => Set(ref _pageIndex, value);
#endif
        }

        public PaginationDemoViewModel(DataService dataService)
        {
            _totalDataList = dataService.GetDemoDataList(100);
            DataList = _totalDataList.Take(10).ToList();
        }

        /// <summary>
        ///     页码改变命令
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCmd =>
            new Lazy<RelayCommand<FunctionEventArgs<int>>>(() =>
                new RelayCommand<FunctionEventArgs<int>>(PageUpdated)).Value;

        /// <summary>
        ///     页码改变
        /// </summary>
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            DataList = _totalDataList.Skip((info.Info - 1) * 10).Take(10).ToList();
        }
    }
}
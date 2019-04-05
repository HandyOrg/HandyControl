using System.Collections.Generic;
using System.Threading;
using GalaSoft.MvvmLight;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ContributorsViewModel : ViewModelBase
    {
        /// <summary>
        ///     数据列表
        /// </summary>
        private List<ContributorModel> _dataList;

        /// <summary>
        ///     数据列表
        /// </summary>
        public List<ContributorModel> DataList
        {
            get => _dataList;
            set => Set(nameof(DataList), ref _dataList, value);
        }

        public ContributorsViewModel(DataService dataService)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                DataList = dataService.GetContributorDataList();
            });
        }
    }
}
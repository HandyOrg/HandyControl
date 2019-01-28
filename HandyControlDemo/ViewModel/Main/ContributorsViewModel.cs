using System.Collections.Generic;
using System.Threading.Tasks;
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
            set => Set(ref _dataList, value);
        }

        public ContributorsViewModel(DataService dataService)
        {
            Task.Run(() =>
            {
                DataList = dataService.GetContributorDataList();
            });
        }
    }
}
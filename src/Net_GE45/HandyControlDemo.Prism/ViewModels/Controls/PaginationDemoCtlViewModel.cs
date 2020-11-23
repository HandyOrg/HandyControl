using HandyControl.Data;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using Prism.Commands;
using System.Collections.Generic;
using System.Linq;

namespace HandyControlDemo.ViewModels
{
    public class PaginationDemoCtlViewModel : DemoViewModelBase<DemoDataModel>
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
            set => SetProperty(ref _pageIndex, value);
        }

        public PaginationDemoCtlViewModel(DataService dataService)
        {
            _totalDataList = dataService.GetDemoDataList(100);
            DataList = _totalDataList.Take(10).ToList();
        }

        private DelegateCommand<FunctionEventArgs<int>> _PageUpdatedCmd;
        public DelegateCommand<FunctionEventArgs<int>> PageUpdatedCmd =>
            _PageUpdatedCmd ?? (_PageUpdatedCmd = new DelegateCommand<FunctionEventArgs<int>>(PageUpdated));

        /// <summary>
        ///     页码改变
        /// </summary>
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            DataList = _totalDataList.Skip((info.Info - 1) * 10).Take(10).ToList();
        }
    }
}

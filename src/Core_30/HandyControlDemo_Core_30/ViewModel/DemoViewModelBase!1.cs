using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace HandyControlDemo.ViewModel
{
    public class DemoViewModelBase<T> : ViewModelBase
    {
        /// <summary>
        ///     数据列表
        /// </summary>
        private IList<T> _dataList;

        /// <summary>
        ///     数据列表
        /// </summary>
        public IList<T> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
        }
    }
}
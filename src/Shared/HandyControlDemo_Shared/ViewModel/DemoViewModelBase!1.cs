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
#if NET40
            set => Set(nameof(DataList), ref _dataList, value);
#else
            set => Set(ref _dataList, value);
#endif       
        }
    }
}
using Prism.Mvvm;
using System.Collections.Generic;

namespace HandyControlDemo.ViewModels
{
    public class DemoViewModelBase<T> : BindableBase
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
            set => SetProperty(ref _dataList, value);
        }

        private bool _dataGot;

        public bool DataGot
        {
            get => _dataGot;
            set => SetProperty(ref _dataGot, value);
        }
    }
}
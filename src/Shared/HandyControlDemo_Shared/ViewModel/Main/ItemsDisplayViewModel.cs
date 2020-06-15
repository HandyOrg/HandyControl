using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandyControlDemo.Data;

namespace HandyControlDemo.ViewModel
{
    public class ItemsDisplayViewModel : DemoViewModelBase<AvatarModel>
    {
        public ItemsDisplayViewModel(Func<List<AvatarModel>> getDataAction)
        {
#if NET40
            Task.Factory.StartNew(() => DataList = getDataAction?.Invoke()).ContinueWith(obj => DataGot = true);
#else
            Task.Run(() => DataList = getDataAction?.Invoke()).ContinueWith(obj => DataGot = true);
#endif
        }

        private bool _dataGot;

        public bool DataGot
        {
            get => _dataGot;
#if NET40
            set => Set(nameof(DataGot), ref _dataGot, value);
#else
            set => Set(ref _dataGot, value);
#endif
        }
    }
}
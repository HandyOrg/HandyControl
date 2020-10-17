using System;
using System.Collections.Generic;
using HandyControlDemo.Data;
#if !NET35
using System.Threading.Tasks;
#else
using System.ComponentModel;
#endif

namespace HandyControlDemo.ViewModel
{
    public class ItemsDisplayViewModel : DemoViewModelBase<AvatarModel>
    {
        public ItemsDisplayViewModel(Func<List<AvatarModel>> getDataAction)
        {
#if NET35
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) => DataList = getDataAction?.Invoke();
            worker.RunWorkerCompleted += (s, e) => DataGot = true;
            worker.RunWorkerAsync();
#elif NET40
            Task.Factory.StartNew(() => DataList = getDataAction?.Invoke()).ContinueWith(obj => DataGot = true);
#else
            Task.Run(() => DataList = getDataAction?.Invoke()).ContinueWith(obj => DataGot = true);
#endif
        }

        private bool _dataGot;

        public bool DataGot
        {
            get => _dataGot;
#if NET35 || NET40
            set => Set(nameof(DataGot), ref _dataGot, value);
#else
            set => Set(ref _dataGot, value);
#endif
        }
    }
}
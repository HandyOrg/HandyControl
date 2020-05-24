using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandyControlDemo.Data;

namespace HandyControlDemo.ViewModel
{
    public class ItemsDisplayViewModel : DemoViewModelBase<AvatarModel>
    {
        public ItemsDisplayViewModel(Func<List<AvatarModel>> getDatAction)
        {
#if NET40
            Task.Factory.StartNew(() => DataList = getDatAction?.Invoke());
#else
            Task.Run(() => DataList = getDatAction?.Invoke());
#endif
        }
    }
}
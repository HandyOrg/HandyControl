using System.Collections.Generic;

namespace HandyControlDemo.Data
{
    public class DemoInfoModel
    {
        public string Title { get; set; }

        public int SelectedIndex { get; set; }

        public IList<DemoItemModel> DemoItemList { get; set; }
    }
}

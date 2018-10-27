using System.Collections.Generic;
using System.Globalization;
using HandyControlDemo.Data;
using HandyControlDemo.Tools.Converter;

namespace HandyControlDemo.Service
{
    public class DataService
    {
        public List<DemoDataModel> GetDemoDataList()
        {
            var list = new List<DemoDataModel>();
            for (var i = 1; i <= 6; i++)
            {
                var dataList = new List<DemoDataModel>();
                for (int j = 0; j < 3; j++)
                {
                    dataList.Add(new DemoDataModel
                    {
                        Index = j,
                        IsSelected = j % 2 == 0,
                        Name = $"SubName{j}",
                        Type = (DemoType)j
                    });
                }
                var model = new DemoDataModel
                {
                    Index = i,
                    IsSelected = i % 2 == 0,
                    Name = $"Name{i}",
                    Type = (DemoType)i,
                    DataList = dataList,
                    ImgPath = $"/HandyControlDemo;component/Resources/Img/Avatar/avatar{i}.png",
                    Remark = new string(i.ToString()[0], 10)
                };
                list.Add(model);
            }

            return list;
        }

        public List<string> GetComboBoxDemoDataList()
        {
            var converter = new StringRepeatConverter();
            var list = new List<string>();
            for (var i = 1; i <= 9; i++)
            {
                list.Add(converter.Convert(Properties.Langs.Lang.Text, null, i, CultureInfo.CurrentCulture)?.ToString());
            }

            return list;
        }
    }
}
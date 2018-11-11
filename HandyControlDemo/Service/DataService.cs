using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using HandyControlDemo.Data;
using HandyControlDemo.Tools.Converter;
using Newtonsoft.Json;

namespace HandyControlDemo.Service
{
    public class DataService
    {
        internal List<DemoDataModel> GetDemoDataList()
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

        internal List<string> GetComboBoxDemoDataList()
        {
            var converter = new StringRepeatConverter();
            var list = new List<string>();
            for (var i = 1; i <= 9; i++)
            {
                list.Add(converter.Convert(Properties.Langs.Lang.Text, null, i, CultureInfo.CurrentCulture)?.ToString());
            }

            return list;
        }

        internal List<ContributorModel> GetContributorDataList()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "request");
            var list = new List<ContributorModel>();
            try
            {
                var json = client.DownloadString(new Uri("https://api.github.com/repos/nabian/handycontrol/contributors"));
                var objList = JsonConvert.DeserializeObject<List<dynamic>>(json);
                list.AddRange(objList.Select(item => new ContributorModel
                {
                    UserName = item.login,
                    AvatarUri = item.avatar_url,
                    Link = item.html_url
                }));
            }
            catch
            {
                // ignored
            }
            return list;
        }

        internal List<StepBarDemoModel> GetStepBarDemoDataList()
        {
            return new List<StepBarDemoModel>
            {
                new StepBarDemoModel
                {
                    Header = $"{Properties.Langs.Lang.Step}1",
                    Content = Properties.Langs.Lang.Register
                },
                new StepBarDemoModel
                {
                    Header = $"{Properties.Langs.Lang.Step}2",
                    Content = Properties.Langs.Lang.BasicInfo
                },
                new StepBarDemoModel
                {
                    Header = $"{Properties.Langs.Lang.Step}3",
                    Content = Properties.Langs.Lang.UploadFile
                },
                new StepBarDemoModel
                {
                    Header = $"{Properties.Langs.Lang.Step}4",
                    Content = Properties.Langs.Lang.Complete
                }
            };
        }
    }
}
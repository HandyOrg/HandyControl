using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using HandyControl.Data;
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
            for (var i = 1; i <= 20; i++)
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
                    ImgPath = $"/HandyControlDemo;component/Resources/Img/Avatar/avatar{i % 7}.png",
                    Remark = new string(i.ToString()[0], 10)
                };
                list.Add(model);
            }

            return list;
        }

        public List<DemoDataModel> GetDemoDataList(int count)
        {
            var list = new List<DemoDataModel>();
            for (var i = 1; i <= count; i++)
            {
                var index = i % 6 + 1;
                var model = new DemoDataModel
                {
                    Index = i,
                    IsSelected = i % 2 == 0,
                    Name = $"Name{i}",
                    Type = (DemoType)index,
                    ImgPath = $"/HandyControlDemo;component/Resources/Img/Avatar/avatar{index}.png",
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

        internal ObservableCollection<CoverViewDemoModel> GetCoverViewDemoDataList()
        {
            return new ObservableCollection<CoverViewDemoModel>
            {
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/1.jpg",
                    BackgroundToken = ResourceToken.SuccessBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/2.jpg",
                    BackgroundToken = ResourceToken.PrimaryBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/3.jpg",
                    BackgroundToken = ResourceToken.WarningBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/4.jpg",
                    BackgroundToken = ResourceToken.DangerBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/5.jpg",
                    BackgroundToken = ResourceToken.SuccessBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/6.jpg",
                    BackgroundToken = ResourceToken.PrimaryBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/7.jpg",
                    BackgroundToken = ResourceToken.InfoBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/8.jpg",
                    BackgroundToken = ResourceToken.WarningBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/9.jpg",
                    BackgroundToken = ResourceToken.PrimaryBrush
                },
                new CoverViewDemoModel
                {
                    ImgPath = "/HandyControlDemo;component/Resources/Img/Album/10.jpg",
                    BackgroundToken = ResourceToken.DangerBrush
                }
            };
        }
    }
}
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
                for (var j = 0; j < 3; j++)
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
                    ImgPath = $"/HandyControlDemo;component/Resources/Img/Avatar/avatar{i % 6 + 1}.png",
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
                list.Add($"{converter.Convert(Properties.Langs.Lang.Text, null, i, CultureInfo.CurrentCulture)}{i}");
            }

            return list;
        }

        internal List<AvatarModel> GetContributorDataList()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "request");
            var list = new List<AvatarModel>();
            try
            {
                var json = client.DownloadString(new Uri("https://api.github.com/repos/nabian/handycontrol/contributors"));
                var objList = JsonConvert.DeserializeObject<List<dynamic>>(json);
                list.AddRange(objList.Select(item => new AvatarModel
                {
                    DisplayName = item.login,
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

        internal List<AvatarModel> GetBlogDataList()
        {
            return new List<AvatarModel>
            {
                new AvatarModel
                {
                    DisplayName = "林德熙",
                    AvatarUri = "https://avatars3.githubusercontent.com/u/16054566?s=400&v=4",
                    Link = "https://blog.lindexi.com/"
                },
                new AvatarModel
                {
                    DisplayName = "吕毅",
                    AvatarUri = "https://avatars2.githubusercontent.com/u/9959623?s=400&v=4",
                    Link = "https://blog.walterlv.com/"
                },
                new AvatarModel
                {
                    DisplayName = "DinoChan",
                    AvatarUri = "https://avatars1.githubusercontent.com/u/6076257?s=400&v=4",
                    Link = "https://www.cnblogs.com/dino623/"
                },
                new AvatarModel
                {
                    DisplayName = "noctwolf",
                    AvatarUri = "https://avatars3.githubusercontent.com/u/21022467?s=400&v=4",
                    Link = "https://www.cnblogs.com/noctwolf/"
                }
            };
        }

        internal List<AvatarModel> GetProjectDataList()
        {
            return new List<AvatarModel>
            {
                new AvatarModel
                {
                    DisplayName = "phpEnv",
                    AvatarUri = "https://cdn.phpenv.cn:444/logo.png",
                    Link = "https://www.phpenv.cn/"
                },
                new AvatarModel
                {
                    DisplayName = "AutumnBox",
                    AvatarUri = "https://www.atmb.top/images/leaves.png",
                    Link = "https://github.com/zsh2401/AutumnBox"
                },
                new AvatarModel
                {
                    DisplayName = "PandaX Studio",
                    AvatarUri = "https://raw.githubusercontent.com/aboutlong/pandaxstudio/master/pandax/pandax/source/image/logo.png",
                    Link = "https://github.com/aboutlong/pandaxstudio"
                }
            };
        }

        internal List<CardModel> GetCardDataList()
        {
            return new List<CardModel>
            {
                new CardModel
                {
                    Header = "Atomic",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/1.jpg",
                    Footer = "Stive Morgan"
                },
                new CardModel
                {
                    Header = "Zinderlong",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/2.jpg",
                    Footer = "Zonderling"
                },
                new CardModel
                {
                    Header = "Busy Doin' Nothin'",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/3.jpg",
                    Footer = "Ace Wilder"
                },
                new CardModel
                {
                    Header = "Wrong",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/4.jpg",
                    Footer = "Blaxy Girls"
                },
                new CardModel
                {
                    Header = "The Lights",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/5.jpg",
                    Footer = "Panda Eyes"
                },
                new CardModel
                {
                    Header = "EA7-50-Cent Disco",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/6.jpg",
                    Footer = "еяхат музыка"
                },
                new CardModel
                {
                    Header = "Monsters",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/7.jpg",
                    Footer = "Different Heaven"
                },
                new CardModel
                {
                    Header = "Gangsta Walk",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/8.jpg",
                    Footer = "Illusionize"
                },
                new CardModel
                {
                    Header = "Won't Back Down",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/9.jpg",
                    Footer = "Boehm / Benjamin Francis Leftwich"
                },
                new CardModel
                {
                    Header = "Katchi",
                    Content = "/HandyControlDemo;component/Resources/Img/Album/10.jpg",
                    Footer = "Ofenbach / Nick Waterhouse"
                }
            };
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HandyControl.Tools;
using HandyControlDemo.Data;
using Newtonsoft.Json;

namespace HandyControlDemo.Service;

public class DataService
{
    internal ObservableCollection<CardModel> GetCardDataList()
    {
        return
        [
            new CardModel { Header = "Atomic", Content = "avares://HandyControlDemo/Resources/Img/Album/1.jpg", Footer = "Stive Morgan" },
            new CardModel { Header = "Zinderlong", Content = "avares://HandyControlDemo/Resources/Img/Album/2.jpg", Footer = "Zonderling" },
            new CardModel { Header = "Busy Doin' Nothin'", Content = "avares://HandyControlDemo/Resources/Img/Album/3.jpg", Footer = "Ace Wilder" },
            new CardModel { Header = "Wrong", Content = "avares://HandyControlDemo/Resources/Img/Album/4.jpg", Footer = "Blaxy Girls" },
            new CardModel { Header = "The Lights", Content = "avares://HandyControlDemo/Resources/Img/Album/5.jpg", Footer = "Panda Eyes" },
            new CardModel { Header = "EA7-50-Cent Disco", Content = "avares://HandyControlDemo/Resources/Img/Album/6.jpg", Footer = "еяхат музыка" },
            new CardModel { Header = "Monsters", Content = "avares://HandyControlDemo/Resources/Img/Album/7.jpg", Footer = "Different Heaven" },
            new CardModel { Header = "Gangsta Walk", Content = "avares://HandyControlDemo/Resources/Img/Album/8.jpg", Footer = "Illusionize" },
            new CardModel { Header = "Won't Back Down", Content = "avares://HandyControlDemo/Resources/Img/Album/9.jpg", Footer = "Boehm / Benjamin Francis Leftwich" },
            new CardModel { Header = "Katchi", Content = "avares://HandyControlDemo/Resources/Img/Album/10.jpg", Footer = "Ofenbach / Nick Waterhouse" }
        ];
    }

    internal CardModel GetCardData()
    {
        return new CardModel
        {
            Content = $"avares://HandyControlDemo/Resources/Img/Album/{DateTime.Now.Second % 10 + 1}.jpg"
        };
    }

    internal List<DemoDataModel> GetDemoDataList()
    {
        var list = new List<DemoDataModel>();
        for (int i = 1; i <= 20; i++)
        {
            var dataList = new List<DemoDataModel>();
            for (int j = 0; j < 3; j++)
            {
                dataList.Add(new DemoDataModel
                {
                    Index = j, IsSelected = j % 2 == 0, Name = $"SubName{j}", Type = (DemoType)j
                });
            }

            var model = new DemoDataModel
            {
                Index = i,
                IsSelected = i % 2 == 0,
                Name = $"Name{i}",
                Type = (DemoType)(i % 6 + 1),
                DataList = dataList,
                ImgPath = $"/HandyControlDemo;component/Resources/Img/Avatar/avatar{i % 6 + 1}.png",
                Remark = new string(i.ToString()[0], 10)
            };
            list.Add(model);
        }

        return list;
    }

    internal List<DemoInfoModel> GetDemoInfo()
    {
        var infoList = new List<DemoInfoModel>();
        string jsonStr = ReadEmbeddedJsonFile("HandyControlDemo.Data.DemoInfo.json");
        dynamic? jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonStr);
        if (jsonObj is null)
        {
            return [];
        }

        foreach (dynamic? item in jsonObj)
        {
            string? titleKey = (string)item.title;
            List<DemoItemModel> list = Convert2DemoItemList(item.demoItemList);

            var demoInfoModel = new DemoInfoModel
            {
                Key = titleKey,
                Title = titleKey,
                DemoItemList = list,
                SelectedIndex = (int)item.selectedIndex,
                IsGroupEnabled = (bool)item.group
            };

            infoList.Add(demoInfoModel);
        }

        return infoList;
    }

    private static string ReadEmbeddedJsonFile(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string? resourceNameWithNamespace = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

        if (resourceNameWithNamespace is null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using var stream = assembly.GetManifestResourceStream(resourceNameWithNamespace);
        if (stream is null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private static List<DemoItemModel> Convert2DemoItemList(dynamic list)
    {
        var resultList = new List<DemoItemModel>();

        foreach (dynamic? item in list)
        {
            string? name = (string)item[0];
            string targetCtlName = item[1];
            string imageBrushName = item[2];
            bool isNew = !string.IsNullOrEmpty((string)item[3]);
            string? groupName = (string)item[4];
            if (string.IsNullOrEmpty(groupName))
            {
                groupName = "Misc";
            }

            resultList.Add(new DemoItemModel
            {
                Name = name,
                TargetCtlName = targetCtlName,
                ImageBrush = ResourceHelper.GetResource<object>(imageBrushName),
                IsNew = isNew,
                GroupName = groupName
            });
        }

        return resultList;
    }
}

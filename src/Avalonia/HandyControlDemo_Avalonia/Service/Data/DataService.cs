using System;
using System.Collections.Generic;
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
    internal List<DemoInfoModel> GetDemoInfo()
    {
        var infoList = new List<DemoInfoModel>();
        string jsonStr = ReadEmbeddedJsonFile("HandyControlDemo.Data.DemoInfo.json");
        var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonStr);
        if (jsonObj is null)
        {
            return new List<DemoInfoModel>();
        }

        foreach (var item in jsonObj)
        {
            var titleKey = (string) item.title;
            var title = titleKey;
            List<DemoItemModel> list = Convert2DemoItemList(item.demoItemList);

            var demoInfoModel = new DemoInfoModel
            {
                Key = titleKey,
                Title = title,
                DemoItemList = list,
                SelectedIndex = (int) item.selectedIndex,
                IsGroupEnabled = (bool) item.group
            };

            infoList.Add(demoInfoModel);
        }

        return infoList;
    }

    private string ReadEmbeddedJsonFile(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNameWithNamespace = assembly.GetManifestResourceNames()
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

    private List<DemoItemModel> Convert2DemoItemList(dynamic list)
    {
        var resultList = new List<DemoItemModel>();

        foreach (var item in list)
        {
            var name = (string) item[0];
            string targetCtlName = item[1];
            string imageBrushName = item[2];
            var isNew = !string.IsNullOrEmpty((string) item[3]);
            var groupName = (string) item[4];
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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HandyControl.Controls;

namespace HandyControl.Tools;

/// <summary>
///     该类可以为可视化元素提供单开的功能
/// </summary>
public class SingleOpenHelper
{
    private static readonly Dictionary<string, ISingleOpen> OpenDic = new();

    /// <summary>
    ///     根据指定的类型创建实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateControl<T>() where T : Visual, ISingleOpen, new()
    {
        var typeStr = typeof(T).FullName;

        if (string.IsNullOrEmpty(typeStr)) return default;

        var temp = new T();
        if (!OpenDic.Keys.Contains(typeStr))
        {
            OpenDic.Add(typeStr, temp);
            return temp;
        }
        var currentCtl = OpenDic[typeStr];
        if (currentCtl.CanDispose)
        {
            currentCtl.Dispose();
            OpenDic[typeStr] = temp;
            return temp;
        }
        return default;
    }
}

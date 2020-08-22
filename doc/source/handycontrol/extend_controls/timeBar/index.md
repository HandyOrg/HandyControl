---
title: TimeBar 时间条
---

一种包含热区和时间刻度的进度条.

```cs
[TemplatePart(Name = ElementBorderTop, Type = typeof(Border))]
[TemplatePart(Name = ElementTextBlockMove, Type = typeof(TextBlock))]
[TemplatePart(Name = ElementTextBlockSelected, Type = typeof(TextBlock))]
[TemplatePart(Name = ElementCanvasSpe, Type = typeof(Canvas))]
[TemplatePart(Name = ElementHotspots, Type = typeof(Panel))]
public class TimeBar : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Hotspots|热区集合|||
|HotspotsBrush|热区颜色|||
|ShowSpeStr|是否显示刻度|false||
|TimeFormat|时间格式|yyyy-MM-dd HH:mm:ss||
|SelectedTime|当前选中时间||||

# 事件

|名称|说明|
|-|-|
| TimeChanged | 选中时间改变时触发 |

# 案例

```xml
<hc:TimeBar BorderThickness="0" Width="600"/>
```

![TimeBar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/TimeBar.gif)

## 设置热区

```cs
for (int i = 0; i < 10; i++)
{
    var hour = 6 * i;
    TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(hour), DateTime.Today.AddHours(hour + 1)));
    TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(-hour), DateTime.Today.AddHours(-hour + 1)));
}
```
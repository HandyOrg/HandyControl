---
title: Watermark 水印
---

可使用指定的内容创建平铺背景.

```cs
[TemplatePart(Name = ElementRoot, Type = typeof(Border))]
[ContentProperty(nameof(Content))]
public class Watermark : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Angle|水印旋转角度|20|由默认样式提供|
|Content|需要使用水印的内容|||
|Mark|水印|||
|MarkWidth|水印宽度|0||
|MarkHeight|水印高度|0||
|MarkBrush|水印颜色|||
|AutoSizeEnabled|水印是否自动适配大小|true||
|MarkMargin|水印边距||||

# 案例

```xml
<hc:Watermark Mark="Project" FontSize="80" FontWeight="Bold" MarkMargin="30,0"/>
```

![Watermark](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Watermark_1.png)

```xml
<hc:Watermark Mark="{StaticResource GitHubStrGeometry}" MarkMargin="30,0" MarkWidth="200" MarkHeight="100"/>
```

![Watermark](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Watermark_2.png)
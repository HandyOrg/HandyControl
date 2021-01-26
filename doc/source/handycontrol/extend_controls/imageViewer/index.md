---
title: ImageViewer 图片视图
---

`ImageViewer` 是 `ImageBrowser` 的核心构件，它可以作为独立的控件使用，无需弹窗。

```cs
[TemplatePart(Name = ElementPanelMain, Type = typeof(Panel))]
[TemplatePart(Name = ElementCanvasSmallImg, Type = typeof(Canvas))]
[TemplatePart(Name = ElementBorderMove, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderBottom, Type = typeof(Border))]
[TemplatePart(Name = ElementImageMain, Type = typeof(Image))]
public class ImageViewer : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ShowImgMap|是否显示小地图|false||
|ImageSource|图片资源|||
|IsFullScreen|是否处于全屏显示中|false|||

# 案例

```xml
<hc:ImageViewer Background="{DynamicResource SecondaryRegionBrush}" Width="600" Height="330" ImageSource="/HandyControlDemo;component/Resources/Img/1.jpg"/>
```

![ImageViewer](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/ImageViewer_1.png)
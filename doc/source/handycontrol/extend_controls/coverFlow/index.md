---
title: CoverFlow 封面流
---

该控件将封面以3D的形式展示出来，可用于增强界面效果。

```cs
[TemplatePart(Name = ElementViewport3D, Type = typeof(Viewport3D))]
[TemplatePart(Name = ElementCamera, Type = typeof(ProjectionCamera))]
[TemplatePart(Name = ElementVisualParent, Type = typeof(ModelVisual3D))]
public class CoverFlow : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|PageIndex|页码|||
|Loop|是否循环展示||||

# 方法

|方法|描述|
|-|-|
|Add(string)|添加一项资源|
|Add(Uri)|添加一项资源|
|AddRange(IEnumerable<object>)|批量添加资源|
|JumpTo(int)|跳转|

# 案例

```cs
CoverFlowMain.AddRange(new []
{
    new Uri(@"pack://application:,,,/Resources/Img/Album/1.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/2.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/3.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/4.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/5.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/6.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/7.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/8.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/9.jpg"),
    new Uri(@"pack://application:,,,/Resources/Img/Album/10.jpg")
});
CoverFlowMain.JumpTo(2);
```

```xml
<hc:CoverFlow x:Name="CoverFlowMain" Margin="32" Width="500" Height="300"/>
```

![CoverFlow](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CoverFlow.gif)
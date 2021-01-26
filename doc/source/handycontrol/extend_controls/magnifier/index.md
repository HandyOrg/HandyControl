---
title: Magnifier 放大镜
---

借助 `Magnifier` 可对任意控件局部放大.

```cs
[TemplatePart(Name = ElementVisualBrush, Type = typeof(VisualBrush))]
public class Magnifier : AdornerElement
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|HorizontalOffset|相对鼠标的水平偏移|0||
|VerticalOffset|相对鼠标的垂直偏移|0||
|Scale|缩放因子|5.0|||

# 案例

```xml
<Image Margin="16" hc:Magnifier.Instance="{x:Static hc:Magnifier.Default}" Source="/HandyControlDemo;component/Resources/Img/b1.jpg" Stretch="None"/>
```

```xml
<Image Margin="16" Source="/HandyControlDemo;component/Resources/Img/b1.jpg" Stretch="None">
    <hc:Magnifier.Instance>
        <hc:Magnifier Scale="6" HorizontalOffset="-16" VerticalOffset="-16"/>
    </hc:Magnifier.Instance>
</Image>
```

![Magnifier](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Magnifier.png)
---
title: ColorPicker 颜色拾取器
---

仿制Chrome的颜色拾取器，相关博文：[《WPF 控件库——仿制Chrome的ColorPicker》](https://www.cnblogs.com/nabian/p/9267646.html)

```cs
[TemplatePart(Name = ElementBorderColor, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderPicker, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderDrag, Type = typeof(Border))]
[TemplatePart(Name = ElementPanelColor, Type = typeof(Panel))]
[TemplatePart(Name = ElementSliderColor, Type = typeof(Panel))]
[TemplatePart(Name = ElementSliderOpacity, Type = typeof(Panel))]
[TemplatePart(Name = ElementPanelRgb, Type = typeof(Panel))]
[TemplatePart(Name = ElementButtonDropper, Type = typeof(ToggleButton))]
public class ColorPicker : Control, ISingleOpen
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|SelectedBrush|选中色|Brushes.White|||

# 事件

|事件|描述|备注|
|-|-|-|
|SelectedColorChanged|颜色改变事件||
|Canceled|取消事件|||

# 案例

```xml
<hc:ColorPicker Name="ColorPicker" Margin="32"/>
```

![ColorPicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ColorPicker.gif)
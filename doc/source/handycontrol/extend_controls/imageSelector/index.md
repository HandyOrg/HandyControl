---
title: ImageSelector 图片选择器
---

该控件封装了图片选择、展示、删除的一整套逻辑.

```cs
public class ImageSelector : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Stretch|图片拉伸模式|Stretch.None||
|Uri|图片资源|||
|PreviewBrush|预览图画刷|||
|StrokeThickness|边框粗细|||
|StrokeDashArray|边框虚线数组|||
|DefaultExt|默认图片后缀名|.jpg||
|Filter|图片过滤字符串|(.jpg)&#124;*.jpg&#124;(.png)&#124;*.png||
|HasValue|是否有值|false|||

# 样式

|样式|描述|
|-|-|
|ImageSelectorBaseStyle|默认样式|

# 案例

```xml
<UniformGrid Columns="3" Margin="16">
    <hc:ImageSelector Width="100" Height="100" Margin="16"/>
    <hc:ImageSelector Width="100" Height="100" Margin="16" hc:BorderElement.CornerRadius="50"/>
    <hc:ImageSelector Width="100" Height="100" Margin="16" hc:BorderElement.CornerRadius="50" StrokeDashArray="10,5"/>
    <hc:ImageSelector Width="100" Height="100" Margin="16" hc:BorderElement.CornerRadius="50" BorderBrush="{DynamicResource SuccessBrush}"/>
    <hc:ImageSelector Width="100" Height="100" Margin="16" hc:BorderElement.CornerRadius="50" StrokeDashArray="10,5,10" BorderBrush="{DynamicResource DangerBrush}"/>
    <hc:ImageSelector Width="100" Height="100" Margin="16" hc:BorderElement.CornerRadius="10" StrokeThickness="2" BorderThickness="2" BorderBrush="{DynamicResource PrimaryBrush}"/>
</UniformGrid>
```

![ImageSelector](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ImageSelector.png)
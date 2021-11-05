---
title: CircleProgressBar 圆形进度条
---

在宽高存在一定限制的区域可以使用圆形进度条节省空间。

```cs
[TemplatePart(Name = IndicatorTemplateName, Type = typeof(Arc))]
public class CircleProgressBar : RangeBase
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ArcThickness|圆形粗细|4|该默认值由主题提供|
|ShowText|是否显示文本|true||
|Text|文本内容|<空>|进度文本|

# 样式

|样式|描述|
|-|-|
|ProgressBarCircleBaseStyle|默认样式|
|ProgressBarSuccessCircle|成功|
|ProgressBarInfoCircle|信息|
|ProgressBarWarningCircle|警告|
|ProgressBarDangerCircle|危险|

# 案例

```xml
<StackPanel Orientation="Horizontal" Margin="0,32,0,0">
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}"/>
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}" FontSize="30" Margin="16,0,0,0"/>
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}" Margin="16,0,0,0" ShowText="False" Width="20" Height="20" ArcThickness="2" Style="{StaticResource ProgressBarSuccessCircle}"/>
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}" Margin="16,0,0,0" ShowText="False" Width="30" Height="30" ArcThickness="6" Style="{StaticResource ProgressBarInfoCircle}"/>
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}" Margin="16,0,0,0" ShowText="False" Width="40" Height="40" ArcThickness="10" Style="{StaticResource ProgressBarWarningCircle}"/>
    <hc:CircleProgressBar Value="{Binding Value,ElementName=SliderDemo}" Margin="16,0,0,0" Width="50" Height="50" Style="{StaticResource ProgressBarDangerCircle}"/>
</StackPanel>
```

![CircleProgressBar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CircleProgressBar.gif)
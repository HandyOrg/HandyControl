---
title: PreviewSlider 预览滑块
---

可借助预览滑块向用户反馈在不同位置的状态信息.

```cs
[TemplatePart(Name = TrackKey, Type = typeof(Track))]
[TemplatePart(Name = ThumbKey, Type = typeof(FrameworkElement))]
public class PreviewSlider : Slider
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|PreviewContent|预览内容|||
|PreviewContentOffset|预览内容偏移|9|||

# 附加属性

|属性|描述|默认值|备注|
|-|-|-|-|
|PreviewPosition|预览位置|0|||

# 事件

|名称|说明|
|-|-|
| PreviewPositionChanged | 预览位置改变时触发 |

# 案例

```xml
<hc:PreviewSlider Name="PreviewSliderHorizontal" Width="300" Value="500" Maximum="1000">
    <hc:PreviewSlider.PreviewContent>
        <Label Style="{StaticResource LabelPrimary}" Content="{Binding Path=(hc:PreviewSlider.PreviewPosition),RelativeSource={RelativeSource Self}}" ContentStringFormat="#0.00"/>
    </hc:PreviewSlider.PreviewContent>
</hc:PreviewSlider>
```

![PreviewSlider](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/PreviewSlider.gif)
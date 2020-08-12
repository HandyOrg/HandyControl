---
title: OutlineText 轮廓文本
---

`OutlineText` 可以使文本突出显示.

```cs
public class OutlineText : FrameworkElement
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|StrokePosition|轮廓位置|StrokePosition.Center||
|Text|文本|||
|TextAlignment|文本对齐方式|TextAlignment.Left||
|TextTrimming|文本截断方式|TextTrimming.None||
|TextWrapping|文本换行方式|TextWrapping.NoWrap||
|Fill|文本颜色|||
|Stroke|轮廓颜色|||
|StrokeThickness|轮廓粗细|0||
|FontFamily|字体|||
|FontSize|字体大小|||
|FontStretch|字体变形方式|||
|FontStyle|字体风格|||
|FontWeight|字体粗细||||

# 案例

```xml
<StackPanel Margin="32" Width="400">
    <hc:OutlineText TextWrapping="Wrap" Fill="{DynamicResource PrimaryTextBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=20}"/>
    <hc:OutlineText Margin="0,10,0,0" FontSize="30" Fill="{DynamicResource SuccessBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=8}"/>
    <hc:OutlineText Margin="0,10,0,0" Fill="{DynamicResource PrimaryTextBrush}" FontSize="30" FontStyle="Italic" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=8}"/>
    <hc:OutlineText Margin="0,10,0,0" TextWrapping="Wrap" TextTrimming="CharacterEllipsis" FontSize="50" StrokeThickness="2" Fill="{DynamicResource DangerBrush}" Stroke="{DynamicResource PrimaryBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=5}"/>
    <hc:OutlineText Margin="0,10,0,0" FontSize="80" FontWeight="Bold" StrokeThickness="4" Fill="{DynamicResource PrimaryBrush}" Stroke="{DynamicResource DangerBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=2}"/>
    <hc:OutlineText Margin="0,10,0,0" StrokePosition="Outside" FontSize="80" FontWeight="Bold" StrokeThickness="4" Fill="{DynamicResource PrimaryBrush}" Stroke="{DynamicResource DangerBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=2}"/>
    <hc:OutlineText Margin="0,10,0,0" StrokePosition="InSide" FontSize="80" FontWeight="Bold" StrokeThickness="4" Fill="{DynamicResource PrimaryBrush}" Stroke="{DynamicResource DangerBrush}" Text="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=2}"/>
</StackPanel>
```

![OutlineText](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/OutlineText.png)
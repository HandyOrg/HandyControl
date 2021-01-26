---
title: ImageBlock 图片块
---

图片块可用于将一张大图片分割成宽高一致的小图片，并按序播放形成类似于gif图片的效果.

```cs
public class ImageBlock : FrameworkElement
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|StartColumn|播放开始列|0||
|StartRow|播放开始行|0||
|EndColumn|播放结束列|0||
|EndRow|播放结束行|0||
|IsPlaying|是否在播放|false||
|Columns|需要分割后的列数|1||
|Rows|需要分割后的行数|1||
|Interval|小图片的播放间隔|1s||
|Source|大图资源||||

# 案例

```xml
<UniformGrid Rows="3" Columns="4" Margin="32">
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="0" EndColumn="7" EndRow="0" Width="110" Height="128" IsPlaying="True"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="1" EndColumn="7" EndRow="1" Width="110" Height="128" IsPlaying="True"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="2" EndColumn="7" EndRow="2" Width="110" Height="128" IsPlaying="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="3" EndColumn="7" EndRow="3" Width="110" Height="128" IsPlaying="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.1" Columns="8" Rows="10" StartColumn="0" StartRow="4" EndColumn="7" EndRow="4" Width="110" Height="128" IsPlaying="True"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="5" EndColumn="7" EndRow="5" Width="110" Height="128" IsPlaying="True"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="6" EndColumn="7" EndRow="6" Width="110" Height="128" IsPlaying="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.1" Columns="8" Rows="10" StartColumn="0" StartRow="7" EndColumn="7" EndRow="7" Width="110" Height="128" IsPlaying="True"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="8" EndColumn="7" EndRow="8" Width="110" Height="128" IsPlaying="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"/>
    <hc:ImageBlock Source="/HandyControlDemo;component/Resources/Img/Dance.png" Interval="0:0:0.125" Columns="8" Rows="10" StartColumn="0" StartRow="9" EndColumn="7" EndRow="9" Width="110" Height="128" IsPlaying="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"/>
</UniformGrid>
```

![ImageBlock](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ImageBlock.gif)

{% note warning %}
不再使用 `ImageBlock` 时记得调用 `Dispose` 方法清理资源。
{% endnote %}
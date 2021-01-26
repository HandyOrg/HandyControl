---
title: GifImage Gif图片
---

Gif的wpf实现。

```cs
public class GifImage : Image, IDisposable
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Uri|图片Uri||||

# 案例

```xml
<hc:GifImage x:Name="GifImageMain" Stretch="None" Margin="32" Uri="/HandyControlDemo;component/Resources/Img/car_chase.gif"/>
```

![GifImage](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/GifImage.gif)

{% note warning %}
不再使用 `GifImage` 时记得调用 `Dispose` 方法清理资源。
{% endnote %}
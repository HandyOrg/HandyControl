---
title: FlipClock 翻页时钟
---

一种3D形式的时钟方案。

```cs
public class FlipClock : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|NumberList|数字集合|||
|DisplayTime|显示时间||||

# 案例

```xml
<hc:FlipClock Margin="32"/>
```

![FlipClock](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/FlipClock.gif)

{% note warning %}
不再使用 `FlipClock` 时记得调用 `Dispose` 方法清理资源。
{% endnote %}

{% note warning %}
可借助 `ViewBox` 调节 `FlipClock` 大小。
{% endnote %}
---
title: DashedBorder 虚线边框
---

一种装饰元素，可提供虚线边框，以实现某些特殊效果.

```cs
public class DashedBorder : Decorator
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|BorderThickness|边框粗细|||
|BorderDashThickness|边框虚线粗细|0||
|Padding|内边距|||
|CornerRadius|圆角|||
|BorderBrush|边框颜色|||
|Background|背景色|||
|BorderDashArray|边框虚线数组|||
|BorderDashCap|边框虚线线帽样式|PenLineCap.Flat||
|BorderDashOffset|边框虚线偏移|0|||

# 案例

```xml
<hc:DashedBorder Width="100" Height="100" BorderDashThickness="2" BorderBrush="Black" BorderDashArray="3, 1" CornerRadius="0,50,0,0"/>
```

![DashedBorder](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/DashedBorder.png)

{% note warning %}
当需要设置边框粗细时要注意，如果4个圆角数值一致，请使用 `BorderThickness`，否则请使用 `BorderDashThickness`
{% endnote %}
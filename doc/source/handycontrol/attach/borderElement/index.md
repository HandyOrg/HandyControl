---
title: BorderElement 具有边框的元素
---

# 属性

| 名称 | 用途 |
|-|-|
| CornerRadius | 设置边框圆角值 |
| Circular | 是否呈现为圆形 True为是、False为否 |

# 使用案例

## CornerRadius 设置边框圆角值

```xml
<StackPanel Width="200" VerticalAlignment="Center">
    <Button Content="Button" hc:BorderElement.CornerRadius="15" HorizontalAlignment="Stretch"/>
    <TextBox Text="TextBox" hc:BorderElement.CornerRadius="15" Margin="0,10,0,0"/>
</StackPanel>
```

![BorderElement.CornerRadius](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/BorderElement.CornerRadius.png)

## Circular 是否呈现为圆形

借助`BorderElement.Circular`附加属性实现圆形Border
```xml
<Border Style="{StaticResource BorderCircular}" Background="OrangeRed" Width="100" Height="100"/>
```
![BorderElement.Circular](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/BorderElement.Circular.png)
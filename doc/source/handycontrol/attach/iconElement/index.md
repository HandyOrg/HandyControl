---
title: IconElement 具有图标的元素
---

# 属性

| 名称 | 用途 |
|-|-|
| Geometry | 几何形状 |
| Width | 图标宽度 |
| Height | 图标高度 |

# 使用案例

## Geometry 几何形状

```xml
<StackPanel Width="200" VerticalAlignment="Center">
    <Button Content="Button" HorizontalAlignment="Stretch"/>
    <Button Content="Button" hc:IconElement.Geometry="{StaticResource CalendarGeometry}" HorizontalAlignment="Stretch" Margin="0,10,0,0"/>
</StackPanel>
```

![IconElement.Geometry](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/IconElement.Geometry.png)
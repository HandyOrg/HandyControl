---
title: BackgroundSwitchElement 可切换背景的元素
---

# 属性

| 名称 | 用途 |
|-|-|
| MouseHoverBackground | 设置鼠标悬浮背景色 |
| MouseDownBackground | 设置鼠标按下背景色 |

# 使用案例

## MouseHoverBackground 设置鼠标悬浮背景色

在样式或模板中我们添加如下的触发器代码：

```xml
<Trigger Property="IsMouseOver" Value="True">
    <Setter Property="Background" TargetName="Chrome" Value="{Binding Path=(hc:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
</Trigger>
```

前台使用方式：

```xml
<目标控件  hc:BackgroundSwitchElement.MouseHoverBackground ="Blue"/>
```

## MouseDownBackground  设置鼠标按下背景色

在样式或模板中我们添加如下的触发器代码：

```xml
<Trigger Property="IsPressed" Value="True">
    <Setter Property="Background" TargetName="Chrome" Value="{Binding Path=(hc:BackgroundSwitchElement.MouseDownBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
</Trigger>
```

前台使用方式：

```xml
<目标控件 hc:BackgroundSwitchElement.MouseDownBackground ="Yellow"/>
```
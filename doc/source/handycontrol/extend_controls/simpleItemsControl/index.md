---
title: SimpleItemsControl 简单项目控件
---

一种轻量级ItemsControl控件.

```cs
[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class SimpleItemsControl : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ItemTemplate|项目模板|||
|ItemContainerStyle|项目容器样式|||
|ItemsSource|项目资源|||
|Items|项目|||
|HasItems|是否有项目|false||
---
title: SimpleText 简单文本
---

一种轻量级文本显示控件.

```cs
public class SimpleText : FrameworkElement
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Text|文本|||
|TextAlignment|文本对齐方式|TextAlignment.Left||
|TextTrimming|文本截断方式|TextTrimming.None||
|TextWrapping|文本换行方式|TextWrapping.NoWrap||
|Foreground|字体颜色|||
|FontFamily|字体|||
|FontSize|字体大小|||
|FontStretch|字体变形方式|||
|FontStyle|字体风格|||
|FontWeight|字体粗细||||

# 案例

```xml
<hc:SimpleText Text="Content"/>
```
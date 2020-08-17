---
title: PopupWindow 弹出窗口
---

`PopupWindow` 相对于 `PopTip` 能更灵活地控制弹出元素的位置.

{% note warning %}
PopupWindow 在未来版本中可能会作废.
{% endnote %}

```cs
[TemplatePart(Name = ElementMainBorder, Type = typeof(Border))]
[TemplatePart(Name = ElementTitleBlock, Type = typeof(TextBlock))]
public class PopupWindow : System.Windows.Window
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|PopupElement|弹出元素|||
|ShowTitle|是否显示标题|true||
|ShowCancel|是否显示取消按钮|false||
|ShowBorder|是否显示边框|false|||

# 方法

|名称|说明|
|-|-|
| Show(FrameworkElement, bool) | 显示窗口(弹出元素, 是否带有背景) |
| Show(System.Windows.Window, Point) | 显示窗口(相对窗口, 偏移坐标) |
| Show(string) | 显示窗口(弹出信息) |
| ShowDialog(FrameworkElement, bool) | 模态化显示窗口(弹出元素, 是否带有背景) |
| ShowDialog(string, string title, bool) | 模态化显示窗口(弹出信息, 标题, 是否显示取消按钮) |
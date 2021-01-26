---
title: Pagination 页码条
---

当数据量过多时，使用分页分解数据.

```cs
[TemplatePart(Name = ElementButtonLeft, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonRight, Type = typeof(Button))]
[TemplatePart(Name = ElementButtonFirst, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementMoreLeft, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementPanelMain, Type = typeof(Panel))]
[TemplatePart(Name = ElementMoreRight, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementButtonLast, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementButtonLast, Type = typeof(NumericUpDown))]
public class Pagination : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|MaxPageCount|最大页数|1||
|DataCountPerPage|每页的数据量|20||
|PageIndex|页码|1||
|MaxPageInterval|当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）|3||
|IsJumpEnabled|是否显示跳转框|false|||

# 事件

|名称|说明|
|-|-|
| PageUpdated | 页码改变时触发 |

# 案例

```xml
<hc:Pagination MaxPageCount="10" PageIndex="5" IsJumpEnabled="True"/>
```

![Pagination](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Pagination.png)

{% note warning %}
当页码为1时，页码条整体不会显示。
{% endnote %}
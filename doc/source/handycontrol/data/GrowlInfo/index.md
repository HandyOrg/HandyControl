---
title: GrowlInfo
---

{% note info no-icon %}
class
{% endnote %}

{% note info no-icon %}
Growl消息初始化模型
{% endnote %}

|成员名|类型|说明|默认值|
|-|-|-|-|
| Message | string | 通知内容 | - |
| ShowDateTime | bool | 是否显示通知时间 | true |
| WaitTime | int | 等待自动关闭时间 | 6 |
| CancelStr | string | 取消字符串 | Lang.Cancel |
| ConfirmStr | string | 确认字符串 | Lang.Confirm |
| ActionBeforeClose | Func<bool, bool> | 关闭前的委托 | - |
| StaysOpen | bool | 保持打开 | false |
| IsCustom | bool | 是否自定义行为 | false |
| Type | InfoType | 消息类型 | InfoType.Success |
| IconKey | string | 图标键名 | - |
| IconBrushKey | string | 图标画刷键名 | - |
| ShowCloseButton | bool | 是否显示关闭按钮 | true |
| Token | string | 消息标记 | - |

## FAQ

{% note info no-icon %}
这里的IconKey和IconBrushKey我该填什么啊？
{% endnote %}

IconKey必须填资源类型为`Geometry`的Key名，例如以下的资源，它的Key名`GitterGeometry`就可以作为IconKey：
``` xml
<Geometry x:Key="GitterGeometry">M260.8 645H160V0h100.8v645zM461.8 152.2h-100.8V1024h100.8V152.2z m201.2 0h-100.8V1024h100.8V152.2zM864 152h-100.8v494H864V152z</Geometry>
```

IconBrushKey必须填资源类型为`Brush`的Key名，例如以下的资源，它的Key名`ToolBarBackground`就可以作为IconBrushKey：
``` xml
<LinearGradientBrush x:Key="ToolBarBackground" EndPoint="0.5,1" StartPoint="0.5,0">
    <GradientStop Color="#F5F4F5" Offset="0"/>
    <GradientStop Color="#D1CFD1" Offset="1"/>
</LinearGradientBrush>
```
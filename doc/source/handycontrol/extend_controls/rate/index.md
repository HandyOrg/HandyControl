---
title: Rate 评分
---

当对评价进行展示或对事物进行快速的评级操作时，可使用评分控件.

```cs
[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class SimpleItemsControl : Control
```

```cs
public class RegularItemsControl : SimpleItemsControl
```

```cs
public class Rate : RegularItemsControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|AllowHalf|是否允许半选|false||
|AllowClear|是否允许再次点击后清除|true||
|Icon|图标|||
|Count|star 总数|5||
|DefaultValue|默认值|0||
|Value|当前评分|0||
|Text|评分文字|||
|ShowText|是否显示评分文字|false||
|IsReadOnly|是否只读|false||
|Value|当前评分|0|||

# 事件

|名称|说明|
|-|-|
| ValueChanged | 评分改变时触发 |

# 案例

```xml
<StackPanel Width="170">
    <hc:Rate Value="2"/>
    <hc:Rate Value="5" Count="6" Margin="0,16,0,0" Foreground="{DynamicResource SuccessBrush}"/>
    <hc:Rate DefaultValue="3" IsReadOnly="True" AllowHalf="True" Margin="0,16,0,0" Foreground="{DynamicResource WarningBrush}"/>
    <hc:Rate DefaultValue="1" AllowClear="False" AllowHalf="True" Margin="0,16,0,0" Foreground="{DynamicResource DangerBrush}" Icon="{StaticResource LoveGeometry}"/>
    <hc:Rate Value="4.5" AllowHalf="True" ShowText="True" Margin="0,16,0,0" Foreground="{DynamicResource DangerBrush}" Icon="{StaticResource LoveGeometry}"/>
</StackPanel>
```

![Rate](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Rate.gif)
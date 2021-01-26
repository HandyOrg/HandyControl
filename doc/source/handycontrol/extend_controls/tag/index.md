---
title: Tag 标签
---

用以指示用户选中的项，相对于文字，`Tag` 有额外的交互.

```cs
public class Tag : ContentControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ShowCloseButton|是否显示删除(关闭)按钮|true||
|Selectable|是否支持选中|false||
|IsSelected|是否选中|false|||

# 事件

|名称|说明|
|-|-|
| Selected | 标签选中时触发 |
| Closing | 标签关闭时触发 |
| Closed | 标签关闭后触发 |

# TagPanel

标签专用容器.

{% note warning %}
即将在未来版本中移除，我们会提供功能更丰富的 `TagContainer` 来代替它.
{% endnote %}

## 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ShowAddButton|是否显示新增标签按钮|false||
|TagMargin|标签边距|Thickness(5)|||

## 事件

|名称|说明|
|-|-|
| AddTagButtonClick | 新增标签时触发 |

# 案例

```xml
<hc:TagPanel AddTagButtonClick="TagPanel_OnAddTagButtonClick" Margin="32" Orientation="Horizontal" MaxWidth="400" VerticalAlignment="Center" ShowAddButton="True">
    <hc:Tag ShowCloseButton="False" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=2}"/>
    <hc:Tag Selectable="True" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=3}"/>
    <hc:Tag ShowCloseButton="False" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=4}"/>
    <hc:Tag Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=5}"/>
    <hc:Tag IsSelected="True" Selectable="True" ShowCloseButton="False" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=4}"/>
    <hc:Tag Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=3}"/>
    <hc:Tag ShowCloseButton="False" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=2}"/>
    <hc:Tag IsSelected="True" Selectable="True" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=3}"/>
    <hc:Tag ShowCloseButton="False" Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=4}"/>
    <hc:Tag Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=5}"/>
</hc:TagPanel>  
```

![Tag](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Tag.png)
---
title: Transfer 穿梭框
---

穿梭选择框用直观的方式在两栏中移动元素，完成选择行为。

```cs
[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class SimpleItemsControl : Control
```

```cs
[TemplatePart(Name = ElementItemsOrigin, Type = typeof(SimpleItemsControl))]
[TemplatePart(Name = ElementItemsSelected, Type = typeof(SimpleItemsControl))]
public class Transfer : SimpleItemsControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|SelectedItems|选中项||||

# 事件

|名称|说明|
|-|-|
| SelectionChanged | 选中项改变时触发 |

# 案例

```xml
<hc:Transfer ItemsSource="{Binding DataList}" Margin="32" Height="300">
    <hc:Transfer.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}"/>
        </DataTemplate>
    </hc:Transfer.ItemTemplate>
</hc:Transfer>
```

![Transfer](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Transfer.gif)
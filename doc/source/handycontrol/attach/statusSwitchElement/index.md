---
title: StatusSwitchElement 可切换状态的元素
---

# 属性

| 名称                 | 用途             |
| -------------------- | ---------------- |
| CheckedElement       | 选中时展示的元素 |
| HideUncheckedElement | 是否隐藏元素     |

# 使用案例

##  CheckedElement 选中时展示的元素

此附加属性适用于`ToggleButton`以及子类控件，用于控制选择类控件选中时需要显示的内容，默认值为`False`，不显示。

```xml
    <ToggleButton Style="{DynamicResource ToggleButtonCustom}" Margin="5">
        <!--Checked-->
        <hc:StatusSwitchElement.CheckedElement>
            <Border Width="60" Height="20" CornerRadius="4" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
                <Ellipse Width="20" Height="20" Fill="{DynamicResource PrimaryBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <!--Default-->
        <Border Width="80" Height="30" CornerRadius="0" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
            <Ellipse Width="20" Height="20" Fill="{DynamicResource BorderBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
        </Border>
    </ToggleButton>
```

默认状态（非选中时）和选中状态

![StatusSwitchElement.CheckedElement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/StatusSwitchElement.CheckedElement.png)

##  HideUncheckedElement 是否隐藏元素

当控制在选中状态时，是否隐藏默认内容，默认值为`False`，不隐藏

```xml
<StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
    <ToggleButton IsChecked="True" Style="{DynamicResource ToggleButtonCustom}" Margin="5">
        <!--Checked-->
        <hc:StatusSwitchElement.CheckedElement>
            <Border Width="60" Height="20" CornerRadius="0" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
                <Ellipse Width="18" Height="18" Fill="{DynamicResource PrimaryBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <!--Default-->
        <Border Width="80" Height="30" CornerRadius="0" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
            <Ellipse Width="18" Height="18" Fill="{DynamicResource BorderBrush}"/>
        </Border>
    </ToggleButton>

    <!--显式设定选中状态下隐藏默认内容元素-->
    <ToggleButton IsChecked="True" Style="{DynamicResource ToggleButtonCustom}" hc:StatusSwitchElement.HideUncheckedElement="True" Margin="5">
        <!--Checked-->
        <hc:StatusSwitchElement.CheckedElement>
            <Border Width="60" Height="20" CornerRadius="0" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
                <Ellipse Width="18" Height="18" Fill="{DynamicResource PrimaryBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <!--Default-->
        <Border Width="80" Height="30" CornerRadius="0" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
            <Ellipse Width="18" Height="18" Fill="{DynamicResource BorderBrush}"/>
        </Border>
    </ToggleButton>
</StackPanel>
```

效果：

![StatusSwitchElement.HideUncheckedElement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/StatusSwitchElement.HideUncheckedElement.png)

{%  note info no-icon %}

以上两个属性，组合使用，能够简单的控制选择类控件状态的展示功能，可根据以上案例进行合理调整

{% endnote %}


---
title: RadioButton 单选按钮
---

# RadioButtonBaseStyle

单选按钮默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

```xml
<StackPanel>
    <RadioButton Content="默认样式"/>
    <RadioButton Margin="0,16,0,0" Content="不可编辑" IsChecked="True" IsEnabled="False"/>
    <RadioButton Margin="0,16,0,0" Content="默认样式"/>
    <RadioButton Margin="0,16,0,0" Content="不可编辑" IsEnabled="False"/>
</StackPanel>
```

效果：

![RadioButton.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RadioButton.DefaultStyle.png)

# RadioButtonIcon : RadioButtonBaseStyle

带图标样式，可只显示图标也可图文显示

案例：

```xml
<UniformGrid Margin="22,0,0,0" Rows="2" Columns="2">
    <RadioButton Margin="10,0,0,0" Background="{DynamicResource SecondaryRegionBrush}" hc:IconElement.Geometry="{StaticResource CalendarGeometry}" Style="{StaticResource RadioButtonIcon}" Content="RadioButtonIcon"/>
    <RadioButton Margin="10,0,0,0" Background="{DynamicResource SecondaryRegionBrush}" Style="{StaticResource RadioButtonIcon}" Content="RadioButtonIcon" IsChecked="True"/>
    <RadioButton Margin="10,0,0,0" BorderThickness="1" hc:IconElement.Geometry="{StaticResource CalendarGeometry}" Style="{StaticResource RadioButtonIcon}" Content="RadioButtonIcon"/>
    <RadioButton Margin="10,0,0,0" BorderThickness="1" Style="{StaticResource RadioButtonIcon}" Content="RadioButtonIcon"/>
</UniformGrid>
```

效果：

![RadioButton.IconStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RadioButton.IconStyle.png)
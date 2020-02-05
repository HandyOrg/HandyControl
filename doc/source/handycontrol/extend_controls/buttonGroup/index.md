---
title: ButtonGroup 按钮组
---

# RadioGroupItemBaseStyle

`ButtonGroup`控件中的子集`RadioButton`样式基类，不建议直接使用，常作为`ButtonGroup`中`Item`样式基类,同时与`ButtonGroup`控件配套使用

案例：

```xml
<!--内部Item样式为ButtonGroup控件自动附加-默认为水平方向item样式-->
<hc:ButtonGroup Margin="0,32,0,0">
    <RadioButton Content="RadioButton" IsChecked="True"/>
    <RadioButton Content="RadioButton"/>
    <RadioButton IsEnabled="False" Content="RadioButton"/>
    <RadioButton Content="RadioButton"/>
</hc:ButtonGroup>
<hc:ButtonGroup Margin="0,16,0,0" Style="{StaticResource ButtonGroupSolid}">
    <RadioButton Content="RadioButton"/>
    <RadioButton Content="RadioButton"/>
    <RadioButton IsEnabled="False" Content="RadioButton"/>
    <RadioButton Content="RadioButton" IsChecked="True"/>
</hc:ButtonGroup>
```

效果：

![RadioButton.RadioButtonItemDefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RadioButton.RadioButtonItemDefaultStyle.png)

# RadioGroup关联样式

| 样式Key                       | 用途                                 | 基类样式                |
| ----------------------------- | ------------------------------------ | ----------------------- |
| RadioGroupItemDefault         | RadioGroupItem默认样式               | RadioGroupItemBaseStyle |
| RadioGroupItemHorizontalFirst | RadioGroupItem水平头样式（从左至右） | RadioGroupItemBaseStyle |
| RadioGroupItemHorizontalLast  | RadioGroupItem水平尾样式（从左至右） | RadioGroupItemBaseStyle |
| RadioGroupItemSingle          | RadioGroupItem单项样式               | RadioGroupItemBaseStyle |
| RadioGroupItemVerticalFirst   | RadioGroupItem垂直头样式（从上至下） | RadioGroupItemBaseStyle |
| RadioGroupItemVerticalLast    | RadioGroupItem垂直尾样式（从上至下） | RadioGroupItemBaseStyle |

案例：

单个`RadioButton`样式：

```xml
<RadioButton Style="{DynamicResource RadioGroupItemSingle}"
                 Margin="0,16,8,0"
                 Content="RadioGroupItemSingle"></RadioButton>
```

效果：

![RadioButton.RadioButtonItemSingleStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RadioButton.RadioButtonItemSingleStyle.png)

多个`RadioButton`垂直样式：

```xml
<StackPanel Orientation="Horizontal" HorizontalAlignment="Center">
    <hc:ButtonGroup Margin="0,16,8,0" Orientation="Vertical">
        <RadioButton Content="RadioButton" IsChecked="True"/>
        <RadioButton Content="RadioButton"/>
        <RadioButton IsEnabled="False" Content="RadioButton"/>
        <RadioButton Content="RadioButton"/>
    </hc:ButtonGroup>
    <hc:ButtonGroup Margin="8,16,0,0" Orientation="Vertical" Style="{StaticResource ButtonGroupSolid}">
        <RadioButton Content="RadioButton"/>
        <RadioButton Content="RadioButton"/>
        <RadioButton IsEnabled="False" Content="RadioButton"/>
        <RadioButton Content="RadioButton" IsChecked="True"/>
    </hc:ButtonGroup>
</StackPanel>
```

效果：

![RadioButton.RadioButtonItemVerticalStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RadioButton.RadioButtonItemVerticalStyle.png)
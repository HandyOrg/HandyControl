---
title: WindowAttach 窗体专用
---

# 属性

| 名称              | 用途                                                         |
| ----------------- | ------------------------------------------------------------ |
| IsDragElement     | 是否允许当前元素可拖动窗体                                   |
| IgnoreAltF4       | 是否忽略快捷键Alt和F4（键盘快速退出或者 结束 当前正在运行的应用程序） |
| ShowInTaskManager | 是否窗体显示到任务管理器中                                   |

# 使用案例

## IsDragElement 是否允许当前元素可拖动窗体

```xml
<hc:BlurWindow x:Class="类命名空间"
			 .....常规项......
             WindowStartupLocation="CenterScreen"
             ShowTitle="True"
             Style="{DynamicResource WindowBlur}"
             xmlns:hc="https://handyorg.github.io/handycontrol"
             d:DesignHeight="450" d:DesignWidth="800">
    <hc:SimplePanel>
        <Rectangle VerticalAlignment="Top" Margin="10" Height="30" RadiusX="4" RadiusY="4" Stroke="{DynamicResource BorderBrush}" StrokeDashArray="2,2"/>
        <Border hc:WindowAttach.IsDragElement="True" VerticalAlignment="Top" Margin="11" Height="28" Background="{DynamicResource DarkDefaultBrush}" CornerRadius="4">
            <TextBlock Text="DragHere" Style="{StaticResource TextBlockDefault}"/>
        </Border>
        <Button HorizontalAlignment="Right" Margin="0,15,15,0" VerticalAlignment="Top" Padding="0" Height="20" Width="20" Style="{StaticResource ButtonPrimary}" hc:IconElement.Geometry="{StaticResource CloseGeometry}" hc:BorderElement.CornerRadius="15"/>
    </hc:SimplePanel>
</hc:BlurWindow>
```

可在深色框区域鼠标拖动窗体

![WindowAttach.IsDragElement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/WindowAttach.IsDragElement.png)

## IgnoreAltF4 是否忽略快捷键Alt和F4

用于屏蔽窗口的 Alt+F4 关闭功能。

```xml
<Setter Property="hc:WindowAttach.IgnoreAltF4" Value="True"/>
```

## ShowInTaskManager 是否将窗体显示到任务管理器中

使用前提：

- 窗口必须为非模态窗口，即不能使用`ShowDialog`显示窗口。
- 窗口必须同时设置`ShowInTaskBar`为`false`

ps：该附加属性在`Windows7`中效果不是很明显
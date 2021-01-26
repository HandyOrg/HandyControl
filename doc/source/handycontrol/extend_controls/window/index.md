---
title: Window 窗口
---

HC 对wpf原生 `Window` 的扩展.

```cs
[TemplatePart(Name = ElementNonClientArea, Type = typeof(UIElement))]
public class Window : System.Windows.Window
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|CloseButtonBackground|关闭按钮背景色|||
|CloseButtonForeground|关闭按钮前景色|||
|CloseButtonHoverBackground|关闭按钮鼠标悬浮背景色|||
|CloseButtonHoverForeground|关闭按钮鼠标悬浮前景色|||
|OtherButtonBackground|其它按钮背景色|||
|OtherButtonForeground|其它按钮前景色|||
|OtherButtonHoverBackground|其它按钮鼠标悬浮背景色|||
|OtherButtonHoverForeground|其它按钮鼠标悬浮前景色|||
|NonClientAreaContent|非客户端区域内容|||
|NonClientAreaBackground|非客户端区域背景色|||
|NonClientAreaForeground|非客户端区域前景色|||
|NonClientAreaHeight|非客户端区域高度|||
|ShowNonClientArea|是否显示非客户端区域|true||
|ShowTitle|是否显示窗口标题|true||
|IsFullScreen|窗口是否处于全屏|false|||

# 案例

```xml
<hc:Window x:Class="HandyControlDemo.Window.CommonWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
           xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
           xmlns:hc="https://handyorg.github.io/handycontrol"
           xmlns:langs="clr-namespace:HandyControlDemo.Properties.Langs"
           xmlns:ex="clr-namespace:HandyControlDemo.Tools.Extension"
           mc:Ignorable="d"
           Background="{DynamicResource MainContentBackgroundBrush}"
           WindowStartupLocation="CenterScreen"
           Title="{ex:Lang Key={x:Static langs:LangKeys.Title}}" 
           Height="450" 
           Width="800" 
           Icon="/HandyControlDemo;component/Resources/Img/icon.ico">
    <Border Background="{DynamicResource MainContentForegroundDrawingBrush}"/>
</hc:Window>
```

![Window](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Window.png)
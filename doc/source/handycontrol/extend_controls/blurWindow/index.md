---
title: BlurWindow 背景模糊窗口
---

背景模糊窗口可用于增强UI效果，但是会牺牲部分性能.

```cs
public class BlurWindow : Window
```

{% note warning %}
操作系统支持范围：win10 10240 ~ win10 18362
{% endnote %}

{% note warning %}
重写资源 `BlurGradientValue` 可自定义模糊颜色
{% endnote %}

# 案例

```xml
<system:UInt32 x:Key="BlurGradientValue">0x99FFFFFF</system:UInt32>
```

```xml
<hc:BlurWindow x:Class="HandyControlDemo.Window.BlurWindow"
               xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
               xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
               xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
               xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
               xmlns:hc="https://handyorg.github.io/handycontrol"
               xmlns:langs="clr-namespace:HandyControlDemo.Properties.Langs"
               xmlns:ex="clr-namespace:HandyControlDemo.Tools.Extension"
               mc:Ignorable="d"
               Style="{StaticResource WindowBlur}"
               WindowStartupLocation="CenterScreen"
               Title="{ex:Lang Key={x:Static langs:LangKeys.Title}}"
               Height="450" 
               Width="800" 
               Icon="/HandyControlDemo;component/Resources/Img/icon.ico">
</hc:BlurWindow>
```

![BlurWindow](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/BlurWindow.gif)
---
title: TabControl 选项卡控件
---

HC 对wpf原生 `TabControl` 的扩展.

```cs
[TemplatePart(Name = OverflowButtonKey, Type = typeof(ContextMenuToggleButton))]
[TemplatePart(Name = HeaderPanelKey, Type = typeof(TabPanel))]
[TemplatePart(Name = OverflowScrollviewer, Type = typeof(ScrollViewer))]
[TemplatePart(Name = ScrollButtonLeft, Type = typeof(ButtonBase))]
[TemplatePart(Name = ScrollButtonRight, Type = typeof(ButtonBase))]
[TemplatePart(Name = HeaderBorder, Type = typeof(Border))]
public class TabControl : System.Windows.Controls.TabControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|IsAnimationEnabled|是否启用动画效果|false||
|IsDraggable|选项卡是否支持拖动|false||
|ShowCloseButton|选项卡上是否显示关闭按钮|false||
|ShowContextMenu|选项卡是否支持上下文菜单|true||
|IsTabFillEnabled|选项卡是否自动平分空间|false||
|TabItemWidth|选项卡宽度|200||
|TabItemHeight|选项卡高度|30||
|IsScrollable|选项卡溢出后，是否支持鼠标滚轮滚动|false||
|ShowOverflowButton|选项卡溢出后，是否显示下拉按钮|true||
|ShowScrollButton|是否显示滚动按钮|false|||

# 案例

```xml
<hc:TabControl IsAnimationEnabled="True" ShowCloseButton="True" IsDraggable="True" IsTabFillEnabled="True" Width="800" Height="300">
    <hc:TabItem Header="TabItem1">
        <hc:SimplePanel Background="{DynamicResource RegionBrush}"/>
    </hc:TabItem>
    <hc:TabItem IsSelected="True" Header="TabItem2">
        <hc:SimplePanel Background="#FFE8563F"/>
    </hc:TabItem>
    <hc:TabItem Header="TabItem3">
        <hc:SimplePanel Background="#FF3F4EE8"/>
    </hc:TabItem>
    <hc:TabItem Header="TabItem4">
        <hc:SimplePanel Background="#FFE83F6D"/>
    </hc:TabItem>
    <hc:TabItem Header="TabItem5">
        <hc:SimplePanel Background="#FFB23FE8"/>
    </hc:TabItem>
    <hc:TabItem Header="TabItem6">
        <hc:SimplePanel Background="#FF3FE8E8"/>
    </hc:TabItem>
    <hc:TabItem Header="TabItem7">
        <hc:SimplePanel Background="#FFE8E03F"/>
    </hc:TabItem>
</hc:TabControl>
```

![TabControl](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/TabControl.gif)

## TabItem

你可以使用 `IconElement` 类对选项卡设置图标.
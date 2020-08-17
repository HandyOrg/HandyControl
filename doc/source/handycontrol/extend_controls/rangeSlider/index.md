---
title: RangeSlider 范围滑块
---

支持选择某一数值范围.

```cs
public class TwoWayRangeBase : Control
```

```cs
[DefaultEvent("ValueChanged"), DefaultProperty("Value")]
[TemplatePart(Name = ElementTrack, Type = typeof(Track))]
public class RangeSlider : TwoWayRangeBase
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Minimum|最小值|0||
|Maximum|最大值|0||
|ValueStart|选中范围的开始|0||
|ValueEnd|选中范围的结束|0||
|LargeChange|单次最大改变值|1||
|SmallChange|单次最小改变值|0.1||
|Orientation|方向|Orientation.Horizontal||
|IsDirectionReversed|是否反向|false||
|Delay|重复点击前的等待时间|||
|Interval|重复点击触发时间间隔|||
|AutoToolTipPlacement|自动工具提示位置|AutoToolTipPlacement.None||
|AutoToolTipPrecision|自动工具提示小数位数|0||
|IsSnapToTickEnabled|是否自动将 Thumb 移动到最近的刻度线|false||
|TickPlacement|刻度线的位置|TickPlacement.None||
|TickFrequency|刻度线之间的间隔|1||
|Ticks|刻度线绘制集合|||
|IsMoveToPointEnabled|Thumb 是否能够立即移动至鼠标点击的位置|false|||

# 事件

|名称|说明|
|-|-|
| ValueChanged | 选中范围改变时触发 |

# 案例

```xml
<StackPanel Margin="32" Orientation="Horizontal">
    <StackPanel>
        <hc:RangeSlider Width="400" IsSnapToTickEnabled="True" ValueStart="2" ValueEnd="8"/>
        <hc:RangeSlider Width="400" IsSnapToTickEnabled="True" ValueEnd="3" Margin="0,32,0,0" IsEnabled="False"/>
        <hc:RangeSlider Width="400" hc:TipElement.Visibility="Visible" hc:TipElement.Placement="Top" IsSnapToTickEnabled="True" Maximum="100" ValueEnd="60" TickFrequency="10" TickPlacement="BottomRight" Margin="0,32,0,0"/>
        <hc:RangeSlider Width="400" hc:TipElement.Visibility="Visible" hc:TipElement.Placement="Bottom" hc:TipElement.StringFormat="#0.00" ValueEnd="5" TickPlacement="Both" Margin="0,32,0,0"/>
    </StackPanel>
    <StackPanel Margin="32,0,0,0" Orientation="Horizontal">
        <hc:RangeSlider Height="400" IsSnapToTickEnabled="True" ValueEnd="8" Orientation="Vertical"/>
        <hc:RangeSlider Height="400" IsSnapToTickEnabled="True" ValueEnd="3" Margin="32,0,0,0" IsEnabled="False" Orientation="Vertical"/>
        <hc:RangeSlider Height="400" hc:TipElement.Visibility="Visible" hc:TipElement.Placement="Left" IsSnapToTickEnabled="True" Maximum="100" ValueEnd="60" TickFrequency="10" TickPlacement="BottomRight" Margin="32,0,0,0" Orientation="Vertical"/>
        <hc:RangeSlider Height="400" hc:TipElement.Visibility="Visible" hc:TipElement.Placement="Right" hc:TipElement.StringFormat="#0.00" ValueEnd="5" TickPlacement="Both" Margin="32,0" Orientation="Vertical"/>
    </StackPanel>
</StackPanel>
```

![RangeSlider](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/RangeSlider.png)
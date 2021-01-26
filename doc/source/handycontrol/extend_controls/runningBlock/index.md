---
title: RunningBlock 滚动块
---

滚动块可用于对连续性内容进行轮播.

```cs
[TemplatePart(Name = ElementContent, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class RunningBlock : ContentControl
```

|属性|描述|默认值|备注|
|-|-|-|-|
|Runaway|滚动内容是否可以脱离视野|true||
|AutoRun|是否自动触发滚动|false|当显示区域不够时才触发滚动|
|Orientation|滚动方向|Orientation.Horizontal||
|Duration|滚动一个周期需要的时间|5s||
|Speed|滚动速度|NaN|如果设定了值，则会覆盖Duration的值|
|IsRunning|是否处于滚动中|true||
|AutoReverse|是否反转滚动|false|||

# 案例

```xml
<StackPanel Margin="32" VerticalAlignment="Center">
    <hc:RunningBlock Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=5}"/>
    <hc:RunningBlock Content="{ex:Lang Key={x:Static langs:LangKeys.Text}, Converter={StaticResource StringRepeatConverter}, ConverterParameter=10}" FontSize="20" FontWeight="Bold" Foreground="OrangeRed" Duration="0:0:10" Margin="0,16,0,0"/>
    <hc:RunningBlock Margin="0,16,0,0" Duration="0:0:20" IsRunning="{Binding IsMouseOver,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2BooleanReConverter}}">
        <StackPanel Orientation="Horizontal">
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/1.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/2.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/3.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/4.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/5.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/6.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/7.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/8.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/9.gif" Margin="10,0" Width="30" Height="30"/>
            <hc:GifImage Uri="/HandyControlDemo;component/Resources/Img/QQ/10.gif" Margin="10,0" Width="30" Height="30"/>
        </StackPanel>
    </hc:RunningBlock>
</StackPanel>
```

![RunningBlock](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/RunningBlock.gif)
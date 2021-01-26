---
title: Loading 加载条
---

耗时操作时显示动效，目前包含圆形和直线两种加载条。相关博文：[《WPF 控件库——仿制Windows10的进度条》](https://www.cnblogs.com/nabian/p/9288576.html)

```cs
public abstract class LoadingBase : ContentControl
```

```cs
public class LoadingLine : LoadingBase
```

```cs
public class LoadingCircle : LoadingBase
```

# LoadingBase 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|IsRunning|是否处于加载|true||
|DotCount|圆点数目|5||
|DotInterval|圆点间隙|10||
|DotBorderBrush|圆点边框颜色|||
|DotBorderThickness|圆点边框粗细|0||
|DotDiameter|圆点半径|6||
|DotSpeed|一遍循环所用时间|4s||
|DotDelayTime|各点的动画延迟|80ms|||

# LoadingCircle 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|DotOffSet|圆点偏移|20||
|NeedHidden|圆点运动中途是否需要隐藏|true|||

# 案例

```xml
<StackPanel Width="600" Margin="32" VerticalAlignment="Center">
    <hc:LoadingLine/>
    <hc:LoadingLine Margin="0,30" Foreground="BlueViolet" Style="{StaticResource LoadingLineLarge}"/>
    <StackPanel Orientation="Horizontal" HorizontalAlignment="Center">
        <hc:LoadingCircle/>
        <Border VerticalAlignment="Center" Margin="32,0,0,0" Background="{DynamicResource PrimaryBrush}" CornerRadius="10">
            <hc:LoadingCircle Style="{StaticResource LoadingCircleLight}" Margin="10"/>
        </Border>
    </StackPanel>
</StackPanel>
```

![Loading](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Loading.gif)
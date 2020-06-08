---
title: Clock 时钟
---

Clock 控件展示一个虚拟的时钟，允许用户在这个时钟上选择时间。

``` CS
[TemplatePart(Name = ElementButtonAm, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementButtonPm, Type = typeof(RadioButton))]
[TemplatePart(Name = ElementCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = ElementBorderTitle, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderClock, Type = typeof(Border))]
[TemplatePart(Name = ElementPanelNum, Type = typeof(CirclePanel))]
[TemplatePart(Name = ElementTimeStr, Type = typeof(TextBlock))]
public class Clock : ClockBase
```

# 创建Clock

``` XML
<hc:Clock />
```

``` CS
var clock = new Clock();
```

生成的Clock如下图所示：

![Clock](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Clock_1.png)

# 选择时间

可以通过点击左边的时钟，或者点击右边的时分秒列表选择时间。也可以在XAML钟或代码中这样设置日期。

``` XML
<hc:Clock SelectedTime="{x:Static system:DateTime.Now}"/>
```

``` CS
clock.SelectedTime = DateTime.Now;
```

# 属性

| 属性             |  描述             |
| ---------------- | ------------------ |
| SelectedTime      | 获取或设置当前选中的时间。 |
| DisplayTime        | 获取或设置当前的显示时间。           |
| TimeFormat      | 获取或设置用于显示选定的时间的格式 |
| ClockRadioButtonStyle    | 获取或设置Clock控件中RadioButton的样式           |

# 事件

| 事件             |   描述             |
| ---------------- | ------------------ |
| DisplayTimeChanged      | 当显示的时间改变时发生。 |

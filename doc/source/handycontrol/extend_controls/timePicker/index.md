---
title: TimePicker
---

TimePicker 控件允许用户通过在文本框中键入时间或使用下拉的日历控件来选择时间。

``` CS
[TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
[TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
[TemplatePart(Name = ElementButton, Type = typeof(Button))]
[TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
public class TimePicker : Control, IDataInput
```

# 创建TimePicker

``` XML
<hc:TimePicker />
```

``` CS
var timePicker = new TimePicker();
```

生成的TimePicker如下图所示：

![TimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/TimePicker_1.png)



# 选择时间

可以通过文本框输入时间，或者点击文本框右边的按钮打开下拉的时钟控件选择时间。

![TimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/TimePicker_2.png)

也可以在XAML钟或代码中这样设置日期。

``` XML
<hc:TimePicker SelectedTime="{x:Static system:DateTime.Now}"/>
```

``` CS
timePicker.SelectedTime = DateTime.Now;
```

# 更改时钟样式

通过更改Clock属性，TimePicker可以更改时钟的样式。

``` XML
<hc:TimePicker ShowClearButton="True" Style="{StaticResource TimePickerPlus}">
    <hc:TimePicker.Clock>
        <hc:ListClock/>
    </hc:TimePicker.Clock>
</hc:TimePicker>
```

![TimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/TimePicker_3.png)

# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到DatePicker，以向用户指示其用途。要使用这两个属性，首先需要应用 **TimePickerExtend** 或 **TimePickerPlus** 样式。

``` XML
<hc:TimePicker hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"
               hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr2}"
               Style="{StaticResource TimePickerExtend}"/>
```




# 属性

| 属性             |   描述             |
| ---------------- | ------------------ |
| SelectedTime      | 获取或设置当前选中的时间 |
| Clock      | 获取或设置下拉时钟 |
| TimeFormat      | 获取或设置用于显示选定的时间的格式 |
| DisplayTime      | 获取或设置要显示的时间 |
| IsDropDownOpen      | 获取或设置一个值，该值指示是打开还是关闭下拉时钟 |
| Text      |  	获取由 **TimePicker** 显示的文本，或设置选定的时间 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             |  描述              |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据 |


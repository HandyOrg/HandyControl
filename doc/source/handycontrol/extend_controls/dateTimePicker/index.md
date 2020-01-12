---
title: DateTimePicker
---


DatePicker 控件允许用户通过在文本框中键入日期和时间，或使用下拉的日历控件来选择日期和时间。

``` CS
[TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
[TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
[TemplatePart(Name = ElementButton, Type = typeof(Button))]
[TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
public class DateTimePicker : Control, IDataInput
```

# 创建DateTimePicker

``` XML
<hc:DateTimePicker />
```

``` CS
var dateTimePicker = new DateTimePicker();
```

生成的DateTimePicker如下图所示：

![DateTimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/DateTimePicker_1.png)



# 选择日期和时间

可以通过文本框输入日期和时间，或者点击文本框右边的按钮打开下拉的日历和时钟控件选择日期和时间。

![DateTimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/DateTimePicker_2.png)

也可以在XAML中或代码中这样设置日期。

``` XML
<hc:DateTimePicker SelectedDateTime="{x:Static system:DateTime.Now}"/>
```

``` CS
dateTimePicker.SelectedDateTime = DateTime.Now;
```

# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到DatePicker，以向用户指示其用途。要使用这两个属性，首先需要应用 **DateTimePickerExtend** 或 **DateTimePickerPlus** 样式。

``` XML
<hc:DateTimePicker ShowClearButton="True"
                   Style="{StaticResource DateTimePickerExtend}"
                   hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr1}"
                   hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"/>
```




# 属性

| 属性             | 描述               |
| ---------------- | ------------------ |
| SelectedDateTime      | 获取或设置当前选中的日期和时间 |
| DateTimeFormat      | 获取或设置用于显示选定日期和时间的格式 |
| CalendarStyle      | 获取或设置在呈现日历时使用的样式 |
| DisplayDateTime      | 获取或设置要显示的日期 |
| IsDropDownOpen      | 获取或设置一个值，该值指示是打开还是关闭下拉 Calendar |
| Text      |  	获取由 **DateTimePicker** 显示的文本，或设置选定日期和时间 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             | 描述               |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据 |
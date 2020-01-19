---
title: DatePicker
---

DatePicker 控件允许用户通过在文本框中键入日期或使用下拉的日历控件来选择日期。

``` CS
[TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
public class DatePicker : System.Windows.Controls.DatePicker, IDataInput
```

# 创建DatePicker

``` XML
<hc:DatePicker />
```

``` CS
var datePicker = new DatePicker();
```

生成的DatePicker如下图所示：

![DatePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/DatePicker_1.png)



# 选择日期

可以通过文本框输入日期，或者点击文本框右边的按钮打开下拉的日历控件选择日期。

![DatePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/DatePicker_2.png)

也可以在XAML钟或代码中这样设置日期。

``` XML
<hc:DatePicker SelectedDate="{x:Static system:DateTime.Now}"/>
```

``` CS
datePicker.SelectedDate = DateTime.Now;
```

# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到DatePicker，以向用户指示其用途。

``` XML
<hc:DatePicker hc:InfoElement.TitleWidth="140"
               hc:InfoElement.TitleAlignment="Left"
               hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"
               hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr3}" />
```




# 属性

| 属性             |  描述             |
| ---------------- | ------------------ |
| SelectedDate      | 获取或设置当前选中的日期 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             |   描述             |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据 |


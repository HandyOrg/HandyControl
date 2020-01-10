---
title: CalendarWithClock
---


CalendarWithClock 控件由日历和时钟组成，允许用户通过鼠标选择日期和时间。

``` CS
[TemplatePart(Name = ElementButtonConfirm, Type = typeof(Button))]
[TemplatePart(Name = ElementClockPresenter, Type = typeof(ContentPresenter))]
[TemplatePart(Name = ElementCalendarPresenter, Type = typeof(ContentPresenter))]
public class CalendarWithClock : Control
```

# 创建日期选取器

``` XML
<hc:CalendarWithClock />
```

``` CS
var dateTimePicker = new CalendarWithClock();
```

生成的CalendarWithClock如下图所示：

![DateTimePicker](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/CalendarWithClock_1.png)



# 选择日期和时间

通过鼠标分别在日历和时钟上选择日期和时间，然后点击 **确定** 按钮即可更新CalendarWithClock控件的日期和时间。


也可以在XAML钟或代码中这样设置日期。

``` XML
<hc:CalendarWithClock SelectedDateTime="{x:Static system:DateTime.Now}"/>
```

``` CS
calendarWithClock.SelectedDateTime = DateTime.Now;
```



# 属性

| 属性             |  描述              |
| ---------------- | ------------------ |
| SelectedDateTime      | 获取或设置当前选中的日期和时间 |
| DateTimeFormat      | 获取或设置用于显示选定日期和时间的格式 |
| DisplayDateTime      | 获取或设置要显示的日期 |
| ShowConfirmButton      |  	获取或设置一个值，该值指示是是否显示 **确定** 按钮 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             |  描述              |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据 |
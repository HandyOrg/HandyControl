---
title: NumericUpDown 数值选择控件
---

NumericUpDown 控件看起来像一对用户可以单击来调整值的箭头按钮和TextBox的组合，该控件显示并设置数值(Value)。 用户可以通过单击向上和向下箭头，或通过在控件的TextBox部分中键入一个数字来改变Value。

``` CS
[TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
public class DatePicker : System.Windows.Controls.DatePicker, IDataInput
```

# 创建NumericUpDown并设置Value

``` XML
<hc:NumericUpDown Value="100"/>
```

``` CS
var numericUpDown = new NumericUpDown();
numericUpDown.Value = 100;
```

生成的NumericUpDown如下图所示：

![NumericUpDown](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/NumericUpDown_1.png)


# 设置DecimalPlaces

如果需要改变Value显示的小数位数，可以改变DecimalPlaces。DecimalPlaces是一个 **int？** 类型，当它为null时不限制显示的小数位数，否则显示的小数位数为DecimalPlaces的Value。

``` XML
<hc:NumericUpDown DecimalPlaces="2" Value="100.12345"/>
```

``` CS
numericUpDown.Value = 10.12345;
numericUpDown.DecimalPlaces = 2;
```

![NumericUpDown](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/NumericUpDown_2.png)

# 设置Increment

可以通设置Increment改变单击一下按钮时增加或减少的数量。

``` XML
<hc:NumericUpDown Value="100" Increment="10"/>
```

``` CS
numericUpDown.Increment = 10;
```

# 设置上限和下限

可以通设置Maximum和Minimum限制Value的上限和下限。

``` XML
<hc:NumericUpDown Maximum="500" Minimum="10"/>
```

``` CS
numericUpDown.Minimum = 10;
numericUpDown.Maximum = 1000;
```

# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到NumericUpDown，以向用户指示其用途。要使用这两个附加属性，需要先应用 **NumericUpDownPlus** 样式。

``` XML
<hc:NumericUpDown hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"
                  hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr1}"
                  Style="{StaticResource NumericUpDownExtend}" />
```

![NumericUpDown](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/NumericUpDown_3.png)

# 属性

| 属性             |  描述             |
| ---------------- | ------------------ |
| Value      | 获取或设置当前值 |
| Maximum      | 获取或设置最大允许值 |
| Minimum      | 获取或设置最小允许值 |
| Increment      | 获取或设置单击向上或向下按钮时，数字显示框（也称作 up-down 控件）递增或递减的值。 |
| DecimalPlaces      |获取或设置NumericUpDown中要显示的十进制位数。 此属性不会影响 Value 属性。 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             |   描述             |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据 |

# 事件

| 事件             |   描述             |
| ---------------- | ------------------ |
| ValueChanged      | 在以某种方式更改 Value 属性后发生。 |

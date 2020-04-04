---
title: PasswordBox 密码框
---

表示用于输入和处理密码的控件。

``` CS
[TemplatePart(Name = ElementPasswordBox, Type = typeof(System.Windows.Controls.PasswordBox))]
[TemplatePart(Name = ElementTextBox, Type = typeof(System.Windows.Controls.TextBox))]
public class PasswordBox : Control, IDataInput
```

# 创建PasswordBox

``` XML
<hc:PasswordBox />
```

``` CS
var passwordBox = new PasswordBox();
```

# 输入密码

可以通过文本框输入密码，也可以在XAML钟或代码中这样设置日期。

``` XML
<hc:PasswordBox Password="123456"/>
```

``` CS
passwordBox.Password = "123456";
```

# 显示密码

PasswordBox控件可以主动显示已输入的密码。将`ShowEyeButton`设置为`true`，PasswordBox将显示一个“眼睛”的按钮，点击这个按钮即可显示已输入的密码。

``` XML
<hc:PasswordBox ShowClearButton="True"/>
```

![PasswordBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/PasswordBox_2.gif)


# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到PasswordBox，以向用户指示其用途。

``` XML
<hc:PasswordBox hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"
               hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr2}"
```

# 属性

| 属性             |   描述             |
| ---------------- | ------------------ |
| Password      | 获取或设置 PasswordBox 当前保留的密码。 |
| PasswordChar      | 获取或设置 PasswordBox 的掩码字符。 |
| ShowEyeButton      | 获取或设置是否显示一个“眼睛”的按钮，点击可以显示密码。 |
| ShowPassword      | 获取或设置是否显示密码。 |
| Text      |  	获取由 **TimePicker** 显示的文本，或设置选定的时间 |
| VerifyFunc        | 获取或设置数据验证委托           |
| IsError           | 获取或设置数据是否错误           |
| ErrorStr    | 获取或设置错误提示           |
| TextType | 获取或设置文本类型       |
| ShowClearButton | 获取或设置是否显示清除按钮       |

# 方法

| 方法             |  描述              |
| ---------------- | ------------------ |
| Clear()    | 清除 Password 属性的值。 |
| Paste()      | 用剪贴板中的内容替换 PasswordBox 中的当前选定内容。 |
| SelectAll()     | 选择 PasswordBox 中的全部内容。 |
| VerifyData()      | 验证数据。 |

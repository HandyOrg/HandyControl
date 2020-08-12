---
title: PinBox PIN码框
---

密码框的另一种形式.

```cs
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class PinBox : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Password|密码||出于安全考虑，无法绑定|
|PasswordChar|掩码字符|●||
|Length|密码长度|4|最小值为4|
|ItemMargin|单元框间隔|||
|ItemWidth|单元框宽度|||
|ItemHeight|单元框高度||||

# 事件

|名称|说明|
|-|-|
| Completed | 输入完成时触发 |

# 案例

```xml
<StackPanel Margin="32" VerticalAlignment="Center" hc:PinBox.Completed="PinBox_OnCompleted">
    <hc:PinBox Length="4" Password="1234"/>
    <hc:PinBox Length="6" Password="123456" Margin="0,16,0,0" PasswordChar="❤"/>
</StackPanel>
```

![PinBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/PinBox.png)
---
title: TextBox 文本框
---

表示一个控件，该控件可用于显示或编辑无格式文本。

``` CS
public class TextBox : System.Windows.Controls.TextBox, IDataInput
```

# 创建TextBox

``` XML
<hc:TextBox />
```

``` CS
var textBox = new TextBox();
```

# 输入文本

此示例演示如何使用 Text 属性设置 TextBox 控件的初始文本内容：

``` XML
<hc:TextBox Text="这是内容"/>
```

``` CS
textBox.Text = "这是内容";
```

生成的TextBox如下图所示：

![TextBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/TextBox_1.png)


有关TextBox的更多示例，可以参考它的基类的[文档](https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.controls.textbox?view=netframework-4.8)。


# 设置标题和占位符文本

你可以将 Header（标题）和 Placeholder（占位符）添加到TextBox，以向用户指示其用途。要使用这两个属性，

``` XML
<hc:TextBox hc:InfoElement.Placeholder="{x:Static langs:Lang.PlsEnterContent}"
               hc:InfoElement.Title="{x:Static langs:Lang.TitleDemoStr2}"
```

# 属性

| 属性             |   描述             |
| ---------------- | ------------------ |
| Text      |  	获取或设置文本框的文本内容。 |
| VerifyFunc        | 获取或设置数据验证委托。           |
| IsError           | 获取或设置数据是否错误。           |
| ErrorStr    | 获取或设置错误提示。           |
| TextType | 获取或设置文本类型。       |
| ShowClearButton | 获取或设置是否显示清除按钮。       |

# 方法

| 方法             |  描述              |
| ---------------- | ------------------ |
| VerifyData()      | 验证数据。 |


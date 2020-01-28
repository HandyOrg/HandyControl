---
title: RichTextBox 富文本框
---

# TextBoxBaseBaseStyle

该样式属于文本框基础样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

```xml
<Style x:Key="RichTextBoxBaseStyle" BasedOn="{StaticResource TextBoxBaseBaseStyle}" TargetType="RichTextBox">
   <Setter Property="MinWidth" Value="10"/>
</Style>
```

# RichTextBoxBaseStyle:TextBoxBaseBaseStyle

富文本框的基础样式，作为默认富文本框的基类样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用

案例：

```xml
<Style BasedOn="{StaticResource RichTextBoxBaseStyle}" TargetType="RichTextBox"/>
```

`xaml`中：

```xml
<RichTextBox Margin="10,10" VerticalAlignment="Center" HorizontalAlignment="Center"></RichTextBox>
```



效果：

![RichTextbox.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/RichTextbox.DefaultStyle.png)






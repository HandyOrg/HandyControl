---
title: TextBox 文本框
---

# TextBoxBaseStyle

原生文本框默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

- 默认样式
`<TextBox Width="200" VerticalAlignment="Center"/>`
![TextBoxBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TextBoxBaseStyle.png)

# TextBoxExtendBaseStyle : TextBoxBaseStyle

原生文本框扩展默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

# TextBoxExtend : TextBoxExtendBaseStyle

相对于原生文本框默认样式，它借助于附加属性可以实现标题、水印的功能。

- 标题在上
`<TextBox Style="{StaticResource TextBoxExtend}" hc:InfoElement.Title="这是标题" Width="200" VerticalAlignment="Center"/>`
![TextBoxExtend_1](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TextBoxExtend_1.png)

- 标题在左
`<TextBox Style="{StaticResource TextBoxExtend}" hc:InfoElement.TitleAlignment="Left" hc:InfoElement.Title="这是标题" Width="300" VerticalAlignment="Center"/>`
![TextBoxExtend_2](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TextBoxExtend_2.png)

{% note warning %}
标题在左时，为了多个输入框左侧对齐，需要设置标题宽度，标题宽度无需逐个设置，可在外部容器上统一设置。
{% endnote %}

- 标题在上，带有水印
[参见Combobox](https://handyorg.github.io/handycontrol/native_controls/comboBox/)

- 标题在上，带有水印，且为必填
[参见Combobox](https://handyorg.github.io/handycontrol/native_controls/comboBox/)

- 标题在上，带有水印，且为必填，同时自定义必填提示符
[参见Combobox](https://handyorg.github.io/handycontrol/native_controls/comboBox/)
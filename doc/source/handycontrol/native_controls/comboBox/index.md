---
title: ComboBox 组合框
---

# ComboBoxBaseStyle

原生组合框默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。原生组合框如果未设置任何样式，则默认使用该样式：

- 默认样式
`<ComboBox ItemsSource="{Binding DataList}" SelectedIndex="0"/>`
![ComboBoxBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxBaseStyle.png)

# ComboBoxExtendBaseStyle : ComboBoxBaseStyle

原生组合框扩展默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

# ComboBoxExtend : ComboBoxExtendBaseStyle

相对于原生组合框默认样式，它借助于附加属性可以实现标题、水印的功能。

- 标题在上
`<ComboBox ItemsSource="{Binding DataList}" SelectedIndex="0" hc:InfoElement.Title="这是标题" Style="{StaticResource ComboBoxExtend}" Text="正文1"/>`
![ComboBoxExtend_1](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_1.png)

- 标题在左
`<ComboBox ItemsSource="{Binding DataList}" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="标题在左侧" Style="{StaticResource ComboBoxExtend}" Text="正文1"/>`
![ComboBoxExtend_2](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_2.png)

{% note warning %}
标题在左时，为了多个输入框左侧对齐，需要设置标题宽度，标题宽度无需逐个设置，可在外部容器上统一设置。
{% endnote %}
{% note warning %}
常见问题：
Combobox的字体比原有默认字体大，同时设定标题在左，标题因字体过大显示不全
![ComboBoxExtend_2_Error1](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_2_Error1.png)
解决方案：
在该Combobox中添加附加属性`hc:InfoElement.ContentHeight`，默认值为30，默认字体号为12，变更后可根据新的字体大小设定该附加属性的数值，如上图，字体大小为18，则设定`hc:InfoElement.ContentHeight`为38，结果如下：![ComboBoxExtend_2_Ok1](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_2_Ok1.png)
{% endnote %}
- 标题在上，带有水印
`<ComboBox ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="请输入内容" hc:InfoElement.Title="此项必填" Style="{StaticResource ComboBoxExtend}"/>`
![ComboBoxExtend_3](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_3.png)

- 标题在上，带有水印，且为必填
`<ComboBox ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="请输入内容" hc:InfoElement.Title="此项必填" Style="{StaticResource ComboBoxExtend}" hc:InfoElement.Necessary="True"/>`
![ComboBoxExtend_4](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_4.png)

- 标题在上，带有水印，且为必填，同时自定义必填提示符
`<ComboBox ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="请输入内容" hc:InfoElement.Title="此项必填" Style="{StaticResource ComboBoxExtend}" hc:InfoElement.Necessary="True" hc:InfoElement.Symbol="*"/>`
![ComboBoxExtend_5](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ComboBoxExtend_5.png)
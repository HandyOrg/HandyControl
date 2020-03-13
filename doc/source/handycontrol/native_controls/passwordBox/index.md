---
title: PasswordBox 密码框
---

# PasswordBoxBaseStyle

原生密码框默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <PasswordBox PasswordChar="*" VerticalAlignment="Center" Width="120"></PasswordBox>
{% endcode %}

![PasswordBox.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/PasswordBox.BaseStyle.png)

{% endnote %}

# PasswordBoxExtendBaseStyle : PasswordBoxBaseStyle

原生密码框扩展默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

# PasswordBoxExtend : PasswordBoxExtendBaseStyle

相对于原生密码框默认样式，它借助于附加属性可以实现标题、水印的功能。

{% note info no-icon %}
用例：
{% code %}
    <!--为使普通密码输入文本框显示水印,需要设定PasswordBoxAttach.PasswordLength="0"-->
    <PasswordBox Style="{DynamicResource PasswordBoxExtend}" PasswordChar="*" 
                 hc:PasswordBoxAttach.PasswordLength="0"
                 hc:InfoElement.Placeholder="请输入密码" 
                 VerticalAlignment="Center"
                 Width="120"></PasswordBox>
    <PasswordBox Style="{DynamicResource PasswordBoxExtend}" PasswordChar="*" 
                 hc:TitleElement.Title="用户密码："
                 hc:TitleElement.TitleAlignment="Top"
                 VerticalAlignment="Center"
                 Width="120"></PasswordBox>
{% endcode %}

![PasswordBox.ExtendStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/PasswordBox.ExtendStyle.png)

{% endnote %}
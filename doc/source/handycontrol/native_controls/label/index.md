---
title: Lable标签
---

# LabelBaseStyle

Label标签默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <Label Content="Label默认样式" Margin="10"></Label>
{% endcode %}

![Label.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.BaseStyle.png)

{% endnote %}

# LabelPrimary : LabelBaseStyle

主样式

{% note info no-icon %}
用例：
{% code %}
    <Label Content="LabelPrimary样式" Margin="10" Style="{DynamicResource LabelPrimary}"></Label>
{% endcode %}

![Label.PrimaryStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.PrimaryStyle.png)

{% endnote %}

# LabelSuccess : LabelBaseStyle

成功类型样式

{% note info no-icon %}
用例：
{% code %}
    <Label Content="LabelSuccess样式" Margin="10" Style="{DynamicResource LabelSuccess}"></Label>
{% endcode %}

![Label.SuccessStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.SuccessStyle.png)

{% endnote %}

# LabelInfo : LabelBaseStyle

信息类型样式

{% note info no-icon %}
用例：
{% code %}
    <Label Content="LabelInfo样式" Margin="10" Style="{DynamicResource LabelInfo}"></Label>
{% endcode %}

![Label.InfoStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.InfoStyle.png)

{% endnote %}

# LabelWarning : LabelBaseStyle

警告类型样式

{% note info no-icon %}
用例：
{% code %}
    <Label Content="LabelWarning样式" Margin="10" Style="{DynamicResource LabelWarning}"></Label>
{% endcode %}

![Label.WarningStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.WarningStyle.png)

{% endnote %}

# LabelDanger : LabelBaseStyle

危险类型样式

{% note info no-icon %}
用例：
{% code %}
    <Label Content="LabelDanger样式" Margin="10" Style="{DynamicResource LabelDanger}"></Label>
{% endcode %}

![Label.DangerStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.DangerStyle.png)

{% endnote %}
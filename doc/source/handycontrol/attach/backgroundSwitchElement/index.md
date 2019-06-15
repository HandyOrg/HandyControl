---
title: backgroundSwitchElement
---

# 附件属性名称
* MouseHoverBackground 鼠标经过背景颜色
* MouseDownBackground  鼠标按下背景颜色

## MouseHoverBackground `鼠标经过背景颜色`
用途：该附加属性常用于自定义控件`Style`时，鼠标经过控件，属性触发器`IsMouseOver`为`true`时，设定目标控件当前状态背景色。
{% note info no-icon %}
用例：

{% code %}
    <Trigger Property="IsMouseOver" Value="True">
        <Setter Property="Background" TargetName="Chrome" Value="{Binding Path=(controls:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
    </Trigger>
{% endcode %}

{% endnote %}
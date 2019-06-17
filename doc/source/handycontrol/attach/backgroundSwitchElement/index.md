---
title: BackgroundSwitchElement 可切换背景的元素
---

# 准备工作
添加`HandyControl`命名空间
{% code %}
xmlns:hc="https://handyorg.github.io/handycontrol"
{% endcode %}

## MouseHoverBackground `鼠标悬浮背景色`

该附加属性常用于设置控件在鼠标悬浮时的背景色，在控件的样式或者模板中我们添加如下的触发器代码：

{% code %}
<Trigger Property="IsMouseOver" Value="True">
    <Setter Property="Background" TargetName="Chrome" Value="{Binding Path=(controls:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
</Trigger>
{% endcode %}

随后我们就可以使用该属性了：

{% code %}
<目标控件  controls:BackgroundSwitchElement.MouseHoverBackground ="Blue"/>
{% endcode %}

## MouseDownBackground  `鼠标按下背景色`

该附加属性常用于设置控件在鼠标按下时的背景色，在控件的样式或者模板中我们添加如下的触发器代码：

{% code %}
<Trigger Property="IsPressed" Value="True">
    <Setter Property="Background" TargetName="Chrome" 
    Value="{Binding Path=(controls:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
</Trigger>
{% endcode %}

随后我们就可以使用该属性了：

{% code %}
<目标控件 controls:BackgroundSwitchElement.MouseDownBackground ="Yellow"/>
{% endcode %}
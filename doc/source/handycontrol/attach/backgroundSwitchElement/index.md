---
title: BackgroundSwitchElement
---

### 附件属性名称
* MouseHoverBackground 鼠标悬浮背景颜色
* MouseDownBackground  鼠标按下背景颜色

### 准备工作
使用的当前`.xaml`页面中引入`HandyCntrol`的控件命名空间
{% code %}
xmlns:controls="https://handyorg.github.io/handycontrol"
或
xmlns:controls="clr-namespace:HandyControl.Controls;assembly=HandyControl"（老版本）
{% endcode %}

### MouseHoverBackground `鼠标悬浮背景颜色`
用途：该附加属性常用于自定义控件`Style`时，鼠标悬浮在控件上，属性触发器`IsMouseOver`为`true`时，设定目标控件当前鼠标悬浮状态背景色。

{% note info no-icon %}
#### 视图控件内添加MouseHoverBackground属性：

{% endnote %}
{% code %}
<控件  controls:BackgroundSwitchElement.MouseHoverBackground ="Blue"/>
{% endcode %}


{% note info no-icon %}
#### IsMouseOver属性触发中使用如下：
{% endnote %}
{% code %}
    <Trigger Property="IsMouseOver" Value="True">
        <Setter Property="Background" TargetName="Chrome" 
        Value="{Binding Path=(controls:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
    </Trigger>
{% endcode %}


### MouseDownBackground  `鼠标按下背景颜色`
用途：该附加属性常用于自定义控件`Style`时，鼠标悬浮在控件上，属性触发器`IsPressed`为`true`时，设定目标控件当前鼠标按下状态背景色。

{% note info no-icon %}
#### 视图控件内添加MouseDownBackground属性：
{% endnote %}
{% code %}
<控件 controls:BackgroundSwitchElement.MouseDownBackground ="Yellow"/>
{% endcode %}
{% note info no-icon %}
#### IsPressed属性触发中使用如下：
{% endnote %}
{% code %}
    <Trigger Property="IsPressed" Value="True">
        <Setter Property="Background" TargetName="Chrome" 
        Value="{Binding Path=(controls:BackgroundSwitchElement.MouseHoverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
    </Trigger>
{% endcode %}
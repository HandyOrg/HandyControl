---
title: Expander 展开框 
---

# ExpanderBaseStyle

Expander 展开框 默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
<StackPanel Margin="32" VerticalAlignment="Center" Width="240">
    <Expander Header="{x:Static langs:Lang.Title}" BorderThickness="1,1,1,0" BorderBrush="{DynamicResource BorderBrush}">
        <Border Height="100" Background="{DynamicResource SecondaryRegionBrush}"/>
    </Expander>
    <Expander Header="{x:Static langs:Lang.Title}" BorderThickness="1,1,1,0" BorderBrush="{DynamicResource BorderBrush}">
        <Border Height="100" Background="{DynamicResource SecondaryRegionBrush}"/>
    </Expander>
    <Expander Header="{x:Static langs:Lang.Title}" BorderThickness="1,1,1,0" BorderBrush="{DynamicResource BorderBrush}">
        <Border Height="100" Background="{DynamicResource SecondaryRegionBrush}"/>
    </Expander>
    <Expander Header="{x:Static langs:Lang.Title}" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
        <Border Height="100" Background="{DynamicResource SecondaryRegionBrush}"/>
    </Expander>
</StackPanel>
{% endcode %}

![ExpanderBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ExpanderBaseStyle.png)

{% endnote %}
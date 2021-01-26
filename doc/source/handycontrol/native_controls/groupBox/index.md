---
title: GroupBox 分组框
---

# GroupBoxBaseStyle 

GroupBox 分组框 默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <GroupBox Grid.Row="0" Grid.Column="0" Width="300" Height="200" Header="{x:Static langs:Lang.TitleDemoStr1}" 
        Padding="10" Margin="16">
        <Border Background="{DynamicResource PrimaryBrush}" CornerRadius="4">
            <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" 
            Foreground="White"/>
        </Border>
    </GroupBox>
{% endcode %}
![GroupBoxBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Base.png)
{% endnote %}

这里提供另外一种样式,供开发者选择

{% note info no-icon %}
只需要添加扩展属性即可
{% code %}
hc:TitleElement.TitlePlacement="Left"
{% endcode %}
用例：
{% code %}
    <GroupBox Grid.Row="0" Grid.Column="1" Width="300" Height="200" Header="{x:Static langs:Lang.TitleDemoStr1}" Padding="10" 
        Margin="16"  hc:TitleElement.TitlePlacement="Left">
        <Border Background="{DynamicResource PrimaryBrush}" CornerRadius="4">
            <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" 
            Foreground="White"/>
        </Border>
    </GroupBox>
{% endcode %}

![GroupBox_Base_left](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Base_left.png)
{% endnote %}

# GroupBoxTab : GroupBoxTabBaseStyle : GroupBoxBaseStyle

GroupBox 分组框 的另一种样式  GroupBoxTabBaseStyle 不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <GroupBox Grid.Row="1" Grid.Column="0" Width="300" Height="200" Header="{x:Static langs:Lang.TitleDemoStr1}" Padding="10" 
        Margin="16" Style="{StaticResource GroupBoxTab}">
        <Border Background="{DynamicResource PrimaryBrush}" CornerRadius="4">
            <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" 
            Foreground="White"/>
        </Border>
    </GroupBox>
{% endcode %}

- 同样可以 使用扩展属性  {% code %} hc:TitleElement.TitlePlacement="Left" {% endcode %}

![GroupBox_Tab](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Tab.png) ![GroupBox_Tab_left](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Tab_left.png)
{% endnote %}

# GroupBoxOriginal : GroupBoxOriginalBaseStyle : GroupBoxBaseStyle

GroupBox 分组框 的另一种样式 GroupBoxOriginalBaseStyle 不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <GroupBox Grid.Row="2" Grid.Column="0" Width="300" Header="{x:Static langs:Lang.TitleDemoStr1}" Margin="16" 
        Style="{StaticResource GroupBoxOriginal}" HorizontalContentAlignment="Left">
        <TextBox/>
    </GroupBox>
    <GroupBox Grid.Row="2" VerticalAlignment="Bottom" Grid.Column="1" Width="300" hc:TitleElement.TitleWidth="100"
        Header="{x:Static langs:Lang.TitleDemoStr1}" Margin="16" Style="{StaticResource GroupBoxOriginal}"
        HorizontalContentAlignment="Left" hc:TitleElement.TitlePlacement="Left">
        <ComboBox DataContext="{Binding ComboBoxDemo,Source={StaticResource Locator}}" ItemsSource="{Binding DataList}"/>
    </GroupBox>
{% endcode %}

![GroupBox_Origin](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Origin.png) ![GroupBox_Origin_left](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/GroupBox_Origin_left.png)
{% endnote %}
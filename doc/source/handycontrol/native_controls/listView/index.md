---
title: ListView 列表视图
---

`HandyControl`中仅提供了一个默认的`ListView`样式，可根据个人需求，自行定义。

{% note info no-icon %}
用例：
{% code %}
    <ListView ItemsSource="{Binding DataList}" Margin="20">
        <ListView.View>
            <GridView>
                <GridViewColumn Width="80" Header="标题1" DisplayMemberBinding="{Binding Index}"/>
                <GridViewColumn Width="100" Header="标题2" DisplayMemberBinding="{Binding Name}"/>
                <GridViewColumn Width="200" Header="标题3" DisplayMemberBinding="{Binding Remark}"/>
            </GridView>
        </ListView.View>
    </ListView>
{% endcode %}

![ListView.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListView.DefaultStyle.png)

{% endnote %}
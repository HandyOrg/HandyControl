---
title: Menu 菜单
---

# MenuBaseStyle

菜单默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <Menu ItemsSource="{Binding Menus}">
        <Menu.ItemTemplate>
            <HierarchicalDataTemplate ItemsSource="{Binding Children}">
                <TextBlock Text="{Binding Name}"></TextBlock>
            </HierarchicalDataTemplate>
        </Menu.ItemTemplate>
</Menu>
{% endcode %}

![Menu.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Menu.BaseStyle.png)

{% endnote %}
---
title: ContextMenu 上下文菜单
---

# ContextMenuBaseStyle

上下文菜单默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：

{% code %}
<ContextMenu ItemsSource="{Binding DataList}">
    <ContextMenu.ItemTemplate>
        <HierarchicalDataTemplate ItemsSource="{Binding DataList}">
            <TextBlock Text="{Binding Name}"/>
        </HierarchicalDataTemplate>
    </ContextMenu.ItemTemplate>
</ContextMenu>
{% endcode %}
![ContextMenu](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ContextMenu.png)
{% endnote %}
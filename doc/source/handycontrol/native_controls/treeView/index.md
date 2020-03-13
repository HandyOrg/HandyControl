---
title: TreeView 树视图
---

# TreeViewBaseStyle : BaseStyle

树视图默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

 ```xml
<TreeView Width="200" VerticalAlignment="Center">
    <TreeViewItem Header="111">
        <TreeViewItem Header="111"/>
        <TreeViewItem Header="222"/>
        <TreeViewItem Header="333"/>
    </TreeViewItem>
    <TreeViewItem Header="222">
        <TreeViewItem Header="111"/>
        <TreeViewItem Header="222"/>
        <TreeViewItem Header="333"/>
    </TreeViewItem>
</TreeView>
 ```

效果：

![TreeViewBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TreeViewBaseStyle.png)

# 相关样式

| 名称 | 继承自 | 用途描述 |
| - | - | - |
| TreeViewItemBaseStyle     | BaseStyle   | 树视图项默认样式       |
| ExpandCollapseToggleStyle     |    | 树视图折叠按钮样式       |
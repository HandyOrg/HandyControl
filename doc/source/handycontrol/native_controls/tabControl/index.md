---
title: TabControl 选项卡控件
---

# TabControlBaseStyle

选项卡控件默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

 ```xml
<TabControl Margin="10">
    <TabItem Header="选项卡1">
    </TabItem>
    <TabItem Header="选项卡2">
    </TabItem>
    <TabItem Header="选项卡3">
    </TabItem>
</TabControl>
 ```

效果：

![TabControl.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.DefaultStyle.png)

# TabControlInLine : TabControlBaseStyle

单行填充选项卡样式

案例：

```xml
<TabControl Margin="10" Style="{StaticResource TabControlInLine}">
    <TabItem Header="选项卡1">
    </TabItem>
    <TabItem Header="选项卡2">
    </TabItem>
    <TabItem Header="选项卡3">
    </TabItem>
</TabControl>
```

效果：

![TabControl.InLineStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.InLineStyle.png)

# TabControlCapsule

胶囊状选项卡样式

案例：

```xml
<TabControl Margin="10" Style="{StaticResource TabControlCapsule}">
    <TabItem Header="选项卡1">
    </TabItem>
    <TabItem Header="选项卡2">
    </TabItem>
    <TabItem Header="选项卡3">
    </TabItem>
</TabControl>
```

效果：

![TabControl.CapsuleStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.CapsuleStyle.png)

# TabControlCapsuleSolid : TabControlCapsule

胶囊状（实心）选项卡样式

案例：

```xml
<TabControl Margin="10" Style="{StaticResource TabControlCapsuleSolid}">
    <TabItem Header="选项卡1">
    </TabItem>
    <TabItem Header="选项卡2">
    </TabItem>
    <TabItem Header="选项卡3">
    </TabItem>
</TabControl>
```

效果：

![TabControl.CapsuleSolidStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.CapsuleSolidStyle.png)

# 温馨提示

可以使用属性`TabStripPlacement`设定头部标题的位置，效果如下：

![TabControl.TabStripPlacement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.TabStripPlacement.png)
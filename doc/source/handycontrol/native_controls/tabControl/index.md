---
title: TabControl 选项卡控件
---

# TabControlBaseStyle

选项卡控件默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用

案例：

```xml
<Style BasedOn="{StaticResource TabControlBaseStyle}" TargetType="TabControl"/>
```

`xaml`中：

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

# TabControlInLine

行内选中选项卡样式

案例：

```xml
<TabControl Margin="10" Style="{DynamicResource TabControlInLine}">
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

简约选项卡样式

案例：

```xml
<TabControl Margin="10" Style="{DynamicResource TabControlCapsule}">
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

# TabControlCapsuleSolid

案例：

```xml
<TabControl Margin="10" Style="{DynamicResource TabControlCapsuleSolid}">
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

使用属性`TabStripPlacement`设定头部标题的位置，效果如下：

![TabControl.TabStripPlacement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/TabControl.TabStripPlacement.png)
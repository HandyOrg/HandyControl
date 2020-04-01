---
title: Badge 标记
---

标记控件作为其他控件的特定状态内容标记，能够做到最小程度的侵入原有控件，可以看作是一种内容控件，内部`Content`就是对应需要显示标记的被修饰控件，继承关系如下：

```c#
public class Badge : ContentControl
```

# 基础属性

| 属性名称    | 用途                                                         |
| ----------- | ------------------------------------------------------------ |
| Text        | 获取或设置标记显示内容                                       |
| Value       | 获取或设置显示数值，支持数值动态变更                         |
| Status      | 获取或设置标记状态类型，Text（文本），Dot（圆点标记），Processing（动态标记）默认为文本显示 |
| Maximum     | 获取或设置最大显示数值。数值显示时，最大显示数值             |
| BadgeMargin | 获取或设置标记相对于被修饰控件的外边距                       |
| ShowBadge   | 获取或设置是否显示标记，默认为True                           |

`xaml`中，记得引入`handycontrol`的命名空间。

```xml
xmlns:hc="https://handyorg.github.io/handycontrol"
```

##  `Text`和`Value`

可能看到属性解释，会比较疑惑，`Text`和`Value`在效果和数值上是一样的，这有什么区别？在标记控件中，将文本和数值类型进行区分，数值类型能够进行动态的变更以及数据统计显示效果，当两者同时存在时，默认显示`Text`。

### 案例

```xml
    <hc:Badge Value="12" Text="New" BadgeMargin="0,-14,-20,0" Height="30">
        <Button Content="默认样式"/>
    </hc:Badge>
    <hc:Badge Value="12" BadgeMargin="0,-14,-20,0" Height="30">
        <Button Content="默认样式"/>
    </hc:Badge>
```

### 效果

![Badge-TextValue](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-TextValue.png)

## `Maximum`最大显示数值

使用`Value`属性，添加最大显示数值限定标记显示的最大值显示效果

### 案例

```xml
    <hc:Badge Value="100" Maximum="99" BadgeMargin="0,-14,-20,0" Height="30">
        <Button Content="Maximum"/>
    </hc:Badge>
    <hc:Badge Value="2" Maximum="99" BadgeMargin="0,-14,-20,0" Height="30">
        <Button Content="Maximum"/>
    </hc:Badge>
```

### 效果

![Badge-Maximum](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-Maximum.png)

## `Status`标记状态类型

标记状态类型，分为三种类型，默认为文本显示

### 案例

```xml
    <hc:Badge Status="Text" Text="text" Height="30" Margin="32,0,0,0" Style="{DynamicResource BadgeSuccess}">
        <Button Content="Text"/>
    </hc:Badge>
    <hc:Badge Status="Dot" Height="30" Margin="32,0,0,0" Style="{DynamicResource BadgeSuccess}">
        <Button Content="Dot"/>
    </hc:Badge>
    <hc:Badge Status="Processing" Height="30" Margin="32,0,0,0" Style="{DynamicResource BadgeSuccess}">
        <Button Content="Processing" />
    </hc:Badge>
```

### 效果

![Badge-Status](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-Status.png)

## `BadgeMargin`相对外边距

`BadgeMargin`用于设定相对于被修饰控件的外边距，一般情况下该属性不做修改，建议使用`0,-14,-20,0`

### 案例

```xml
    <hc:Badge Height="30" Value="11" Style="{DynamicResource BadgeSuccess}">
        <Button Content="BadgeMargin" />
    </hc:Badge>
    <hc:Badge Height="30" Value="11" BadgeMargin="30,0,0,0" Style="{DynamicResource BadgeSuccess}">
        <Button Content="BadgeMargin" />
    </hc:Badge>
```

### 效果

![Badge-BadgeMargin](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-BadgeMargin.png)

# 事件

| 事件            | 用途               |
| --------------- | ------------------ |
| `ValueChanged ` | 数值变更后触发事件 |

# 相关样式

| 样式名称       | 用途                         | 继承样式       |
| -------------- | ---------------------------- | -------------- |
| BadgeBaseStyle | 默认样式，作为BaseOn继承使用 | -              |
| BadgePrimary   | 主题色标记样式               | BadgeBaseStyle |
| BadgeSuccess   | 成功色标记样式               | BadgeBaseStyle |
| BadgeInfo      | 信息色标记样式               | BadgeBaseStyle |
| BadgeWarning   | 警告色标记样式               | BadgeBaseStyle |
| BadgeDanger    | 异常色标记样式               | BadgeBaseStyle |

## 案例

```xml
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11">
        <Button Content="Default" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgePrimary}">
        <Button Content="Primary" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgeInfo}">
        <Button Content="Info" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgeSuccess}">
        <Button Content="Success" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgeWarning}">
        <Button Content="Warning" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgeDanger}">
        <Button Content="Danger" />
    </hc:Badge>
```

## 效果

![Badge-Styles](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-Styles.png)

# FAQ

## 坐标偏移问题[版本 小于等于 2.4]

### 问题描述

当父级容器不是`stackpanel`默认排列方式时，标记和被修饰控件出现偏移

### 现状

```xml
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11">
        <Button Content="Default" />
    </hc:Badge>
    <hc:Badge Height="30"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgePrimary}">
        <Button Content="Primary" />
    </hc:Badge>
```

![Badge-Error01](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-Error01.png)

### 解决方案

将属性`Badge`中的属性`VerticalAlignment`和`HorizontalAlignment`设置为`Center`即可

```xml
    <hc:Badge Height="30" VerticalAlignment="Center" HorizontalAlignment="Center"  BadgeMargin="0,-14,-20,0" Value="11">
        <Button Content="Default" />
    </hc:Badge>
    <hc:Badge Height="30" VerticalAlignment="Center" HorizontalAlignment="Center"  BadgeMargin="0,-14,-20,0" Value="11" Style="{DynamicResource BadgePrimary}">
        <Button Content="Primary" />
    </hc:Badge>
```

![Badge-ErrorCorrection01](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Badge-ErrorCorrection01.png)
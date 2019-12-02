---
title: Lable标签
---

# LabelBaseStyle

Label标签 默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用，

具体源码见：[LabelBaseStyle.xaml]( https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Themes/Styles/Base/LabelBaseStyle.xaml )

使用如下：

```
    <Style BasedOn="{StaticResource LabelBaseStyle}" TargetType="Label">
        <Setter Property="Foreground" Value="{DynamicResource PrimaryTextBrush}"/>
        <Setter Property="BorderThickness" Value="1"/>
    </Style>
```

案例：

```
    <Label Content="Label默认样式" Margin="10"></Label>
```

![Label.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.BaseStyle.png)

# LabelPrimary

Label标签 主样式

案例：

```
    <Label Content="LabelPrimary样式" Margin="10" 
    Style="{DynamicResource LabelPrimary}"></Label>
```

![Label.PrimaryStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.PrimaryStyle.png)

# LabelSuccess

Label标签 成功类型样式

案例：

```
    <Label Content="LabelSuccess样式" Margin="10" 
    Style="{DynamicResource LabelSuccess}"></Label>
```

![Label.SuccessStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.SuccessStyle.png)

# LabelInfo

Label标签 信息类型样式

案例：

```
    <Label Content="LabelInfo样式" Margin="10" 
    Style="{DynamicResource LabelInfo}"></Label>
```

![Label.InfoStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.InfoStyle.png)

# LabelWarning

Label标签 警告类型样式

案例：

```
    <Label Content="LabelWarning样式" Margin="10" 
    Style="{DynamicResource LabelWarning}"></Label>
```

![Label.WarningStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.WarningStyle.png)

# LabelDanger

Label标签 危险类型样式

```
    <Label Content="LabelDanger样式" Margin="10" 
    Style="{DynamicResource LabelDanger}"></Label>
```

![Label.DangerStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Label.DangerStyle.png)


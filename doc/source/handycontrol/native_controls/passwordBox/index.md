---
title: PasswordBox 密码框
---

# PasswordBoxBaseStyle

控件默认样式，该样式不建议直接使用，而是选择继承的方式进行个人控件样式的处理，参考在线资源[PasswordBoxBaseStyle](https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Themes/Styles/Base/PasswordBoxBaseStyle.xaml)

```
<Style BasedOn="{StaticResource PasswordBoxBaseStyle}" TargetType="PasswordBox"/>
```

使用

```
    <PasswordBox PasswordChar="*"
                 VerticalAlignment="Center"
                 Width="120"></PasswordBox>
```

效果

![PasswordBox.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/PasswordBox.BaseStyle.png)

# PasswordBoxExtendBaseStyle

控件拓展基础样式，是样式`PasswordBoxExtend`的基本样式，不建议直接使用，而是选择使用`PasswordBoxExtend`，该样式支持添加`PasswordBox`专用附加属性`PasswordBoxAttach`、额外信息元素`InfoElement`和标题元素`TitleElement`

```
    <!--为使普通密码输入文本框显示水印,需要设定PasswordBoxAttach.PasswordLength="0"-->
    <PasswordBox Style="{DynamicResource PasswordBoxExtend}" PasswordChar="*" 
                 hc:PasswordBoxAttach.PasswordLength="0"
                 hc:InfoElement.Placeholder="请输入密码" 
                 VerticalAlignment="Center"
                 Width="120"></PasswordBox>
    <PasswordBox Style="{DynamicResource PasswordBoxExtend}" PasswordChar="*" 
                 hc:TitleElement.Title="用户密码："
                 hc:TitleElement.TitleAlignment="Top"
                 VerticalAlignment="Center"
                 Width="120"></PasswordBox>
```

效果

![PasswordBox.ExtendStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/PasswordBox.ExtendStyle.png)


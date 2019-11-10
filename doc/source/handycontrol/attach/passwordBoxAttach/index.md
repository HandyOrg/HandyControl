---
title: PasswordBoxAttach密码框专用
---

# 属性

该部分附加属性，仅仅适用于`HandyControl`自定义控件`hc:PasswordBox`以及部分`HandyControl`自带的`PasswordBox`的样式

| 名称           | 用途     |
| -------------- | -------- |
| IsMonitoring   | 是否监测 |
| PasswordLength | 密码长度 |

#  案例

对应`xaml`中添加`HandyControl`对应的命名空`xmlns:hc="https://handyorg.github.io/handycontrol"`

## IsMonitoring 是否监测

设定是否启用监听该`PasswordBox`控件，用于进行密码长度的判定和其他逻辑处理，单独使用无明显可视化效果

```xml
    <hc:PasswordBox hc:PasswordBoxAttach.IsMonitoring="True"
                 Width="100"
                 Height="Auto"></hc:PasswordBox>
```



## PasswordLength 密码长度

当密码长度为0时，显示输入提示信息，录入密码，水印消失

```xml
    <hc:PasswordBox hc:PasswordBoxAttach.IsMonitoring="True"
                 hc:PasswordBoxAttach.PasswordLength="10"
                 Width="100"
                 Height="Auto"
                 VerticalAlignment="Center"
                 hc:InfoElement.Placeholder="请输入密码"></hc:PasswordBox>
```

效果：

![PasswordAttach.Length](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/PasswordAttach.Length.png)


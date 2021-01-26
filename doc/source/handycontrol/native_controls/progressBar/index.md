---
title: ProgressBar 进度条
---

# ProgressBarBaseStyle

进度条默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

# 其他样式

`HandyControl`中自带的其他样式和效果，其中包含如下样式：

| 样式Key            | 用途 |父样式|
| ------------------ | ---- ||
| ProgressBarSuccess | 成功色进度条 |ProgressBarBaseStyle|
| ProgressBarInfo | 提示色进度条 |ProgressBarBaseStyle|
| ProgressBarWarning | 警告色进度条 |ProgressBarBaseStyle|
| ProgressBarDanger | 危险色进度条 |ProgressBarBaseStyle|
| ProgressBarStripeBaseStyle | 条纹进度条默认样式（不推荐直接使用） |-|
| ProgressBarPrimaryStripe | 主题色条纹进度条 |ProgressBarStripeBaseStyle|
| ProgressBarSuccessStripe | 成功色条纹进度条 |ProgressBarStripeBaseStyle|
| ProgressBarInfoStripe | 信息色条纹进度条 |ProgressBarStripeBaseStyle|
| ProgressBarWarningStripe | 警告色条纹进度条 |ProgressBarStripeBaseStyle|
| ProgressBarDangerStripe | 危险色条纹进度条 |ProgressBarStripeBaseStyle|
| ProgressBarFlat | 扁平风格 |-|

案例：
```xml
<StackPanel Margin="20">
        <TextBlock Text="默认样式"></TextBlock>
        <ProgressBar Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarSuccess"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarSuccess}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarInfo"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarInfo}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarWarning"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarWarning}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarDanger"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarDanger}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarPrimaryStripe"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarPrimaryStripe}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarSuccessStripe"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarSuccessStripe}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarInfoStripe"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarInfoStripe}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarWarningStripe"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarWarningStripe}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarDangerStripe"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarDangerStripe}" Value="40"></ProgressBar>
    </StackPanel>
    <StackPanel Margin="20">
        <TextBlock Text="ProgressBarFlat"></TextBlock>
        <ProgressBar Style="{DynamicResource ProgressBarFlat}" Value="40"></ProgressBar>
    </StackPanel>
```
效果：
![ProgressBar.Styles](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ProgressBar.Styles.png)

# 温馨提示

对于颜色、圆角或其它自定义需求，可参考[进度条样式源码](https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Themes/Styles/ProgressBar.xaml)自行定义。
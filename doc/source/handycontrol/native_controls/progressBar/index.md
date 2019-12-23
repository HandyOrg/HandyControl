---
title: ProgressBar 进度条
---

# ProgressBarBaseStyle

`HandyControl`中自带的默认样式，不建议直接使用而是选择继承的方式，案例如下：

```
<Style BasedOn="{StaticResource ProgressBarBaseStyle}" TargetType="ProgressBar">
        <Setter Property="Foreground" Value="{DynamicResource PrimaryBrush}"/>
</Style>
```

# 其他样式

`HandyControl`中自带的其他样式和效果，其中包含如下样式：

| 样式Key            | 用途 |
| ------------------ | ---- |
| ProgressBarSuccess | 成功色进度条 |
| ProgressBarInfo | 提示色进度条 |
| ProgressBarWarning | 警告色进度条 |
| ProgressBarDanger | 危险色进度条 |
| ProgressBarPrimaryStripe | 主题色条纹进度条 |
| ProgressBarSuccessStripe | 成功色条纹进度条 |
| ProgressBarInfoStripe | 信息色条纹进度条 |
| ProgressBarWarningStripe | 警告色条纹进度条 |
| ProgressBarDangerStripe | 危险色条纹进度条 |
| ProgressBarFlat | 扁平风格 |

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

对于进度条进度颜色、圆角以及其他样式的个人使用需求，可查看`HandyControl`开源目中的[源码样式部分](https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Themes/Styles/ProgressBar.xaml)自行定义


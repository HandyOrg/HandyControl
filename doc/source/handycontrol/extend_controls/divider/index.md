---
title: Divider 分割线
---

区隔内容的分割线。

```cs
public class Divider : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Content|内容|||
|Orientation|方向|||
|ContentTemplate|内容模板|||
|ContentStringFormat|内容字符串格式|||
|ContentTemplateSelector|内容模板|||
|LineStroke|分割线颜色|||
|LineStrokeThickness|分割线粗细|1||
|LineStrokeDashArray|分割线间隙||||

# 案例

```xml
<WrapPanel Margin="16">
    <StackPanel Margin="16" Width="300">
        <hc:Divider/>
        <hc:Divider Content="{ex:Lang Key={x:Static langs:LangKeys.Title}}"/>
        <hc:Divider Content="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Padding="10,0"/>
        <hc:Divider LineStrokeThickness="2" LineStroke="{DynamicResource DarkPrimaryBrush}"/>
        <hc:Divider LineStrokeDashArray="2,2"/>
    </StackPanel>
    <StackPanel Margin="16" Width="300">
        <hc:Divider Content="{ex:Lang Key={x:Static langs:LangKeys.Title}}" HorizontalContentAlignment="Left"/>
        <hc:Divider Content="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Padding="10,0"  HorizontalContentAlignment="Right"/>
        <StackPanel Orientation="Horizontal">
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
        </StackPanel>
        <StackPanel Orientation="Horizontal" Margin="0,16,0,0">
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider LineStrokeThickness="2" Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider LineStrokeThickness="2" Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
        </StackPanel>
        <StackPanel Orientation="Horizontal" Margin="0,16,0,0">
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider LineStrokeThickness="2" LineStroke="{DynamicResource DarkPrimaryBrush}" Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
            <hc:Divider LineStrokeThickness="2" LineStroke="{DynamicResource DarkPrimaryBrush}" Orientation="Vertical" MaxHeight="16"/>
            <Button Content="{ex:Lang Key={x:Static langs:LangKeys.Button}}"/>
        </StackPanel>
    </StackPanel>
</WrapPanel>
```

![Divider](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Divider.png)
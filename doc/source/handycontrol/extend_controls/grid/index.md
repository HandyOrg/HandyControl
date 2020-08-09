---
title: Grid 栅格
---

栅格系统的wpf实现，web版本可参考 [element/layout](https://element.eleme.cn/#/zh-CN/component/layout)

```cs
public class Row : Panel
```

```cs
public class Col : ContentControl
```

# 属性

## Row 行

|属性|描述|默认值|备注|
|-|-|-|-|
|Gutter|栅格间隔|0|||

## Col 列

|属性|描述|默认值|备注|
|-|-|-|-|
|Layout|布局方式|||
|Offset|栅格左侧的间隔格数|0||
|Span|栅格占据的列数|24||
|IsFixed|该列是否固定|false|||

# 案例

```xml
<StackPanel Margin="32">
    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.BasicLayout}}" Style="{StaticResource TextBlockLargeBold}" HorizontalAlignment="Left"/>
    <hc:Row Margin="0,20,0,0">
        <hc:Col Span="24">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0">
        <hc:Col Span="12">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="12">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0">
        <hc:Col Span="8">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="8">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="8">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0">
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0">
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ColumnSpacing}}" Margin="0,32,0,0" Style="{StaticResource TextBlockLargeBold}" HorizontalAlignment="Left"/>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.HybridLayout}}" Margin="0,32,0,0" Style="{StaticResource TextBlockLargeBold}" HorizontalAlignment="Left"/>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="16">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="8">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="8">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="8">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="16">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="4">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ColumnOffset}}" Margin="0,32,0,0" Style="{StaticResource TextBlockLargeBold}" HorizontalAlignment="Left"/>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6" Offset="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="6" Offset="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Span="6" Offset="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <hc:Row Margin="0,20,0,0" Gutter="20">
        <hc:Col Span="12" Offset="6">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ResponsiveLayout}}" Margin="0,32,0,0" Style="{StaticResource TextBlockLargeBold}" HorizontalAlignment="Left"/>
    <hc:Row Margin="0,20,0,0" Gutter="10">
        <hc:Col Layout="8,6,4,3,1">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Layout="4 6 8 9 11">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Layout="{extension:ColLayout Xs=4, Sm=6, Md=8, Lg=9, Xl=11}">
            <Border Background="{DynamicResource BorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
        <hc:Col Layout="{extension:ColLayout Xs=8, Sm=6, Md=4, Lg=3, Xl=1}">
            <Border Background="{DynamicResource SecondaryBorderBrush}" Height="36" CornerRadius="4"/>
        </hc:Col>
    </hc:Row>
</StackPanel>
```

![Grid](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Grid.gif)

{% note info %}
xaml中 `Layout` 写法的3种方式：
Layout="{extension:ColLayout Xs=4, Sm=6, Md=8, Lg=9, Xl=11}"
Layout="4,6,8,9,11"
Layout="4 6 8 9 11"
{% endnote %}
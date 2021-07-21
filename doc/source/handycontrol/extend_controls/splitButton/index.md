---
title: SplitButton 分割按钮
---

在普通按钮的基础上提供额外的点击项.

```cs
public class SplitButton : ButtonBase
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|HitMode|下拉框触发模式|HitMode.Click||
|MaxDropDownHeight|下拉框最大高度|||
|IsDropDownOpen|下拉框是否打开|false||
|DropDownContent|下拉框内容||||

# 案例

```xml
<StackPanel Margin="32" VerticalAlignment="Center">
    <hc:SplitButton Margin="0,0,0,10" Content="{ex:Lang Key={x:Static langs:LangKeys.Default}}" Command="{Binding SelectCmd}" CommandParameter="Command0" HorizontalAlignment="Stretch">
        <hc:SplitButton.DropDownContent>
            <StackPanel>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command1"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command2"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command3"/>
            </StackPanel>
        </hc:SplitButton.DropDownContent>
    </hc:SplitButton>
    <hc:SplitButton Style="{StaticResource SplitButtonPrimary}" Margin="0,0,0,10" Content="{ex:Lang Key={x:Static langs:LangKeys.Primary}}" Command="{Binding SelectCmd}" CommandParameter="Command0" HorizontalAlignment="Stretch">
        <hc:SplitButton.DropDownContent>
            <StackPanel>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command1"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command2"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command3"/>
            </StackPanel>
        </hc:SplitButton.DropDownContent>
    </hc:SplitButton>
    <hc:SplitButton Style="{StaticResource SplitButtonWarning}" Margin="0,0,0,10" Content="{ex:Lang Key={x:Static langs:LangKeys.Warning}}" HitMode="Hover" HorizontalAlignment="Stretch">
        <hc:SplitButton.DropDownContent>
            <StackPanel>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command1"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command2"/>
                <MenuItem Header="{ex:Lang Key={x:Static langs:LangKeys.Title}}" Command="{Binding SelectCmd}" CommandParameter="Command3"/>
            </StackPanel>
        </hc:SplitButton.DropDownContent>
    </hc:SplitButton>
</StackPanel>
```

![SplitButton](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/SplitButton.gif)
---
title: SearchBar 搜索栏
---

通过键入关键字触发搜索.

```cs
public class SearchBar : TextBox, ICommandSource
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|IsRealTime|是否实时搜索|false|如果为`true`则每次输入都会自动触发搜索开始事件|
|Command|命令|||
|CommandParameter|命令参数|||
|CommandTarget|命令目标元素||||

# 附加属性

|属性|描述|默认值|备注|
|-|-|-|-|
|InfoElement.Placeholder|占位符|||
|InfoElement.Necessary|是否必填|false||
|InfoElement.Symbol|必填标记|●||
|InfoElement.ContentHeight|内容高度|30|标题在上时可用|
|InfoElement.MinContentHeight|最小内容高度|30|标题在上时可用|
|TitleElement.Title|标题|||
|TitleElement.TitlePlacement|标题对齐方式|TitlePlacementType.Top||
|TitleElement.TitleWidth|标题宽度|120|||

# 事件

|名称|说明|
|-|-|
| SearchStarted | 搜索开始时触发 |

# 样式

|样式|描述|
|-|-|
|SearchBarBaseStyle|默认样式|
|SearchBarExtendBaseStyle|默认扩展样式|
|SearchBarPlusBaseStyle|默认加强样式|
|SearchBarExtend|扩展样式|
|SearchBarPlus|加强样式|

# 案例

```xml
<StackPanel Margin="32" Orientation="Horizontal">
    <StackPanel>
        <hc:SearchBar Command="{Binding SearchCmd}" CommandParameter="{Binding Text,RelativeSource={RelativeSource Self}}"/>
        <hc:SearchBar Margin="0,32,0,0" IsEnabled="False"/>

        <hc:SearchBar hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" Style="{StaticResource SearchBarExtend}"/>
        <hc:SearchBar hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" hc:InfoElement.Necessary="True" Style="{StaticResource SearchBarExtend}"/>
        <hc:SearchBar Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource SearchBarExtend}" Margin="0,32,0,0"/>
        <hc:SearchBar Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource SearchBarExtend}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>
    </StackPanel>
    <StackPanel Margin="32,0,0,0">
        <hc:SearchBar Name="SearchBarCustomVerify" ShowClearButton="True" Style="{StaticResource SearchBarPlus}"/>
        <hc:SearchBar Margin="0,32,0,0" IsEnabled="False" Style="{StaticResource SearchBarPlus}"/>

        <hc:SearchBar ShowClearButton="True" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" Style="{StaticResource SearchBarPlus}"/>
        <hc:SearchBar TextType="Mail" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterEmail}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" hc:InfoElement.Necessary="True" Style="{StaticResource SearchBarPlus}"/>
        <hc:SearchBar Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource SearchBarPlus}" Margin="0,32,0,0"/>
        <hc:SearchBar ShowClearButton="True" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource SearchBarPlus}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>
    </StackPanel>
</StackPanel>
```

![SearchBar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/SearchBar.png)
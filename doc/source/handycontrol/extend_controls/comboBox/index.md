---
title: ComboBox 组合框
---

原生组合框的HC扩展。

```cs
public class ComboBox : System.Windows.Controls.ComboBox, IDataInput
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|VerifyFunc|数据验证委托|||
|IsError|数据是否错误|false||
|ErrorStr|错误提示|||
|TextType|文本类型|||
|ShowClearButton|是否显示清除按钮|false||
|AutoComplete|是否自动完成输入|false|||

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
|TitleElement.TitleWidth|标题宽度|120||
|DropDownElement.ConsistentWidth|下拉内容是否和下拉框宽度一致|false|||

# 方法

|方法|描述|备注|
|-|-|-|
|VerifyData|验证数据|可手动触发验证|

# 样式

|样式|描述|
|-|-|
|ComboBoxPlusBaseStyle|默认样式|

# 案例

原生功能在扩展ComboBox上行为一致。

```xml
<StackPanel Margin="32">
    <hc:ComboBox ShowClearButton="True" ItemsSource="{Binding DataList}" SelectedIndex="0"/>
    <hc:ComboBox ItemsSource="{Binding DataList}" Margin="0,16,0,0" SelectedIndex="0" IsEnabled="False"/>
    <hc:ComboBox ShowClearButton="True" ItemsSource="{Binding DataList}" SelectedIndex="0" Margin="0,16,0,0" IsEditable="True"/>

    <hc:ComboBox ItemsSource="{Binding DataList}" SelectedIndex="0" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}"/>
    <hc:ComboBox ShowClearButton="True" ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr2}}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>
    <hc:ComboBox ItemsSource="{Binding DataList}" IsEditable="True" SelectedIndex="0" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}"/>
    <hc:ComboBox AutoComplete="True" ShowClearButton="True" ItemsSource="{Binding DataList}" IsEditable="True" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr2}}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>

    <hc:ComboBox ItemsSource="{Binding DataList}" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Margin="0,32,0,0" Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}"/>
    <hc:ComboBox ShowClearButton="True" ItemsSource="{Binding DataList}" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>
    <hc:ComboBox ItemsSource="{Binding DataList}" IsEditable="True" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Margin="0,32,0,0" Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}"/>
    <hc:ComboBox AutoComplete="True" ShowClearButton="True" ItemsSource="{Binding DataList}" IsEditable="True" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" hc:InfoElement.Necessary="True" Margin="0,32,0,0"/>
</StackPanel>
```

![ComboBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/ComboBox_1.png)
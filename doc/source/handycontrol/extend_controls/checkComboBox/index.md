---
title: CheckComboBox 复选组合框
---

可选选中多个项目的组合框.

```cs
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
[TemplatePart(Name = ElementSelectAll, Type = typeof(CheckComboBoxItem))]
public class CheckComboBox : ListBox, IDataInput
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|VerifyFunc|数据验证委托|||
|IsError|数据是否错误|false||
|ErrorStr|错误提示|||
|TextType|文本类型|||
|ShowClearButton|是否显示清除按钮|false||
|MaxDropDownHeight|下拉内容最大高度|||
|IsDropDownOpen|下拉框是否展开|false||
|TagStyle|选中标记样式|||
|ShowSelectAllButton|是否显示选中所有按钮|false|||

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
|CheckComboBoxBaseStyle|默认样式|
|CheckComboBoxExtendBaseStyle|默认扩展样式|
|CheckComboBoxPlusBaseStyle|默认增强样式|
|CheckComboBoxExtend|扩展样式|
|CheckComboBoxPlus|增强样式|

# 案例

```xml
<StackPanel Margin="32" Orientation="Horizontal">
    <StackPanel>
        <hc:CheckComboBox ShowClearButton="True" MaxWidth="380" ItemsSource="{Binding DataList}" ShowSelectAllButton="True"/>
        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" Style="{StaticResource CheckComboBoxExtend}" Margin="0,16,0,0"  IsEnabled="False" ShowSelectAllButton="True"/>

        <hc:CheckComboBox ShowClearButton="True" MaxWidth="380" ItemsSource="{Binding DataList}" Style="{StaticResource CheckComboBoxExtend}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" ShowSelectAllButton="True"/>
        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr2}}" Style="{StaticResource CheckComboBoxExtend}" hc:InfoElement.Necessary="True" Margin="0,16,0,0"/>

        <hc:CheckComboBox ShowClearButton="True" Width="380" ItemsSource="{Binding DataList}" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" Style="{StaticResource CheckComboBoxExtend}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Margin="0,32,0,0" ShowSelectAllButton="True"/>
        <hc:CheckComboBox Width="380" ItemsSource="{Binding DataList}" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource CheckComboBoxExtend}" hc:InfoElement.Necessary="True" Margin="0,16,0,0"/>
    </StackPanel>
    <StackPanel Margin="32,0,0,0">
        <hc:CheckComboBox ShowClearButton="True" MaxWidth="380" ItemsSource="{Binding DataList}" Style="{StaticResource CheckComboBoxPlus}" ShowSelectAllButton="True"/>
        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" Margin="0,16,0,0" IsEnabled="False" Style="{StaticResource CheckComboBoxPlus}"/>

        <hc:CheckComboBox MaxWidth="380" ShowClearButton="True" ItemsSource="{Binding DataList}" Style="{StaticResource CheckComboBoxPlus}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr1}}" Margin="0,32,0,0" ShowSelectAllButton="True"/>
        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr2}}" Style="{StaticResource CheckComboBoxPlus}" hc:InfoElement.Necessary="True" Margin="0,16,0,0"/>

        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" ShowClearButton="True" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" Style="{StaticResource CheckComboBoxPlus}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Margin="0,32,0,0" ShowSelectAllButton="True"/>
        <hc:CheckComboBox MaxWidth="380" ItemsSource="{Binding DataList}" Width="380" hc:InfoElement.TitleWidth="140" hc:InfoElement.TitlePlacement="Left" hc:InfoElement.Placeholder="{ex:Lang Key={x:Static langs:LangKeys.PlsEnterContent}}" hc:InfoElement.Title="{ex:Lang Key={x:Static langs:LangKeys.TitleDemoStr3}}" Style="{StaticResource CheckComboBoxPlus}" hc:InfoElement.Necessary="True" Margin="0,16,0,0"/>
    </StackPanel>
</StackPanel>
```

![CheckComboBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CheckComboBox.png)
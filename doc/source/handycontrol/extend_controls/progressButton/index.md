---
title: ProgressButton 进度按钮
---

在切换按钮的基础上附带了进度显示.

```cs
public class ProgressButton : ToggleButton
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ProgressStyle|进度条样式|||
|Progress|进度|0|||

# 案例

```xml
<StackPanel>
    <hc:ProgressButton IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
    <hc:ProgressButton Style="{StaticResource ProgressButtonPrimary}" IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
    <hc:ProgressButton Style="{StaticResource ProgressButtonSuccess}" IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
    <hc:ProgressButton Style="{StaticResource ProgressButtonInfo}" IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
    <hc:ProgressButton Style="{StaticResource ProgressButtonWarning}" IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
    <hc:ProgressButton Style="{StaticResource ProgressButtonDanger}" IsChecked="{Binding IsUploading,Mode=TwoWay}" Progress="{Binding Progress}" Content="{Binding IsChecked,RelativeSource={RelativeSource Self},Converter={StaticResource Boolean2StringConverter},ConverterParameter={x:Static langs:Lang.UploadStr}}" Width="180" Margin="5"/>
</StackPanel>
```

![ProgressButton](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ProgressButton.gif)
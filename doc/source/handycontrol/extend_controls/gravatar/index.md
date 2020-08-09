---
title: Gravatar 头像
---

一种用户随机头像的实现方式.

```cs
public class Gravatar : ContentControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Generator|头像生成器|GithubGravatarGenerator||
|Id|用户Id|||
|Source|用户头像||||

# 样式

|样式|描述|
|-|-|
|GravatarBaseStyle|默认样式|
|GravatarCircle|圆形头像|
|GravatarCircleImg|圆形图片头像|

# 案例

```xml
<StackPanel Margin="32" Orientation="Horizontal">
    <UniformGrid Rows="3" Columns="3" Width="240" Height="240">
        <hc:Gravatar Id="User1"/>
        <hc:Gravatar Id="User2"/>
        <hc:Gravatar Style="{StaticResource GravatarCircleImg}">
            <Image Source="/HandyControlDemo;component/Resources/Img/Album/2.jpg"/>
        </hc:Gravatar>
        <hc:Gravatar Id="User4"/>
        <hc:Gravatar Id="User5" Style="{StaticResource GravatarCircle}"/>
        <hc:Gravatar Id="User6"/>
        <hc:Gravatar Style="{StaticResource GravatarCircle}" Source="/HandyControlDemo;component/Resources/Img/Album/1.jpg"/>
        <hc:Gravatar Id="User8"/>
        <hc:Gravatar Id="User9"/>
    </UniformGrid>
    <StackPanel Margin="16,0,0,0" Height="220" VerticalAlignment="Center">
        <TextBox Text="User1" Name="TextBoxName" Width="180"/>
        <hc:Gravatar Height="180" Width="180" Id="{Binding Text,ElementName=TextBoxName}" Margin="10"/>
    </StackPanel>
</StackPanel>
```

![Gravatar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Gravatar.gif)
---
title: Carousel 轮播
---

可以在固定时间内循环切换项目。

```cs
public class Carousel : SimpleItemsControl, IDisposable
```

# 属性

| 属性                   | 用途                           |
| ---------------------- | ------------------------------|
| AutoRun                | 是否自动轮播                   |
| Interval               | 轮播间隔时间                   |
| ExtendWidth            | 项内容扩展宽度                 |
| IsCenter               | 轮播项是否居中显示              |
| PageButtonStyle        | 页按钮样式                     |

# 案例

## 显示填充轮播项

```xml
<hc:Carousel Margin="32" IsCenter="True" AutoRun="True" Width="600" Height="330" VerticalAlignment="Center">
    <Image Width="600" Stretch="UniformToFill" Source="/HandyControlDemo;component/Resources/Img/1.jpg"/>
    <Image Width="300" Stretch="UniformToFill" Source="/HandyControlDemo;component/Resources/Img/2.jpg"/>
    <hc:SimplePanel Width="600">
        <Image Stretch="UniformToFill" Source="/HandyControlDemo;component/Resources/Img/3.jpg"/>
        <TextBlock Text="Demo Text" Style="{StaticResource TextBlockDefault}" FontSize="100" FontWeight="Bold" Foreground="White"/>
    </hc:SimplePanel>
    <Image Width="600" Stretch="UniformToFill" Source="/HandyControlDemo;component/Resources/Img/4.jpg"/>
    <Image Width="600" Stretch="UniformToFill" Source="/HandyControlDemo;component/Resources/Img/5.jpg"/>
</hc:Carousel>
```

![Carousel](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Carousel.gif)

## 使用绑定生成轮播项

```xml
<hc:Carousel AutoRun="True" ItemsSource="{Binding Images}" Width="300" Height="200">
    <hc:Carousel.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding}" Width="300"/>
        </DataTemplate>
    </hc:Carousel.ItemTemplate>
</hc:Carousel>
```

{% note info %}
`ExtendWidth`会在最后一个轮播项后扩展一段距离，请注意，这不是偏移的概念，而是扩展的概念。
{% endnote %}
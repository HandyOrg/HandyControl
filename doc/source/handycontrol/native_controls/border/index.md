---
title: Border 边框
---

# BorderRegionStyle

此样式用于包裹一个控件区域，配合基础xaml定义中的几个DropShadowEffect资源可制作如下效果：

![Border](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Border.png)

以上效果的xaml代码如下：

``` xml
        <UniformGrid Margin="32,32,0,0" Columns="3" Rows="2">
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="0,0,32,32">
                <Border Background="{DynamicResource PrimaryBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="0,0,32,32" Effect="{StaticResource EffectShadow1}">
                <Border Background="{DynamicResource InfoBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="0,0,32,32" Effect="{StaticResource EffectShadow2}">
                <Border Background="{DynamicResource WarningBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow3}">
                <Border Background="{DynamicResource DangerBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow4}">
                <Border Background="{DynamicResource AccentBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegion}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow5}">
                <Border Background="{DynamicResource BorderBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                </Border>
            </Border>
        </UniformGrid>
```

提示：新版本中使用的`BorderRegion`，有别于老版本使用样式`BorderRegionStyle`，由直角变更为圆角

# BorderVerticallySplitter

此样式使用1单位宽度的Border来分割横向排列的控件。

{% note info no-icon %}
用例：

{% code %}
<StackPanel Height="20" Orientation="Horizontal">
    <Ellipse Margin="5,0" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderVerticallySplitter}"/>
    <Ellipse Margin="5,0" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderVerticallySplitter}"/>
    <Ellipse Margin="5,0" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderVerticallySplitter}"/>
    <Ellipse Margin="5,0" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderVerticallySplitter}"/>
    <Ellipse Margin="5,0" Fill="OrangeRed" Width="20" Height="20"/>
</StackPanel>
{% endcode %}
![BorderVerticallySplitter](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/BorderVerticallySplitter_1.png)
{% endnote %}

# BorderHorizontallySplitter

此样式使用1单位高度的Border来分割纵向排列的控件。

{% note info no-icon %}
用例：

{% code %}
<StackPanel Width="20">
    <Ellipse Margin="0,5" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderHorizontallySplitter}"/>
    <Ellipse Margin="0,5" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderHorizontallySplitter}"/>
    <Ellipse Margin="0,5" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderHorizontallySplitter}"/>
    <Ellipse Margin="0,5" Fill="OrangeRed" Width="20" Height="20"/>
    <Border Style="{StaticResource BorderHorizontallySplitter}"/>
    <Ellipse Margin="0,5" Fill="OrangeRed" Width="20" Height="20"/>
</StackPanel>
{% endcode %}
![BorderHorizontallySplitter](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/BorderHorizontallySplitter_1.png)
{% endnote %}

# BorderCircular

此样式借助`BorderElement.Circular`附加属性可实现圆形Border。

{% note info no-icon %}
用例：

{% code %}
<Border Style="{StaticResource BorderCircular}" Background="OrangeRed" Width="100" Height="100"/>
{% endcode %}
![BorderCircular](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/BorderCircular_1.png)
{% endnote %}

# BorderClip

此样式在`BorderCircular`基础上添加了裁剪功能，常用于显示圆形Image。

{% note info no-icon %}
用例：

{% code %}
<Border Style="{StaticResource BorderClip}" Width="100" Height="100">
    <Image Source="/HandyControlDemo;component/Resources/Img/Album/2.jpg"/>
</Border>
{% endcode %}
![BorderClip](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/BorderClip_1.png)
{% endnote %}
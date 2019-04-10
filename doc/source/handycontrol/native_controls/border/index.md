---
title: Border 边框
---

# BorderRegionStyle

此样式用于包裹一个控件区域，配合基础xaml定义中的几个DropShadowEffect资源可制作如下效果：

![Border](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Border.png)

以上效果的xaml代码如下：

``` xml
        <UniformGrid Margin="32,32,0,0" Columns="3" Rows="2">
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200" Margin="0,0,32,32">
                <Border Background="{DynamicResource PrimaryBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200" Margin="0,0,32,32" Effect="{StaticResource EffectShadow1}">
                <Border Background="{DynamicResource InfoBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200" Margin="0,0,32,32" Effect="{StaticResource EffectShadow2}">
                <Border Background="{DynamicResource WarningBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow3}">
                <Border Background="{DynamicResource DangerBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow4}">
                <Border Background="{DynamicResource AccentBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </Border>
            <Border Style="{StaticResource BorderRegionStyle}" Width="200" Height="200"  Margin="0,0,32,32" Effect="{StaticResource EffectShadow5}">
                <Border Background="{DynamicResource BorderBrush}">
                    <TextBlock Text="{x:Static langs:Lang.ContentDemoStr}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                </Border>
            </Border>
        </UniformGrid>
```

# BorderVerticallySplitter

此样式使用1单位宽度的Border来分割横向排列的控件。

# BorderHorizontallySplitter

此样式使用1单位高度的Border来分割纵向排列的控件。

# BorderCircular

此样式借助`BorderElement.Circular`附加属性可实现圆形Border。

# BorderClip

此样式在`BorderCircular`基础上添加了裁剪功能，常用于显示圆形Image。
---
title: ScrollViewer 滚动视图
---

对原生滚动视图的扩展.

```cs
public class ScrollViewer : System.Windows.Controls.ScrollViewer
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Orientation|滚动方向|Orientation.Vertical||
|CanMouseWheel|是否响应鼠标滚轮操作|true||
|IsInertiaEnabled|是否支持惯性滚动|false||
|IsPenetrating|是否可以穿透点击|false|||

# 案例

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <Border BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
        <hc:ScrollViewer Orientation="Horizontal" HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Hidden" IsInertiaEnabled="True">
            <StackPanel Orientation="Horizontal">
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
            </StackPanel>
        </hc:ScrollViewer>
    </Border>
    <Border Grid.Row="1" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}" Margin="0,16,0,0">
        <hc:ScrollViewer IsInertiaEnabled="True">
            <WrapPanel Orientation="Horizontal">
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
                <Border Style="{StaticResource BorderRegion}" Width="200" Height="200" Margin="10,10,32,10">
                    <Border Background="{DynamicResource PrimaryBrush}">
                        <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                    </Border>
                </Border>
            </WrapPanel>
        </hc:ScrollViewer>
    </Border>
</Grid>
```

![ScrollViewer](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/ScrollViewer.gif)
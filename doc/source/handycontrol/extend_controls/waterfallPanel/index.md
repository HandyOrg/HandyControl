---
title: WaterfallPanel 瀑布流
---

该面板可使元素按照等宽不等高或者等高不等宽进行布局.

```cs
public class WaterfallPanel : Panel
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Groups|组数|2|等宽不等高时即为列数，等高不等宽即为行数|
|Orientation|排列方向|Orientation.Horizontal|Horizontal为等宽不等高，Vertical为等高不等宽||

# 案例

```xml
<Grid Margin="32">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <hc:NumericUpDown x:Name="UpDownGroups" HorizontalAlignment="Center" Value="2" Minimum="1" Maximum="3" Width="200" Style="{StaticResource NumericUpDownExtend}" hc:TitleElement.Title="{ex:Lang Key={x:Static langs:LangKeys.Groups}}" hc:TitleElement.TitleWidth="50" hc:TitleElement.TitlePlacement="Left"/>
    <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="0,16,0,0">
        <hc:ScrollViewer MaxHeight="300">
            <hc:WaterfallPanel VerticalAlignment="Center" Width="300" Groups="{Binding Value,ElementName=UpDownGroups}" hc:PanelElement.FluidMoveBehavior="{StaticResource BehaviorXY200}">
                <Border Height="100" Background="{DynamicResource PrimaryBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Height="140" Background="{DynamicResource DangerBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Height="100" Background="{DynamicResource SuccessBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Height="170" Background="{DynamicResource InfoBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Height="100" Background="{DynamicResource WarningBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </hc:WaterfallPanel>
        </hc:ScrollViewer>
        <hc:ScrollViewer Margin="32,0,0,0" HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Hidden" Orientation="Horizontal" MaxWidth="300">
            <hc:WaterfallPanel VerticalAlignment="Center" Height="300" Orientation="Vertical" Groups="{Binding Value,ElementName=UpDownGroups}" hc:PanelElement.FluidMoveBehavior="{StaticResource BehaviorXY200}">
                <Border Width="100" Background="{DynamicResource PrimaryBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Width="140" Background="{DynamicResource DangerBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Width="100" Background="{DynamicResource SuccessBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Width="170" Background="{DynamicResource InfoBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
                <Border Width="100" Background="{DynamicResource WarningBrush}" Effect="{StaticResource EffectShadow1}" Margin="5">
                    <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
                </Border>
            </hc:WaterfallPanel>
        </hc:ScrollViewer>
    </StackPanel>
</Grid>
```

![WaterfallPanel](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/WaterfallPanel.png)
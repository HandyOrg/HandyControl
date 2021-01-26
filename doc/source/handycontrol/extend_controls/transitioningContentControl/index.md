---
title: TransitioningContentControl 内容过渡控件
---

主要用于变化内容的过渡呈现效果.

```cs
public class TransitioningContentControl : ContentControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|TransitionMode|过渡模式|TransitionMode.Right2Left||
|TransitionStoryboard|过渡动画||||

# 案例

```xml
<Storyboard x:Key="Custom1Transition" x:Shared="False">
    <DoubleAnimation From="50" To="0" Duration="0:0:0.4" Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)">
        <DoubleAnimation.EasingFunction>
            <ElasticEase Oscillations="1"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>

<Storyboard x:Key="Custom2Transition" x:Shared="False">
    <DoubleAnimation From="10" To="0" Duration="0:0:0.4" Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)">
        <DoubleAnimation.EasingFunction>
            <ElasticEase Oscillations="1"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>

<Storyboard x:Key="Custom3Transition" x:Shared="False">
    <DoubleAnimation From=".8" To="1" Duration="0:0:0.4" Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)">
        <DoubleAnimation.EasingFunction>
            <ElasticEase Oscillations="1"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

```xml
<Grid Margin="32">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition/>
    </Grid.RowDefinitions>
    <ToggleButton Margin="0,0,0,32" IsChecked="True" Name="ButtonVisibilitySwitch" Style="{StaticResource ToggleButtonSwitch}"/>
    <UniformGrid Visibility="{Binding IsChecked,ElementName=ButtonVisibilitySwitch,Converter={StaticResource Boolean2VisibilityConverter}}" Grid.Row="1" Rows="4" Columns="3">
        <hc:TransitioningContentControl>
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Right2Left}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Left2Right">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Left2Right}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Bottom2Top">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Bottom2Top}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Top2Bottom">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Top2Bottom}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Right2LeftWithFade">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Right2LeftWithFade}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Left2RightWithFade">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Left2RightWithFade}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Bottom2TopWithFade">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Bottom2TopWithFade}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Top2BottomWithFade">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Top2BottomWithFade}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionMode="Right2LeftWithFade">
            <Label HorizontalAlignment="Stretch" Content="{x:Static hc:TransitionMode.Right2LeftWithFade}" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionStoryboard="{StaticResource Custom1Transition}">
            <Label HorizontalAlignment="Stretch" Content="Custom1" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionStoryboard="{StaticResource Custom2Transition}">
            <Label HorizontalAlignment="Stretch" Content="Custom2" Margin="32"/>
        </hc:TransitioningContentControl>
        <hc:TransitioningContentControl TransitionStoryboard="{StaticResource Custom3Transition}">
            <Label HorizontalAlignment="Stretch" Content="Custom3" Margin="32"/>
        </hc:TransitioningContentControl>
    </UniformGrid>
</Grid>
```

![TransitioningContentControl](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/TransitioningContentControl.gif)
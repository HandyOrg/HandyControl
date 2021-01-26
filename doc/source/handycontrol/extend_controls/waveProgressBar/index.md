---
title: WaveProgressBar 波形进度条
---

进度条的一种特殊效果，可用于增强用户体验.

```cs
[TemplatePart(Name = ElementWave, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementClip, Type = typeof(FrameworkElement))]
public class WaveProgressBar : RangeBase
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Text|进度文本|||
|ShowText|是否显示进度文本|true||
|WaveFill|波浪画刷|||
|WaveThickness|波浪边框粗细|0||
|WaveStroke|波浪边框画刷||||

# 案例

```xml
<StackPanel Orientation="Horizontal" Margin="0,32,0,0">
    <hc:WaveProgressBar Value="{Binding Value,ElementName=SliderDemo}"/>
    <hc:WaveProgressBar Value="{Binding Value,ElementName=SliderDemo}" FontSize="20" Margin="16,0,0,0" WaveThickness="4" WaveStroke="#FFFF0080">
        <hc:WaveProgressBar.WaveFill>
            <LinearGradientBrush EndPoint="0,1" StartPoint="0,0">
                <GradientStop Color="#66FF0080" Offset="0"/>
                <GradientStop Color="#FFFF0080" Offset="1"/>
            </LinearGradientBrush>
        </hc:WaveProgressBar.WaveFill>
    </hc:WaveProgressBar>
    <hc:WaveProgressBar Value="{Binding Value,ElementName=SliderDemo}" Margin="16,0,0,0" ShowText="False" Width="50" Height="50" Style="{StaticResource ProgressBarWarningWave}"/>
</StackPanel>
```

![WaveProgressBar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/WaveProgressBar.gif)
---
title: GotoTop 回到顶部
---

通过点击 `GotoTop` 可以让 `ScrollView` 或带有 `ScrollView` 的控件滚动到顶部位置.

```cs
public class GotoTop : Button
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Target|`ScrollView` 或带有 `ScrollView` 的控件|||
|Animated|是否使用动画|true||
|AnimationTime|动画持续时间|0.2s||
|HidingHeight|当 `ScrollView` 滚动该高度后 `GotoTop` 会被隐藏|0||
|AutoHiding|是否自动隐藏|true|||

# 案例

```xml
<hc:SimplePanel Width="500" Height="400">
    <hc:ScrollViewer Name="ScrollViewerDemo" IsInertiaEnabled="True" Margin="0,10,0,0">
        <Border Height="2000" Margin="8,0">
            <Border.Background>
                <LinearGradientBrush EndPoint="0.5,1" StartPoint="0.5,0">
                    <GradientStop Color="White" Offset="0"/>
                    <GradientStop Color="Black" Offset="1"/>
                </LinearGradientBrush>
            </Border.Background>
        </Border>
    </hc:ScrollViewer>
    <hc:GotoTop Animated="True" AutoHiding="True" AnimationTime="500" Target="{Binding ElementName=ScrollViewerDemo}" HorizontalAlignment="Right" VerticalAlignment="Bottom" Margin="0,0,20,20"/>
</hc:SimplePanel>
```

![GotoTop](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/GoToTop.gif)
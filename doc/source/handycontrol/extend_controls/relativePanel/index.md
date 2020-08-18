---
title: RelativePanel 相对布局
---

可以使用 `RelativePanel` 的附加属性对其内容进行相对布局.
详见：[RelativePanel Class](https://docs.microsoft.com/zh-cn/uwp/api/Windows.UI.Xaml.Controls.RelativePanel)

```cs
public class RelativePanel : Panel
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Above|使此元素位于目标元素的上方|||
|AlignBottomWith|使此元素与目标元素底部对齐|||
|AlignBottomWithPanel|使此元素与Panel底部对齐|false||
|AlignHorizontalCenterWith|使此元素与目标元素水平居中对齐|||
|AlignHorizontalCenterWithPanel|使此元素与Panel水平居中对齐|false||
|AlignLeftWith|使此元素与目标元素左侧对齐|||
|AlignLeftWithPanel|使此元素与Panel左侧对齐|false||
|AlignRightWith|使此元素与目标元素右侧对齐|||
|AlignRightWithPanel|使此元素与Panel右侧对齐|false||
|AlignTopWith|使此元素与目标元素顶部对齐|||
|AlignTopWithPanel|使此元素与Panel顶侧对齐|false||
|AlignVerticalCenterWith|使此元素与目标元素垂直居中对齐|||
|AlignVerticalCenterWithPanel|使此元素与Panel垂直居中对齐|false||
|Below|使此元素位于目标元素的下方|||
|LeftOf|使此元素位于目标元素的左侧|||
|RightOf|使此元素位于目标元素的右侧||||

# 案例

```xml
<hc:RelativePanel Width="620" Height="700" Margin="32">
    <Border Name="Rect1" Background="{DynamicResource DangerBrush}" Height="50" Width="50">
        <TextBlock Text="Rect1" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect2" Background="{DynamicResource PrimaryBrush}" Height="50" Width="50" hc:RelativePanel.AlignHorizontalCenterWithPanel="True">
        <TextBlock Text="Rect2" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect3" Background="{DynamicResource SuccessBrush}" Height="50" Width="50" hc:RelativePanel.AlignRightWithPanel="True">
        <TextBlock Text="Rect3" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect4" Background="{DynamicResource DangerBrush}" Height="50" Width="50" hc:RelativePanel.AlignBottomWithPanel="True">
        <TextBlock Text="Rect4" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect5" Background="{DynamicResource PrimaryBrush}" Height="50" Width="50" hc:RelativePanel.AlignBottomWithPanel="True" hc:RelativePanel.AlignHorizontalCenterWithPanel="True">
        <TextBlock Text="Rect5" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect6" Background="{DynamicResource SuccessBrush}" Height="50" Width="50" hc:RelativePanel.AlignBottomWithPanel="True" hc:RelativePanel.AlignRightWithPanel="True">
        <TextBlock Text="Rect6" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect7" Background="{DynamicResource PrimaryBrush}" Height="50" hc:RelativePanel.RightOf="{Binding ElementName=Rect1}">
        <TextBlock Text="Rect7 (RightOf Rect1)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect8" Background="{DynamicResource SuccessBrush}" Height="50" hc:RelativePanel.Below="{Binding ElementName=Rect7}">
        <TextBlock Text="Rect8 (Below Rect7)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect9" Background="{DynamicResource PrimaryBrush}" Height="140" Width="460" hc:RelativePanel.AlignHorizontalCenterWithPanel="True" hc:RelativePanel.AlignVerticalCenterWithPanel="True">
        <TextBlock Text="Rect9" Padding="10" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Top"/>
    </Border>
    <Border Name="Rect10" Background="{DynamicResource DangerBrush}" Width="50" hc:RelativePanel.RightOf="{Binding ElementName=Rect9}" hc:RelativePanel.AlignVerticalCenterWith="{Binding ElementName=Rect9}">
        <TextBlock Text="Rect14 (RightOf Rect9, AlignVerticalCenterWith Rect9)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center">
            <TextBlock.LayoutTransform>
                <TransformGroup>
                    <RotateTransform Angle="-90"/>
                </TransformGroup>
            </TextBlock.LayoutTransform>
        </TextBlock>
    </Border>
    <Border Name="Rect11" Background="{DynamicResource DangerBrush}" Height="50" hc:RelativePanel.AlignBottomWith="{Binding ElementName=Rect9}" hc:RelativePanel.AlignHorizontalCenterWith="{Binding ElementName=Rect9}">
        <TextBlock Text="Rect11 (AlignBottomWith Rect9, AlignHorizontalCenterWith Rect9)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect12" Background="{DynamicResource DangerBrush}" Height="50" hc:RelativePanel.Below="{Binding ElementName=Rect8}" hc:RelativePanel.AlignLeftWith="{Binding ElementName=Rect7}">
        <TextBlock Text="Rect12 (Below Rect8, AlignLeftWith Rect7)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect13" Background="{DynamicResource PrimaryBrush}" Height="50" hc:RelativePanel.Below="{Binding ElementName=Rect12}" hc:RelativePanel.AlignRightWith="{Binding ElementName=Rect12}">
        <TextBlock Text="Rect13 (Below Rect12, AlignRightWith Rect12)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect14" Background="{DynamicResource SuccessBrush}" Height="50" hc:RelativePanel.Above="{Binding ElementName=Rect9}" hc:RelativePanel.AlignRightWith="{Binding ElementName=Rect9}">
        <TextBlock Text="Rect14 (Above Rect9, AlignRightWith Rect9)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
    <Border Name="Rect15" Background="{DynamicResource DangerBrush}" Height="50" hc:RelativePanel.LeftOf="{Binding ElementName=Rect2}" hc:RelativePanel.AlignTopWith="{Binding ElementName=Rect9}">
        <TextBlock Text="Rect15 (LeftOf Rect2, AlignTopWith Rect9)" Padding="10,0" Foreground="White" FontWeight="Bold" HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Border>
</hc:RelativePanel>
```

![RelativePanel](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/RelativePanel.png)
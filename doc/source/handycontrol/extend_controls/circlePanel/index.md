---
title: CirclePanel 圆形布局
---

圆形布局常用作方向按钮、功能按钮的容器。

```cs
public class CirclePanel : Panel
```

# 属性

| 属性                   | 用途                           |
| ---------------------- | ------------------------------|
| Diameter               | 直径                          |
| KeepVertical           | 元素是否保持垂直               |
| OffsetAngle            | 整体的角度偏移                 |

# 案例

```xml
<Style x:Key="CirclePanelButton" BasedOn="{StaticResource ButtonCustom}" TargetType="Button">
    <Setter Property="UseLayoutRounding" Value="False"/>
    <Setter Property="Height" Value="77"/>
    <Setter Property="Width" Value="36.06"/>
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <hc:SimplePanel>
                    <Path Data="{StaticResource CirclePanelDemoGeometry}" Fill="{DynamicResource PrimaryBrush}" Height="77" Width="36.06"/>
                    <Path Data="{StaticResource CirclePanelRightGeometry}" Stretch="Uniform" Margin="12" Fill="White"/>
                </hc:SimplePanel>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter Property="Opacity" Value=".9"/>
                    </Trigger>
                    <Trigger Property="IsPressed" Value="True">
                        <Setter Property="Opacity" Value=".6"/>
                    </Trigger>
                    <Trigger Property="IsEnabled" Value="False">
                        <Setter Property="Opacity" Value="0.4"/>
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>

<hc:CirclePanel Margin="64" Diameter="170">
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
    <Button Style="{StaticResource CirclePanelButton}"/>
</hc:CirclePanel>
```

![CirclePanel](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CirclePanel.png)
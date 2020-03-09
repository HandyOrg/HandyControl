---
title: ToggleButton 切换按钮
---

# ToggleButtonBaseStyle : ButtonBaseBaseStyle

切换按钮默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info %}
所有继承此样式的按钮都可以使用`IconElement`中定义的附加属性来控制按钮中几何图形的属性。
{% endnote %}

{% note info %}
所有继承此样式的按钮都可以使用`BorderElement.CornerRadius`附加属性来控制按钮的圆角大小。
{% endnote %}

## 相关样式

| 名称 | 继承自 | 用途描述 |
| - | - | - |
| ToggleButtonPrimary     | ToggleButtonBaseStyle   | 主要       |
| ToggleButtonSuccess     | ToggleButtonBaseStyle   | 成功       |
| ToggleButtonInfo     | ToggleButtonBaseStyle   | 信息       |
| ToggleButtonWarning     | ToggleButtonBaseStyle   | 警告       |
| ToggleButtonDanger     | ToggleButtonBaseStyle   | 危险       |

案例：

```xml
<StackPanel HorizontalAlignment="Center" VerticalAlignment="Center"> 
    <ToggleButton MinWidth="100" Content="Default"/>
    <ToggleButton MinWidth="100" Content="Primary" Margin="0,6,0,0" Style="{StaticResource ToggleButtonPrimary}"/>
    <ToggleButton MinWidth="100" Content="Success" Margin="0,6,0,0" Style="{StaticResource ToggleButtonSuccess}"/>
    <ToggleButton MinWidth="100" Content="Info" Margin="0,6,0,0" Style="{StaticResource ToggleButtonInfo}"/>
    <ToggleButton MinWidth="100" Content="Warning" Margin="0,6,0,0" Style="{StaticResource ToggleButtonWarning}"/>
    <ToggleButton MinWidth="100" Content="Danger" Margin="0,6,0,0" Style="{StaticResource ToggleButtonDanger}"/>
</StackPanel>
```

效果：

![ToggleButtonBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ToggleButtonBaseStyle.png)

# ToggleButtonIconBaseStyle : BaseStyle

只显示图标的切换按钮，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info %}
所有继承此样式的按钮都可以使用`IconElement`中定义的附加属性来控制按钮中几何图形的属性。
{% endnote %}

{% note info %}
所有继承此样式的按钮都可以使用`BorderElement.CornerRadius`附加属性来控制按钮的圆角大小。
{% endnote %}

## 相关样式

| 名称 | 继承自 | 用途描述 |
| - | - | - |
| ToggleButtonIcon     | ToggleButtonIconBaseStyle   | 默认       |
| ToggleButtonIconPrimary     | ToggleButtonIconBaseStyle   | 主要       |
| ToggleButtonIconSuccess     | ToggleButtonIconBaseStyle   | 成功       |
| ToggleButtonIconInfo     | ToggleButtonIconBaseStyle   | 信息       |
| ToggleButtonIconWarning     | ToggleButtonIconBaseStyle   | 警告       |
| ToggleButtonIconDanger     | ToggleButtonIconBaseStyle   | 危险       |
| ToggleButtonIconTransparent     | ToggleButtonIconBaseStyle   | 透明       |

案例：

```xml
<StackPanel Orientation="Horizontal" HorizontalAlignment="Center" VerticalAlignment="Center">
    <ToggleButton Padding="6" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIcon}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconPrimary}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconSuccess}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconInfo}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconWarning}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconDanger}"/>
    <ToggleButton Margin="6,0,0,0" hc:IconElement.Geometry="{StaticResource ClockGeometry}" Style="{StaticResource ToggleButtonIconTransparent}"/>
</StackPanel>
```

效果：

![ToggleButtonIconBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ToggleButtonIconBaseStyle.png)

# ToggleButtonSwitchBaseStyle : BaseStyle

开关式切换按钮，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

## 相关样式

| 名称 | 继承自 |
| - | - |
| ToggleButtonSwitch     | ToggleButtonSwitchBaseStyle   |

案例：

```xml
<StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
    <ToggleButton Style="{StaticResource ToggleButtonSwitch}"/>
    <ToggleButton Margin="0,6,0,0" IsChecked="True" Style="{StaticResource ToggleButtonSwitch}"/>
</StackPanel>
```

效果：

![ToggleButtonSwitchBaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ToggleButtonSwitchBaseStyle.png)

# ToggleButtonFlip : BaseStyle

翻转式切换按钮。

案例：

```xml
<StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
    <ToggleButton BorderThickness="0" IsChecked="True" Style="{StaticResource ToggleButtonFlip}">
        <hc:StatusSwitchElement.CheckedElement>
            <Border Background="{DynamicResource PrimaryBrush}">
                <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center" Text="关" Foreground="{DynamicResource TextIconBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <Border Background="{DynamicResource DangerBrush}">
            <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center" Text="开" Foreground="{DynamicResource TextIconBrush}"/>
        </Border>
    </ToggleButton>
    <ToggleButton Margin="0,6,0,0" BorderThickness="0" IsChecked="False" Style="{StaticResource ToggleButtonFlip}">
        <hc:StatusSwitchElement.CheckedElement>
            <Border Background="{DynamicResource PrimaryBrush}">
                <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center" Text="关" Foreground="{DynamicResource TextIconBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <Border Background="{DynamicResource DangerBrush}">
            <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center" Text="开" Foreground="{DynamicResource TextIconBrush}"/>
        </Border>
    </ToggleButton>
</StackPanel>
```

效果：

![ToggleButtonFlip](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ToggleButtonFlip.png)

# ToggleButtonCustom : BaseStyle

如果想完全自定义按钮的内容，则推荐使用此样式。`ToggleButtonCustom`中的内容完全由你自己决定。

案例：

```xml
<StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
    <ToggleButton IsChecked="True" Style="{StaticResource ToggleButtonCustom}" hc:StatusSwitchElement.HideUncheckedElement="True">
        <hc:StatusSwitchElement.CheckedElement>
            <Border Width="80" Height="30" CornerRadius="4" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
                <Ellipse Width="20" Height="20" Fill="{DynamicResource PrimaryBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <Border Width="80" Height="30" CornerRadius="4" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
            <Ellipse Width="20" Height="20" Fill="{DynamicResource BorderBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
        </Border>
    </ToggleButton>
    <ToggleButton Margin="0,6,0,0" IsChecked="False" Style="{StaticResource ToggleButtonCustom}" hc:StatusSwitchElement.HideUncheckedElement="True">
        <hc:StatusSwitchElement.CheckedElement>
            <Border Width="80" Height="30" CornerRadius="4" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
                <Ellipse Width="20" Height="20" Fill="{DynamicResource PrimaryBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
            </Border>
        </hc:StatusSwitchElement.CheckedElement>
        <Border Width="80" Height="30" CornerRadius="4" BorderThickness="1" BorderBrush="{DynamicResource BorderBrush}">
            <Ellipse Width="20" Height="20" Fill="{DynamicResource BorderBrush}" StrokeThickness="1" Stroke="{DynamicResource BorderBrush}"/>
        </Border>
    </ToggleButton>
</StackPanel>
```

效果：

![ToggleButtonCustom](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ToggleButtonCustom.png)

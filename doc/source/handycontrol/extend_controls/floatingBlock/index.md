---
title: FloatingBlock 漂浮块
---

可借助漂浮块实现特殊的点击效果。

```cs
public class FloatingBlock : Control
```

# 依赖属性

|属性|描述|默认值|备注|
|-|-|-|-|
|ToX|X轴消失位置|0||
|ToY|y轴消失位置|-100||
|Duration|动画持续时间|2s||
|HorizontalOffset|横向偏移|0||
|VerticalOffset|纵向偏移|0||
|ContentTemplate|漂浮内容模板|||
|Content|漂浮内容||||

# 案例

```xml
<StackPanel Margin="32" VerticalAlignment="Center">
    <Button hc:IconElement.Geometry="{StaticResource ThumbsUpGeometry}" Width="180">
        <hc:FloatingBlock.ContentTemplate>
            <DataTemplate>
                <Path Data="{StaticResource ThumbsUpGeometry}" Fill="{DynamicResource DangerBrush}" Width="16" Height="16"/>
            </DataTemplate>
        </hc:FloatingBlock.ContentTemplate>
    </Button>
    <Button Content="Good" hc:FloatingBlock.Content="Good" Width="180" Margin="0,10,0,0"/>
    <Button hc:IconElement.Geometry="{StaticResource ThumbsUpGeometry}" Width="180" Margin="0,10,0,0" hc:FloatingBlock.Duration="0:0:1" hc:FloatingBlock.VerticalOffset="-20" hc:FloatingBlock.ToX="50" hc:FloatingBlock.ToY="-80">
        <hc:FloatingBlock.ContentTemplate>
            <DataTemplate>
                <Path Data="{StaticResource ThumbsUpGeometry}" Fill="{DynamicResource DangerBrush}" Width="16" Height="16"/>
            </DataTemplate>
        </hc:FloatingBlock.ContentTemplate>
    </Button>
</StackPanel>
```

![FloatingBlock](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/FloatingBlock.gif)
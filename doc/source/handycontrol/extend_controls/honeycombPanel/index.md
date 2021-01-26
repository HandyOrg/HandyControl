---
title: HoneycombPanel 蜂窝布局
---

一种可以让子元素呈现蜂窝状布局的容器.

```cs
public class HoneycombPanel : Panel
```

# 蜂窝布局方式

其它元素将以第一个元素为起点，环绕式布局：

```
         ●       ●
        (7)     (8)

     ●       ●       ●
    (6)     (1)     (9)

 ●       ●       ●       ...
(5)     (0)     (2)

     ●       ●       ...
    (4)     (3)
```

# 案例

```xml
<ListBox Background="Transparent" BorderThickness="0" Style="{StaticResource ListBoxCustom}" ItemsSource="{Binding DataList}">
    <ListBox.ItemsPanel>
        <ItemsPanelTemplate>
            <hc:HoneycombPanel hc:PanelElement.FluidMoveBehavior="{StaticResource BehaviorXY200}"/>
        </ItemsPanelTemplate>
    </ListBox.ItemsPanel>
    <ListBox.ItemTemplate>
        <DataTemplate>
            <hc:Gravatar Style="{StaticResource GravatarCircle}" Margin="10" Source="{Binding Content}"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

![HoneycombPanel](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/HoneycombPanel.png)
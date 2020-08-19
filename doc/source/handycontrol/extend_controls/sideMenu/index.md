---
title: SideMenu 侧边菜单
---

当界面为左右布局时，可以使用侧边菜单来导航.

{% note warning %}
目前侧边菜单只支持垂直布局，将来可能扩展为任意布局.
{% endnote %}

```cs
[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class SimpleItemsControl : Control
```

```cs
public class HeaderedSimpleItemsControl : SimpleItemsControl
```

```cs
public class SideMenu : HeaderedSimpleItemsControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|AutoSelect|是否自动选中首项|true||
|ExpandMode|子项展开模式|ExpandMode.ShowOne||
|PanelAreaLength|子项容器高度|NaN|目前侧边菜单只支持垂直布局，将来可能扩展为任意布局，故此属性名称没有定为`PanelAreaHeight`|

# 事件

|名称|说明|
|-|-|
| SelectionChanged | 选中项改变时触发 |

# 案例

```xml
<hc:SideMenu BorderThickness="1" Width="200" Margin="32">
    <hc:Interaction.Triggers>
        <hc:EventTrigger EventName="SelectionChanged">
            <hc:EventToCommand Command="{Binding SwitchItemCmd}" PassEventArgsToCommand="True" />
        </hc:EventTrigger>
    </hc:Interaction.Triggers>
    <hc:SideMenuItem Header="Overview">
        <hc:SideMenuItem.Icon>
            <Image Source="/HandyControlDemo;component/Resources/Img/DevOps/DevOps-Overview.png" Width="24" Height="24"/>
        </hc:SideMenuItem.Icon>
        <hc:SideMenuItem Header="Summary" Command="{Binding SelectCmd}" CommandParameter="{Binding Header,RelativeSource={RelativeSource Self}}">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf2cb;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Dashboards" Command="{Binding SelectCmd}" CommandParameter="{Binding Header,RelativeSource={RelativeSource Self}}">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf246;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Wiki" Command="{Binding SelectCmd}" CommandParameter="{Binding Header,RelativeSource={RelativeSource Self}}">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xe82d;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
    </hc:SideMenuItem>
    <hc:SideMenuItem Header="Boards">
        <hc:SideMenuItem.Icon>
            <Image Source="/HandyControlDemo;component/Resources/Img/DevOps/DevOps-Boards.png" Width="24" Height="24"/>
        </hc:SideMenuItem.Icon>
        <hc:SideMenuItem Header="Work Items">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf314;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Boards">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf444;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Backlogs">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf6bf;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Sprints">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf3b0;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Queries">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf2b8;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
    </hc:SideMenuItem>
    <hc:SideMenuItem Header="Repos">
        <hc:SideMenuItem.Icon>
            <Image Source="/HandyControlDemo;component/Resources/Img/DevOps/DevOps-Repos.png" Width="24" Height="24"/>
        </hc:SideMenuItem.Icon>
        <hc:SideMenuItem Header="Files">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf30e;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Commits">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf293;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Pushes">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf298;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Branches">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xebc2;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Tags">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xe8ec;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Pull requests">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf296;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
    </hc:SideMenuItem>
    <hc:SideMenuItem Header="Pipelines">
        <hc:SideMenuItem.Icon>
            <Image Source="/HandyControlDemo;component/Resources/Img/DevOps/DevOps-Pipelines.png" Width="24" Height="24"/>
        </hc:SideMenuItem.Icon>
        <hc:SideMenuItem Header="Builds">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf28f;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Releases">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf3b3;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Library">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xe8f1;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Task groups">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf2ae;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Deployment groups">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf29d;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
    </hc:SideMenuItem>
    <hc:SideMenuItem Header="Test Plans">
        <hc:SideMenuItem.Icon>
            <Image Source="/HandyControlDemo;component/Resources/Img/DevOps/DevOps-TestPlans.png" Width="24" Height="24"/>
        </hc:SideMenuItem.Icon>
        <hc:SideMenuItem Header="Test Plans">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf3ab;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Runs">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xf3ac;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
        <hc:SideMenuItem Header="Load test">
            <hc:SideMenuItem.Icon>
                <TextBlock Text="&#xe8a9;" Style="{StaticResource TextBlockFabricIcons}"/>
            </hc:SideMenuItem.Icon>
        </hc:SideMenuItem>
    </hc:SideMenuItem>
</hc:SideMenu>
```

![SideMenu](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/SideMenu.png)
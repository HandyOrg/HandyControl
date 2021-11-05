---
title: StepBar 步骤条
---

引导用户按照流程完成任务的分步导航条.

```cs
[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StepBarItem))]
[DefaultEvent("StepChanged")]
[TemplatePart(Name = ElementProgressBarBack, Type = typeof(ProgressBar))]
public class StepBar : ItemsControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|StepIndex|当前步骤序号|0||
|Dock|步骤条顶靠方式|Dock.Top|||

# 方法

|名称|说明|
|-|-|
| Next( ) | 跳转到下一步 |
| Prev( ) | 跳转到上一步 |

# 事件

|名称|说明|
|-|-|
| StepChanged | 步骤改变时触发 |

# 案例

```xml
<Grid Margin="32">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
    <hc:StepBar Grid.ColumnSpan="2" StepIndex="{Binding StepIndex}">
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.Register}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.BasicInfo}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.UploadFile}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.Complete}}"/>
    </hc:StepBar>
    <StackPanel Margin="0,32" Grid.Column="0" Grid.Row="1" Grid.ColumnSpan="2" HorizontalAlignment="Center">
        <Button Command="{Binding PrevCmd}" CommandParameter="{Binding ElementName=PanelMain}" Width="180" Content="{ex:Lang Key={x:Static langs:LangKeys.Prev}}" Style="{StaticResource ButtonPrimary}"/>
        <Button Command="{Binding NextCmd}" CommandParameter="{Binding ElementName=PanelMain}" Width="180" Margin="0,16,0,0" Content="{ex:Lang Key={x:Static langs:LangKeys.Next}}" Style="{StaticResource ButtonPrimary}"/>
    </StackPanel>
    <hc:StepBar Margin="0,0,0,32" Grid.Column="0" Grid.Row="2" Grid.ColumnSpan="2" Dock="Bottom">
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.Register}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.BasicInfo}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.UploadFile}}"/>
        <hc:StepBarItem Content="{ex:Lang Key={x:Static langs:LangKeys.Complete}}"/>
    </hc:StepBar>
    <hc:StepBar Grid.Column="0" Grid.Row="3" ItemsSource="{Binding DataList}" Dock="Left">
        <hc:StepBar.ItemTemplate>
            <DataTemplate>
                <StackPanel>
                    <TextBlock FontSize="16" FontWeight="Bold" HorizontalAlignment="Left">
                        <Run Text="{ex:Lang Key={Binding Header}}"/>
                        <Run Text="{Binding Index,RelativeSource={RelativeSource AncestorType=hc:StepBarItem}}"/>
                    </TextBlock>
                    <TextBlock Margin="0,4,0,0" Text="{ex:Lang Key={Binding Content}}"/>
                </StackPanel>
            </DataTemplate>
        </hc:StepBar.ItemTemplate>
    </hc:StepBar>
    <hc:StepBar Grid.Column="1" Grid.Row="3" ItemsSource="{Binding DataList}" Dock="Right">
        <hc:StepBar.ItemTemplate>
            <DataTemplate>
                <StackPanel>
                    <TextBlock FontSize="16" FontWeight="Bold" HorizontalAlignment="Left">
                        <Run Text="{ex:Lang Key={Binding Header}}"/>
                        <Run Text="{Binding Index,RelativeSource={RelativeSource AncestorType=hc:StepBarItem}}"/>
                    </TextBlock>
                    <TextBlock Margin="0,4,0,0" Text="{ex:Lang Key={Binding Content}}"/>
                </StackPanel>
            </DataTemplate>
        </hc:StepBar.ItemTemplate>
    </hc:StepBar>
</Grid>
```

![StepBar](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/StepBar.gif)
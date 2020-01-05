---
title: ListBox 列表框
---

#  ListBoxBaseStyle

 ListBox默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

{% note info no-icon %}
用例：
{% code %}
    <Style BasedOn="{StaticResource ListBoxBaseStyle}" TargetType="ListBox"/>
{% endcode %}

![ListBox.BaseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.BaseStyle.png)

{% endnote %}

#  ListBoxCustom : ListBoxBaseStyle

ListBox列表Custom样式，该样式保留了Listbox的基本属性样式，而数据显示样式由当前用户自定义，实现个性化定制。

{% note info no-icon %}
用例：
{% code %}
    <ListBox Margin="10" ItemsSource="{Binding Datas}"  
             Style="{DynamicResource ListBoxCustom}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Border BorderThickness="1" BorderBrush="Black" Margin="0,5">
                    <DockPanel LastChildFill="True">
                        <Path DockPanel.Dock="Left" Fill="YellowGreen" Width="20" Margin="10,0,10,0" HorizontalAlignment="Center" Data="{DynamicResource BubbleTailGeometry}"></Path>
                        <TextBlock Padding="10" Text="{Binding Name}"></TextBlock>
                    </DockPanel>
                </Border>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
{% endcode %}

![ListBox.CustomStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.CustomStyle.png)

{% endnote %}

#  WrapPanelHorizontalListBox : ListBoxCustom

布局容器为WrapPanel，水平方向显示样式。

{% note info no-icon %}
用例：
{% code %}
    <ListBox Margin="10" ItemsSource="{Binding Datas}"  
             Style="{DynamicResource WrapPanelHorizontalListBox}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Border BorderThickness="1" BorderBrush="Black" Margin="5,0">
                    <DockPanel LastChildFill="True">
                        <Path DockPanel.Dock="Left" Fill="YellowGreen" Width="20" Margin="10,0,10,0" HorizontalAlignment="Center" Data="{DynamicResource BubbleTailGeometry}"></Path>
                        <TextBlock Padding="10" Text="{Binding Name}"></TextBlock>
                    </DockPanel>
                </Border>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
{% endcode %}

![ListBox.WrapPanelHorizontalStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.WrapPanelHorizontalStyle.png)

{% endnote %}

#  WrapPanelVerticalListBox : ListBoxCustom

布局容器为WrapPanel，垂直方向显示样式。

{% note info no-icon %}
用例：
{% code %}
    <ListBox Margin="10" ItemsSource="{Binding Datas}"  
             Style="{DynamicResource WrapPanelVerticalListBox}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Border BorderThickness="1" BorderBrush="Black" Margin="0,5">
                    <DockPanel LastChildFill="True">
                        <Path DockPanel.Dock="Left" Fill="YellowGreen" Width="20" Margin="10,0,10,0" HorizontalAlignment="Center" Data="{DynamicResource BubbleTailGeometry}"></Path>
                        <TextBlock Padding="10" Text="{Binding Name}"></TextBlock>
                    </DockPanel>
                </Border>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
{% endcode %}

![ListBox.WrapPanelVerticalStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.WrapPanelVerticalStyle.png)

{% endnote %}

#  StackPanelHorizontalListBox : ListBoxCustom

布局容器为StackPanel，水平方向显示样式。

{% note info no-icon %}
用例：
{% code %}
    <ListBox Margin="10" ItemsSource="{Binding Datas}"  
             Style="{DynamicResource StackPanelHorizontalListBox}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Border BorderThickness="1" BorderBrush="Black" Margin="5,0">
                    <DockPanel LastChildFill="True">
                        <Path DockPanel.Dock="Left" Fill="YellowGreen" Width="20" Margin="10,0,10,0" HorizontalAlignment="Center" Data="{DynamicResource BubbleTailGeometry}"></Path>
                        <TextBlock Padding="10" Text="{Binding Name}"></TextBlock>
                    </DockPanel>
                </Border>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
{% endcode %}

![ListBox.StackPanelHorizontalStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.StackPanelHorizontalStyle.png)

{% endnote %}

#  StackPanelVerticalListBox : ListBoxCustom

布局容器为StackPanel，垂直方向显示样式。

{% note info no-icon %}
用例：
{% code %}
    <ListBox Margin="10" ItemsSource="{Binding Datas}"  
             Style="{DynamicResource StackPanelVerticalListBox}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Border BorderThickness="1" BorderBrush="Black" Margin="0,1">
                    <DockPanel LastChildFill="True">
                        <Path DockPanel.Dock="Left" Fill="YellowGreen" Width="20" Margin="10,0,10,0" HorizontalAlignment="Center" Data="{DynamicResource BubbleTailGeometry}"></Path>
                        <TextBlock Padding="10" Text="{Binding Name}"></TextBlock>
                    </DockPanel>
                </Border>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
{% endcode %}

![ListBox.StackPanelVerticalStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ListBox.StackPanelVerticalStyle.png)

{% endnote %}
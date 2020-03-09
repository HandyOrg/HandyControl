---
title: DataGrid 数据表格
---

# 相关样式

| style名称 | 所属类型 | 用途描述 |
| - | - | - |
| RowHeaderGripperStyle     | Thumb                                       | 行标题拖拽条样式       |
| ColumnHeaderGripperStyle  | Thumb                                       | 列标题拖拽条样式       |
| DataGridCellStyle         | DataGridCell                                | 单元格样式            |
| DataGridRowStyle          | DataGridRow                                 | 行样式                |
| DataGridColumnHeaderStyle | DataGridColumnHeader                        | 列标题样式             |
| DataGridRowHeaderStyle    | DataGridRowHeader                           | 行标题样式             |
| TextBlockComboBoxStyle    | ComboBox                                    | ComboBox非编辑模式样式 |
| DataGridTextColumnStyle   | TextBlock                                   | 默认文本样式           |

{% note info no-icon %}
用例：

{% code %}
<DataGrid HeadersVisibility="All" RowHeaderWidth="60" AutoGenerateColumns="False" ItemsSource="{Binding DataList}">
    <DataGrid.RowHeaderTemplate>
        <DataTemplate>
            <CheckBox IsChecked="{Binding IsSelected,RelativeSource={RelativeSource AncestorType=DataGridRow}}"/>
        </DataTemplate>
    </DataGrid.RowHeaderTemplate>
    <DataGrid.Columns>
        <DataGridTextColumn IsReadOnly="True" Width="80" CanUserResize="False" Binding="{Binding Index}" Header="{x:Static langs:Lang.Index}"/>
        <DataGridTemplateColumn Width="60" CanUserResize="False">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <Image Source="{Binding ImgPath}" Width="32" Height="32" Stretch="Uniform"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
        <DataGridTextColumn Width="1*" Binding="{Binding Name}" Header="{x:Static langs:Lang.Name}"/>
        <DataGridCheckBoxColumn Width="100" CanUserResize="False" Binding="{Binding IsSelected}" Header="{x:Static langs:Lang.Selected}"/>
        <DataGridComboBoxColumn ItemsSource="{Binding Source={StaticResource DemoTypes}}" Width="100" CanUserResize="False" SelectedValueBinding="{Binding Type}" Header="{x:Static langs:Lang.Type}"/>
        <DataGridTextColumn Width="1*" Binding="{Binding Remark}" Header="{x:Static langs:Lang.Remark}"/>
    </DataGrid.Columns>
</DataGrid>
{% endcode %}
![DataGrid](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/DataGrid.png)
{% endnote %}

# FAQ
{% note warning  no-icon%}
对于文本显示列`DataGridTextColumn`需要设定文本内容`水平居中`或者`水平居右`，而不是HandyControl中设定的样式默认显示为`居左`时，需要继承`DataGridCellStyle`重写`HorizontalContentAlignment` 属性为`Center` 或`Right` 需要注意的是，此方式仅仅适用于引入了HandControl资源样式的项目，普通原生DataGrid使用该方式无效。

{% endnote %}

用例如下：

{% code %}

样式：

<Style x:Key="DataGridTextCenterColumnStyle" TargetType="DataGridCell" BasedOn="{StaticResource DataGridCellStyle}">
    <Setter Property="HorizontalContentAlignment" Value="Center"/>
</Style>

xaml中的使用：

<DataGrid ItemsSource="{Binding Datas}" AutoGenerateColumns="False">
    <DataGrid.Columns>
        <DataGridTextColumn Header="居左" Binding="{Binding Name}" Width="*"/>
        <DataGridTextColumn Header="居中" CellStyle="{StaticResource DataGridTextCenterColumnStyle}" Width="*" Binding="{Binding Name}"/>
​    </DataGrid.Columns>
</DataGrid>

{% endcode %}

效果如下：

![DataGridWarning01](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/DataGrid-Warning01.png)

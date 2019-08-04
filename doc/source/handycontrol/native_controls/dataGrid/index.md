---
title: DataGrid 数据表格
---

**HandyControl中自带DataGrid相关样式**

| style名称                 | 所属类型                                    | 用途描述                                               |
| :------------------------ | :------------------------------------------ | :----------------------------------------------------- |
| RowHeaderGripperStyle     | Thumb                                       | RowHeader的可拖拽样式                                  |
| ColumnHeaderGripperStyle  | Thumb                                       | ColumnHeader可拖拽样式                                 |
| DataGridCellStyle         | DataGridCell                                | DataGrid数据列样式                                     |
| DataGridRowStyle          | DataGridRow                                 | DataGrid数据行样式                                     |
| DataGridColumnHeaderStyle | DataGridColumnHeader                        | DataGrid列头样式                                       |
| DataGridRowHeaderStyle    | DataGridRowHeader                           | DataGrid行头样式                                       |
| TextBlockComboBoxStyle    | controls:DataGridAttach.ComboBoxColumnStyle | DataGrid附加属性文本框下拉列的非编辑模式下文本显示样式 |
| DataGridTextColumnStyle   | TextBlock                                   | DataGrid文本列样式                                     |

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
![ContextMenu](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/DataGrid.png)
{% endnote %}

**常见问题：**
{% note warning%}
对于文本显示列`DataGridTextColumn`需要设定文本内容`水平居中`或者`水平居右`，而不是HandyControl中设定的样式默认显示为`居左`时，需要继承`DataGridCellStyle`重写`HorizontalContentAlignment` 属性为`Center` 或`Right` 需要注意的是，此方式仅仅使用与引入了HandControl资源样式的项目，普通原生DataGrid使用该方式无效。

用例如下：

{% code %}

样式：
<Style x:Key="DataGridTextCenterColumnStyle" TargetType="DataGridCell" BasedOn="{StaticResource DataGridCellStyle}">
    <Setter Property="HorizontalContentAlignment" Value="Center"/>
</Style>

xaml中的使用：

<DataGrid ItemsSource="{Binding Datas}" AutoGenerateColumns="False">
            <DataGrid.Columns>
                <DataGridTextColumn Header="居左" Binding="{Binding Name}" Width="*"></DataGridTextColumn>
                <DataGridTextColumn Header="居中" CellStyle="{StaticResource DataGridTextCenterColumnStyle}" Width="*" Binding="{Binding Name}"></DataGridTextColumn>
            </DataGrid.Columns>
        </DataGrid>

{% endcode %}

效果如下：

![DataGridWarning01](..\images\DataGrid-Warning01.png)

{% endnode %}

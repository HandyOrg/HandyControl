---
title: DataGrid 数据表格
---

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
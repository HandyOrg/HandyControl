---
title: CoverView 封面视图
---

仿制Itunes的专辑视图。

```cs
public class CoverView : RegularItemsControl
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|CoverViewContentStyle|封面内容样式|||
|ItemContentHeight|项内容高度|300||
|ItemContentHeightFixed|项内容高度是否固定|true||
|ItemHeaderTemplate|项标题模板|||
|SourceContent|源内容（当前内容）|||
|Groups|组数||||

# 案例

```xml
<hc:CoverView Margin="27" Width="880" Height="432" ItemWidth="160" ItemHeight="160" ItemsSource="{Binding DataList}">
    <hc:CoverView.ItemHeaderTemplate>
        <DataTemplate>
            <Image Source="{Binding ImgPath}"/>
        </DataTemplate>
    </hc:CoverView.ItemHeaderTemplate>
    <hc:CoverView.ItemTemplate>
        <DataTemplate>
            <Border Margin="10" Height="300" Background="{Binding BackgroundToken,Converter={StaticResource String2BrushConverter}}">
                <TextBlock Text="{ex:Lang Key={x:Static langs:LangKeys.ContentDemoStr}}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White"/>
            </Border>
        </DataTemplate>
    </hc:CoverView.ItemTemplate>
</hc:CoverView>
```

![CoverView](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CoverView.gif)
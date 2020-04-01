---
title: Card 卡片
---

用于卡片化展示数据，为`ContentControl`的派生类

```c#
public class Card : ContentControl
```

# 基础属性

| 属性                   | 用途                           |
| ---------------------- | ------------------------------ |
| Header                 | 卡片头部内容，用于显示同步文本 |
| HeaderTemplate         | 卡片头部模板                   |
| HeaderTemplateSelector | 卡片模板样式选择器             |
| HeaderStringFormat     | 卡片头部模板内容显示格式       |
| Footer                 | 卡片尾部内容                   |
| FooterTemplate         | 卡片尾部模板                   |
| FooterTemplateSelector | 卡片尾部样式选择器             |
| FooterStringFormat     | 卡片尾部内容显示格式           |

# 案例

## 单卡片使用

### `xaml`中使用

```xml
    <hc:Card MaxWidth="240" BorderThickness="0" Effect="{DynamicResource EffectShadow2}" Margin="8">
        <!--Card 的内容部分-->
        <Border CornerRadius="4,4,0,0" Width="160" Height="160">
            <TextBlock TextWrapping="Wrap" VerticalAlignment="Center" HorizontalAlignment="Center" Text="测试"/>
        </Border>
        <!--Card 的尾部部分-->
        <hc:Card.Footer>
            <StackPanel Margin="10" Width="160">
                <!--Card 的一级内容-->
                <TextBlock TextWrapping="NoWrap"  Style="{DynamicResource TextBlockLargeBold}" TextTrimming="CharacterEllipsis" 
                                           Text="大标题" 
                                           HorizontalAlignment="Left"/>
                <!--Card 的二级内容-->
                <TextBlock TextWrapping="NoWrap" Style="{DynamicResource TextBlockDefault}" TextTrimming="CharacterEllipsis" 
                                           Text="描述信息" Margin="0,6,0,0"
                                           HorizontalAlignment="Left"/>
            </StackPanel>
        </hc:Card.Footer>
    </hc:Card>
```

### 效果

![Card-SampleCase](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Card-SampleCase.png)

## 作为数据模板

### 数据类型

```c#
    public class CardModel
    {
        public string Header { get; set; }

        public string Content { get; set; }

        public string Footer { get; set; }
    }
```

###  视图实体

此实体并没有按照规范的`mvvm`方式进行设计，仅仅是作为普通数据源做展示使用

```c#
    public class CardDemoViewModel
    {
        private IList<CardModel> _dataList;
        public CardDemoViewModel()
        {
            DataList = GetCardDataList();
        }

        internal ObservableCollection<CardModel> GetCardDataList()
        {
            return new ObservableCollection<CardModel>
            {
                new CardModel
                {
                    Header = "Atomic",
                    Content = "1.jpg",
                    Footer = "Stive Morgan"
                },
                new CardModel
                {
                    Header = "Zinderlong",
                    Content = "2.jpg",
                    Footer = "Zonderling"
                },
                new CardModel
                {
                    Header = "Busy Doin' Nothin'",
                    Content = "3.jpg",
                    Footer = "Ace Wilder"
                },
                new CardModel
                {
                    Header = "Wrong",
                    Content = "4.jpg",
                    Footer = "Blaxy Girls"
                },
                new CardModel
                {
                    Header = "The Lights",
                    Content = "5.jpg",
                    Footer = "Panda Eyes"
                },
                new CardModel
                {
                    Header = "EA7-50-Cent Disco",
                    Content = "6.jpg",
                    Footer = "еяхат музыка"
                },
                new CardModel
                {
                    Header = "Monsters",
                    Content = "7.jpg",
                    Footer = "Different Heaven"
                },
                new CardModel
                {
                    Header = "Gangsta Walk",
                    Content = "8.jpg",
                    Footer = "Illusionize"
                },
                new CardModel
                {
                    Header = "Won't Back Down",
                    Content = "9.jpg",
                    Footer = "Boehm"
                },
                new CardModel
                {
                    Header = "Katchi",
                    Content = "10.jpg",
                    Footer = "Ofenbach"
                }
            };
        }
        public IList<CardModel> DataList { get => _dataList; set => _dataList = value; }
    }
```

### `xaml`中的使用方式

`handycontrol`的命名空间和`DataContext`上下文需要自行引入

```xml
xmlns:hc="https://handyorg.github.io/handycontrol"
xmlns:data="CardModel所在命名空间"
xmlns:vm="CardDemoViewModel所在命名空间"
........
```

```xml
	<!--在listbox的父级中使用-->
    <listbox的父级.Resources>
        <vm:CardDemoViewModel x:Key="Card"></vm:CardDemoViewModel>
    </listbox的父级.Resources>
    <listbox的父级.DataContext>
        <Binding Source="{StaticResource Card}"/>
    </listbox的父级.DataContext>
```

```xml
<ListBox Margin="32" BorderThickness="0" Style="{DynamicResource WrapPanelHorizontalListBox}" ItemsSource="{Binding DataList}">
        <ListBox.ItemTemplate>
            <DataTemplate DataType="data:CardModel">
                <hc:Card MaxWidth="240" BorderThickness="0" Effect="{DynamicResource EffectShadow2}" Margin="8" Footer="{Binding Footer}">
                    <!--Card 的内容部分模板-->
                    <Border CornerRadius="4,4,0,0" Width="160" Height="160">
                        <TextBlock TextWrapping="Wrap" VerticalAlignment="Center" HorizontalAlignment="Center" Text="{Binding Content}"/>
                    </Border>
                    <!--Card 的尾部部分模板-->
                    <hc:Card.FooterTemplate>
                        <DataTemplate>
                            <StackPanel Margin="10" Width="160">
                                <!--Card 的一级内容-->
                                <TextBlock TextWrapping="NoWrap"  Style="{DynamicResource TextBlockLargeBold}" TextTrimming="CharacterEllipsis" 
                                           Text="{Binding DataContext.Header,RelativeSource={RelativeSource AncestorType=hc:Card}}" 
                                           HorizontalAlignment="Left"/>
                                <!--Card 的二级内容-->
                                <TextBlock TextWrapping="NoWrap" Style="{DynamicResource TextBlockDefault}" TextTrimming="CharacterEllipsis" 
                                           Text="{Binding}" Margin="0,6,0,0"
                                           HorizontalAlignment="Left"/>
                            </StackPanel>
                        </DataTemplate>
                    </hc:Card.FooterTemplate>
                </hc:Card>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
```

### 效果

![Card-Case01](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/Card-Case01.png)
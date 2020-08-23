---
title: TitleElement 标题元素
---

# 属性

| 名称           | 用途         |
| -------------- | ------------ |
| Title          | 标题信息     |
| Background     | 标题背景色   |
| Foreground     | 标题字体色   |
| BorderBrush    | 标题边框色   |
| TitlePlacement | 标题对齐方式 |
| TitleWidth     | 标题宽度     |

# 使用案例

## Title 标题信息

```xml
    <hc:TextBox hc:TitleElement.Title="标题信息"
                Margin="10,10"></hc:TextBox>
```

![TitleElement.Title](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/TitleElement.Title.png)

## TitlePlacement 标题对齐方式

```xml
    <!--标题居于顶侧-->
    <hc:TextBox hc:TitleElement.Title="标题信息"
             hc:TitleElement.TitlePlacement="Top"
             Margin="10,10"></hc:TextBox>
    <!--标题居于左侧-->
    <hc:TextBox hc:TitleElement.Title="标题信息"
             hc:TitleElement.TitlePlacement="Left"
             Margin="10,10"></hc:TextBox>
```

![TitleElement.TitlePlacement](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/TitleElement.TitlePlacement.png)

## TitleWidth 标题宽度

```xml
        <!--设定TitleWidth为Auto-->
        <hc:TextBox hc:TitleElement.Title="标题信息"
             hc:TitleElement.TitlePlacement="Left"
             hc:TitleElement.TitleWidth="Auto"
             Margin="10,10"></hc:TextBox>
        <!--设定TitleWidth为具体数值-->
        <hc:TextBox hc:TitleElement.Title="标题信息"
             hc:TitleElement.TitlePlacement="Left"
             hc:TitleElement.TitleWidth="60"
             Margin="10,10"></hc:TextBox>
```

![TitleElement.TitleWidth](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/TitleElement.TitleWidth.png)
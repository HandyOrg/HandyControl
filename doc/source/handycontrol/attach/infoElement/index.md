---
title: InfoElement信息元素
---

{%  note info no-icon %}

继承至[TitleElement]( https://handyorg.github.io/handycontrol/attach/titleElement/ )

{% endnote%}

# 属性

| 名称             | 用途               |
| ---------------- | ------------------ |
| Placeholder      | 占位符（输入提示） |
| Necessary        | 是否必填           |
| Symbol           | 标记信息           |
| ContentHeight    | 内容高度           |
| MinContentHeight | 最小内容高度       |
| MaxContentHeight | 最大内容高度       |

# 使用案例

对应`xaml`引入`handycontrol`对应的命名空间

```xml
xmlns:hc="https://handyorg.github.io/handycontrol"
```

## `Placeholder` 占位符

```xml
    <hc:SearchBar Style="{StaticResource SearchBarExtend}" Margin="10,10"/>
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" Style="{StaticResource SearchBarExtend}" Margin="10,10"/>
```

效果：

![InfoElement.Placeholder](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Placeholder.png)

## `Necessary` 是否必填

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" 
                  hc:InfoElement.Title="查询条件" 
                  Margin="10,10" hc:InfoElement.Necessary="True" 
                  Style="{StaticResource SearchBarExtend}"/>
```

其中`hc:InfoElement.Title="查询条件" `为继承至父类

效果：

![InfoElement.Necssary](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Necssary.png)

## `Symbol` 标记信息

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入内容" 
              hc:InfoElement.Title="此项必填" Style="{StaticResource SearchBarExtend}" 
              Margin="10,10" hc:InfoElement.Necessary="True" 
              hc:InfoElement.Symbol="x"/>
```

效果：

![InfoElement.Symbol](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Symbol.png)

## `ContentHeight` 内容高度

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" 
                  hc:InfoElement.ContentHeight="50"
                  hc:InfoElement.Title="查询条件" 
                  Margin="10,10" hc:InfoElement.Necessary="True" 
                  Style="{StaticResource SearchBarExtend}"/>
```

效果：

![InfoElement.ContentHeight](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.ContentHeight.png)
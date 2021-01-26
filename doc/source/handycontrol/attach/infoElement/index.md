---
title: InfoElement 信息元素
---

{%  note info no-icon %}

继承自 [TitleElement]( https://handyorg.github.io/handycontrol/attach/titleElement/ )

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
| RegexPattern | 正则表达式       |

# 使用案例

## Placeholder 占位符

```xml
<StackPanel Width="200" VerticalAlignment="Center">
    <hc:SearchBar/>
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" Style="{StaticResource SearchBarExtend}" Margin="0,16,0,0"/>
</StackPanel>
```

![InfoElement.Placeholder](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Placeholder.png)

## Necessary 是否必填

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" 
                  hc:InfoElement.Title="查询条件" 
                  Margin="10,10" hc:InfoElement.Necessary="True" 
                  Style="{StaticResource SearchBarExtend}"/>
```

其中`hc:InfoElement.Title="查询条件" `继承自父类

![InfoElement.Necssary](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Necssary.png)

## Symbol 标记信息

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入内容" 
              hc:InfoElement.Title="此项必填" Style="{StaticResource SearchBarExtend}" 
              Margin="10,10" hc:InfoElement.Necessary="True" 
              hc:InfoElement.Symbol="x"/>
```

![InfoElement.Symbol](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.Symbol.png)

## ContentHeight 内容高度

```xml
    <hc:SearchBar hc:InfoElement.Placeholder="请输入查询条件" 
                  hc:InfoElement.ContentHeight="50"
                  hc:InfoElement.Title="查询条件" 
                  Margin="10,10" hc:InfoElement.Necessary="True" 
                  Style="{StaticResource SearchBarExtend}"/>
```

![InfoElement.ContentHeight](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/InfoElement.ContentHeight.png)

## RegexPattern 正则表达式

通过设置该属性，可自定义信息元素内部的验证逻辑实现对指定格式外的内容进行错误提示。
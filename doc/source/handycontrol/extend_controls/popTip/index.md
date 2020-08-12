---
title: PopTip 气泡提示
---

常用于展示鼠标 hover 时的提示信息.

```cs
public class Poptip : AdornerElement
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|HitMode|触发方式|HitMode.Hover||
|Content|提示内容|||
|ContentTemplate|提示内容模板|||
|ContentStringFormat|提示内容文本格式|||
|ContentTemplateSelector|提示内容模板选择器|||
|Offset|偏移|6||
|PlacementType|出现位置|PlacementType.Top||
|IsOpen|是否打开|false|||

# 案例

```xml
<Button>
    <hc:Poptip.Instance>
        <hc:Poptip Content="{ex:Lang Key={x:Static langs:LangKeys.Text}}" PlacementType="TopLeft"/>
    </hc:Poptip.Instance>
</Button>
```

```xml
<ToggleButton hc:Poptip.HitMode="None" hc:Poptip.IsOpen="{Binding IsChecked,RelativeSource={RelativeSource Self}}" hc:Poptip.Content="{ex:Lang Key={x:Static langs:LangKeys.Text}}" hc:Poptip.Placement="RightTop"/>
```

![PopTip](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Poptip.gif)
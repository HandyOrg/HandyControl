---
title: TipElement 提示元素
---

# 属性

| 名称       | 用途     |
| ---------- | -------- |
| Placement | 提示元素显示位置 |
| Visibility | 是否可见 |


# 使用案例

## Placement 提示元素显示位置

该属性，用于设定提示元素显示位置，目前支持BottomRight和TopLeft（默认值）

```
<hc:RangeSlider Width="400" 
                    hc:TipElement.Visibility="Visible" 
                    TickPlacement="BottomRight" 
                    IsSnapToTickEnabled="True" 
                    Maximum="100" ValueEnd="60" 
                    TickFrequency="10" 
                    Margin="0,32,0,0"/>
    <hc:RangeSlider Width="400" 
                    hc:TipElement.Visibility="Visible"
                    hc:TipElement.Placement="BottomRight"
                    TickPlacement="BottomRight" 
                    IsSnapToTickEnabled="True" 
                    Maximum="100" ValueEnd="60" 
                    TickFrequency="10" 
                    Margin="0,32,0,0"/>
```

效果

![TipElement.Default](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/TipElement.Default.png)

![TipElement.TopLeft](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/attach/TipElement.TopLeft.png)


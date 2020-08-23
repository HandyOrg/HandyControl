---
title: ToggleBlock 切换块
---

该控件可根据选中状态切换显示内容，在某些情况下可以代替 `ToggleButton`.

```cs
public class ToggleBlock : Control
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|IsChecked|是否选中|false||
|CheckedContent|选中时显示的内容|||
|UnCheckedContent|未选中时显示的内容|||
|IndeterminateContent|选中状态未知时显示的内容||||

# 案例

```xml
<hc:ToggleBlock>
    <hc:ToggleBlock.UnCheckedContent>
        
    </hc:ToggleBlock.UnCheckedContent>
    <hc:ToggleBlock.CheckedContent>

    </hc:ToggleBlock.CheckedContent>
</hc:ToggleBlock>
```
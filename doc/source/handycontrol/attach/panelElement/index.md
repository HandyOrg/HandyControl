---
title: PanelElement 面板元素
---

# 属性

| 名称 | 用途 |
|-|-|
| FluidMoveBehavior | 设置面板的流体移动方式 |

# 使用案例

## FluidMoveBehavior 设置面板的流体移动方式

```xml
<hc:HoneycombPanel hc:PanelElement.FluidMoveBehavior="{StaticResource BehaviorXY200}"/>
```

当向面板容器中添加或移除元素时，就可以看到流体移动的动画效果。
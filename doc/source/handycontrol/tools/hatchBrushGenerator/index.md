---
title: HatchBrushGenerator 阴影画笔生成器
---

HC提供了50多种不同图案的阴影样式，其行为与winform中的表现一致.

# 案例

## 基础用法

我们先创建一个阴影画笔生成器实例：

```cs
var brushGenerator = new HatchBrushGenerator();
```

再调用 `GetHatchBrush` 方法，传入 `HatchStyle` 枚举、前景色、背景色，即可返回一个 `DrawingBrush` 画刷.

```cs
var brush =  brushGenerator.GetHatchBrush(HatchStyle.Horizontal, 前景色, 背景色);
```
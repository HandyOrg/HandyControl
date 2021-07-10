---
title: 原生控件
---

原生控件是指WPF框架自带的控件，除此之外，通过附加属性实现简单的功能扩展。

{% note warning %}
HandyControl中，几乎所有的原生控件都有默认实现的基样式（即定义样式的时候，没有显式给定Key值）。
{% endnote %}

{% note warning %}
基样式一般带有“Base”字样，不推荐用户直接在视图层中使用，合适的使用场景是资源文件。
{% endnote %}

# FAQ

{% note info no-icon %}
我该怎么覆盖样式的默认实现呢？没有Key可以给我BasedOn啊？
{% endnote %}

可以使用这种语法：`BasedOn="{StaticResource {x:Type Button}}"`来覆盖`Button`的默认样式。
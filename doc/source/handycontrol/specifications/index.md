---
title: 规范
---

HandyControl中有一些贯穿全部生命周期的规范，此文档将逐条列举。

# 样式命名规范

- 带有BaseStyle后缀的样式，不推荐在视图层使用，而适合于扩展。

{% note info %}
例如`ButtonBaseStyle`、`ComboBoxBaseStyle`。
{% endnote %}

- 带有Extend字样的样式，为原生控件样式的扩展模式，它比没有该字样的样式多一些功能。

{% note info %}
例如`ComboBoxExtendBaseStyle`、`TextBoxExtendBaseStyle`。
{% endnote %}

- 带有Plus字样的样式，为扩展控件的默认样式，它比`Extend`样式多一些功能。

{% note info %}
例如`ComboBoxPlusBaseStyle`、`TextBoxPlusBaseStyle`。
{% endnote %}

{% note info %}
如果一个控件拥有`Plus`样式，说明在原生库中存在与其同名的控件。
{% endnote %}

# 控件命名规范

- 功能和原生控件相似的扩展控件一般名称和原生控件一致。

{% note info %}
例如`Window`、`TabControl`、`ScrollViewer`,原生库中有这三个控件，但是HandyControl中也有，且名称一致。
{% endnote %}
---
title: 国际化
---

# 使用方式

可通过`ConfigHelper.Instance.SetLang(string lang)`指定需要使用的语言包，默认使用简体中文（zh-cn）。

控件库自带的语言包一般为控件库内部使用，但用户也可以通过以下两种方式使用：

- xaml
第一步，引入命名空间：`xmlns:hc="https://handyorg.github.io/handycontrol"`
第二步，使用语言包：`<TextBlock Text="{x:Static hc:Lang.Cancel}"/>`

- C#
`HandyControl.Properties.Langs.Lang.Cancel`

{% note warning %}
控件库不支持动态语言包切换，未来也不会有支持。
{% endnote %}

# 扩展

控件库自带的语言包包括：

* 简体中文（zh-cn）
* 英文（en）
* 波斯语（fa）
* 法语（fr）
* 韩文 （ko-kr）

默认为简体中文（zh-cn）。

如果需要可自行扩展，这里推荐使用开源插件：[ResXManager](https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager)，来维护所有的语言包。

引用控件库后，会在运行目录生成语言包文件夹，其命名方式形如zh-cn、en等。
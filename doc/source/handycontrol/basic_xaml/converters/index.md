---
title: 转换器
---

# Boolean2BooleanReConverter

该转换器用于反转布尔类型

# Boolean2StrConverter

该转换器需要提供一个字符串作为参数，该字符串的一般形式为：`[字符串0];[字符串1]`，当转换器获取的值为`true`时，会返回`[字符串0]`，否则返回`[字符串1]`。

# Boolean2VisibilityConverter

该转换器会将`true`转换为`Visible`，会将`false`转换为`Collapse`

# Boolean2VisibilityReConverter

该转换器情况与`Boolean2VisibilityConverter`相反。

# BooleanArr2VisibilityConverter

该转换器会将布尔数组转换为`Visibility`，当数组内全部为`true`时，返回`Visible`，否则返回`Collapse`。

# BorderCircularClipConverter

该转换器为Border专用，接受一个object数组，该数组长度为3，分别对应Border的长、宽和圆角半径，它会返回一个圆形几何形状作为Clip来剪裁Border。

# BorderCircularConverter

该转换器为Border专用，接受`Border`的长和宽合成的一个整型数组，返回它们中最小值的一半，作为圆形Border的半径。

# BorderClipConverter

该转换器为Border专用，接受一个object数组，该数组长度为3，分别对应Border的长、宽和圆角半径，它会返回一个圆角矩形作为Clip来剪裁Border。

# ColLayoutConverter

该转换器用于xaml设计时，主要功能是将字符串转换为有效的`ColLayout`值。

# Color2ChannelAConverter（内部使用）

该转换器会提取`SolidColorBrush.Color`的透明通道（A）的值。

# Color2HexStrConverter

该转换器会将`SolidColorBrush`转换为16进制字符串形式（带“#”）。

# CornerRadiusSplitConverter

该转换器需要提供一个字符串作为参数，该字符串的一般形式为：`[0/1],[0/1],[0/1],[0/1]`，4个以逗号分隔的值分别对应`CornerRadius`的`Left`、`Top`，`Right`，`Bottom`属性，当值为1则返回对应属性的值，否则返回0。

# Double2GridLengthConverter

该转换器会将`double`值转换为`GridLength`。

# DoubleMinConverter（内部使用）

可以向该转换器提供一个字符串作为最小值（如果不提供，则最小值默认为0），当传入的值小于最小值时，返回最小值，否则返回传入的值。

# Int2StrConverter

该转换器需要提供一个字符串作为参数，该字符串的一般形式为：`[字符串0];[字符串1];...[字符串n]`，转换器会将一个整型的值作为索引，来获取指定位置的字符串并返回。

# Long2FileSizeConverter

该转换器会将长整型转换为文件大小的字符串。

# Number2PercentageConverter

该转换器接受一个长度为2的数字数组，返回两数的百分比值。

# Object2BooleanConverter

如果该转换器获取到的值为`null`则返回`false`，否则返回`true`。

# Object2VisibilityConverter

如果该转换器获取到的值为`null`则返回`Collapsed`，否则返回`Visible`。

# RectangleCircularConverter

该转换器为Rectangle专用，接受`Rectangle`的长和宽合成的一个整型数组，返回它们中最小值的一半，作为圆形Rectangle的半径。

# String2VisibilityConverter

该转换器接受一个字符串，如果该字符串为空或者为null，则返回`Collapse`，否则返回`Visible`。

# String2VisibilityReConverter

该转换器情况与`String2VisibilityConverter`相反。

# ThicknessSplitConverter

该转换器需要提供一个字符串作为参数，该字符串的一般形式为：`[0/1],[0/1],[0/1],[0/1]`，4个以逗号分隔的值分别对应`Thickness`的`Left`、`Top`，`Right`，`Bottom`属性，当值为1则返回对应属性的值，否则返回0。

# TreeViewItemMarginConverter（内部使用）

该转换器用于为TreeViewItem提供适当的左边距。

{% note info no-icon %}
用例：

{% code %}
Visibility="{Binding ShowButton,Converter={StaticResource Boolean2VisibilityConverter}}"
{% endcode %}

{% endnote %}
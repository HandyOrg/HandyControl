---
title: Image图片
---

在`HandyControl`中，对应原生的`Image`控件，仅仅提供了一个默认的控件样式，并未提供特殊样式，对于个性化定制，需要使用者自行定制。

{% note info no-icon %}
用例：
{% code %}
    <StackPanel Background="LightGray">
        <Image Source="Resources/Images/Image_basestyle.png" Margin="0,10"></Image>
        <Image Source="Resources/Images/Image_basestyle.png" RenderOptions.BitmapScalingMode="HighQuality" Stretch="Uniform"></Image>
    </StackPanel>
{% endcode %}

![image.baseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/image.baseStyle.png)

{% endnote %}
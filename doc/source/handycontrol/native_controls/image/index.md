---
title: Image图片
---

在`HandyControl`中，对应原生的`Image`控件，仅仅提供了一个默认的控件样式，并未提供特殊样式，对于个性化定制，需要使用者自行定制

默认样式定义：

```
    <Style TargetType="Image">
    	<!--设定图片的长宽分布为均匀分布-->
        <Setter Property="Stretch" Value="Uniform" />
        <!--设置图片缩放模式为高质量位图缩放-->
        <Setter Property="RenderOptions.BitmapScalingMode" Value="HighQuality" />
    </Style>
```

案例：

```
    <StackPanel Background="LightGray">
        <Image Source="Resources/Images/Image_basestyle.png" Margin="0,10"></Image>
        <Image Source="Resources/Images/Image_basestyle.png" RenderOptions.BitmapScalingMode="HighQuality" Stretch="Uniform"></Image>
    </StackPanel>
```

![image.baseStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/image.baseStyle.png)


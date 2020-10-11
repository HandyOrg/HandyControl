---
title: Effect 特效
---

# BlendEffectBox 特效混合器

通过特效混合器可以很方便得把多种特效应用在同一个控件上：

```xml
<hc:BlendEffectBox>
    <hc:BlendEffectBox.Effects>
        <BlurEffect Radius="30"/>
        <hc:BrightnessEffect Brightness="1"/>
        <hc:ContrastEffect Contrast="20"/>
    </hc:BlendEffectBox.Effects>
    // 你的控件
</hc:BlendEffectBox>
```

# BrightnessEffect 明度特效

```xml
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:BrightnessEffect Brightness="1"/>
    </Image.Effect>
</Image>
```

# ColorComplementEffect 补色特效

```xml
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:ColorComplementEffect/>
    </Image.Effect>
</Image>
```

# ColorMatrixEffect 颜色矩阵特效

```xml
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:ColorMatrixEffect M11="-1" M22="-1" M33="-1" M41="1" M42="1" M43="1" M44="1" M51="1"/>
    </Image.Effect>
</Image>
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:ColorMatrixEffect M11="1.5" M21="1.5" M31="1.5" M12="1.5" M22="1.5" M32="1.5" M13="1.5" M23="1.5" M33="1.5" M51="-1" M52="-1" M53="-1"/>
    </Image.Effect>
</Image>
```

# ContrastEffect 对比度特效

```xml
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:ContrastEffect Contrast="20"/>
    </Image.Effect>
</Image>
```

# GrayScaleEffect 灰度特效

```xml
<Image Width="120" Height="120" Source="/HandyControlDemo;component/Resources/Img/Album/10.jpg">
    <Image.Effect>
        <hc:GrayScaleEffect Scale=".7"/>
    </Image.Effect>
</Image>
```
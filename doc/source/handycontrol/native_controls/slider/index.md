---
title: Slider 滑块
---

# SliderBaseStyle

滑块基础样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

```xml
<UniformGrid.Resources>
    <Style BasedOn="{StaticResource SliderBaseStyle}" TargetType="Slider"/>
</UniformGrid.Resources>
<Slider Margin="10,30"></Slider>
```

效果：

![Slider.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/Slider.DefaultStyle.png)
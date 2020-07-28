---
title: CompareSlider 对比滑块
---

借助对比滑块可以很方便得看出元素改动前后的差异。

```cs
public class CompareSlider : Slider
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|TargetContent|目标内容|||
|SourceContent|源内容（当前内容）||||

# 案例

```xml
<WrapPanel ItemHeight="384" ItemWidth="632">
    <hc:CompareSlider Value="5" Width="600" Height="352">
        <hc:CompareSlider.TargetContent>
            <Border>
                <Border.Background>
                    <ImageBrush ImageSource="/HandyControlDemo;component/Resources/Img/b1.jpg"/>
                </Border.Background>
            </Border>
        </hc:CompareSlider.TargetContent>
        <hc:CompareSlider.SourceContent>
            <Border>
                <Border.Background>
                    <ImageBrush ImageSource="/HandyControlDemo;component/Resources/Img/b2.jpg"/>
                </Border.Background>
            </Border>
        </hc:CompareSlider.SourceContent>
    </hc:CompareSlider>
    <hc:CompareSlider Orientation="Vertical" Value="5" Width="600" Height="352">
        <hc:CompareSlider.TargetContent>
            <Border>
                <Border.Background>
                    <ImageBrush ImageSource="/HandyControlDemo;component/Resources/Img/b1.jpg"/>
                </Border.Background>
            </Border>
        </hc:CompareSlider.TargetContent>
        <hc:CompareSlider.SourceContent>
            <Border>
                <Border.Background>
                    <ImageBrush ImageSource="/HandyControlDemo;component/Resources/Img/b2.jpg"/>
                </Border.Background>
            </Border>
        </hc:CompareSlider.SourceContent>
    </hc:CompareSlider>
</WrapPanel>
```

![CompareSlider](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CompareSlider-v.gif)

![CompareSlider](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/CompareSlider-h.gif)
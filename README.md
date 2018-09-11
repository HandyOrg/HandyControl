![logo](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/icon.png)

![csharp-version](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/csharp-version.png) ![IDE-version](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/IDE-version.png) [![nuget-version](https://img.shields.io/nuget/v/HandyControl.svg)](https://www.nuget.org/packages/HandyControl)  [![build-status](https://ci.appveyor.com/api/projects/status/github/NaBian/handycontrol?svg=true)](https://ci.appveyor.com/project/NaBian/handycontrol)

# 欢迎使用 HandyControl

> HandyControl包含了我在开发过程中觉得wpf原生库中所欠缺的东西，现在所有的控件均已编码完毕，我会抽空将最为常用的部分一一开源，因个人能力和时间精力的原因，控件库中难免会留有bug，如果你发现了它们请给我提issue，谢谢。

让我们开门见山，HandyControl已经开源的控件截图有：

### 1、颜色拾取器ColorPicker

![颜色拾取器ColorPicker](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ColorPicker.gif)

### 2、加载条Loading

![加载条Loading](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Loading.gif)

### 3、轮播Carousel

![轮播Carousel](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Carousel.gif)

### 4、页码条Pagination

![轮播Pagination](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Pagination.gif)

### 5、展开折叠框Expander

![展开折叠框Expander](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Expander.gif)

### 6、时间条TimeBar

![时间条TimeBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TimeBar.gif)

### 7、图片浏览器ImageBrowser

![图片浏览器ImageBrowser](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ImageBrowser.gif)

### 8、对比滑块CompareSlider

![对比滑块CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-h.gif)

![对比滑块CompareSlider](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/CompareSlider-v.gif)

### 9、信息通知Growl

![信息通知Growl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/Growl.gif)

### 10、动画路径AnimationPath

![动画路径AnimationPath](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/AnimationPath.gif)

### 11、进度条ProgressBar

![进度条ProgressBar](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/ProgressBar.gif)

### 12、选项卡控件TabControl

![选项卡控件TabControl](https://raw.githubusercontent.com/NaBian/HandyControl/master/Resources/TabControl.gif)

# 使用方法

第一步：添加HandyControl的引用；  
第二步：在App.xaml中按照以下方式添加代码：  
```XML
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/ThemesDefault.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```
第三步：enjoy coding

# FAQ
* 如何让**Scrollviewer**具有惯性？只要：  
```XML
<controls:ScrollViewer IsEnableInertia="True">
    <!--你的控件-->
</controls:ScrollViewer>
```

* 如何让**Scrollviewer**具有穿透效果？只要：  
```XML
<controls:ScrollViewer IsPenetrating="True">
    <!--你的控件-->
</controls:ScrollViewer>
```

* 为什么我连编译都无法通过：  
编译之前，请确保IDE为vs2017且c#最低版本为7.0,HandyControl目前仅支持4.5以上的.net环境。

# v1.3.0版本计划

1、添加时间、日历控件；  
2、添加步骤条控件；
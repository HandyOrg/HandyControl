---
title: 快速开始
---

# 第一步
下载源码或者以Nuget的方式引用控件库。
源码链接：[HandyControl-Github](https://github.com/HandyOrg/HandyControl)
Nuget链接：[HandyControl-Nuget](https://www.nuget.org/packages/HandyControl/)

# 第二步
在App.xaml中添加以下代码：

``` xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/Theme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

# 第三步
添加命名空间：`xmlns:hc="https://handyorg.github.io/handycontrol"`
---
title: ScrollViewer 滚动视图
---

# ScrollViewerNativeBaseStyle

原生滚动视图默认样式，不推荐直接使用，应该始终被其它样式以BasedOn的方式使用。

案例：

```xml
<ScrollViewer>
   <Grid Height="500">
      <TextBlock Text="内容区域"></TextBlock>
   </Grid>
</ScrollViewer>
```

效果：

![ScrollViewer.DefaultStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ScrollViewer.DefaultStyle.png)

# ScrollViewerUpDown

显示上下按钮的滚动视图样式

案例：

```xml
<ScrollViewer Style="{StaticResource ScrollViewerUpDown}">
    <StackPanel Height="500">
        <TextBlock Text="内容区域" Height="250"></TextBlock>
        <TextBlock Text="内容区域" Height="250"></TextBlock>
        <TextBlock Text="内容区域"></TextBlock>
        <TextBlock Text="内容区域"></TextBlock>
    </StackPanel>
</ScrollViewer>
```

效果：

![ScrollViewer.UpDownStyle](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/native_controls/ScrollViewer.UpDownStyle.png)
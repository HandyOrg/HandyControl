---
title: ImageBrowser 图片浏览器
---

可借助 `ImageBrowser` 简单地浏览单张图片.

```cs
public class ImageBrowser : Window
```

# 案例

构造函数传入图片地址后即可开始浏览图片:

```cs
new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show()
```

![ImageBrowser](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ImageBrowser.gif)

# 功能

功能面板截图如下：

![ImageBrowser](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Doc/extend_controls/ImageBrowser_1.png)

从左到右功能依次为：`保存至本地`、`使用系统默认程序打开该图片`、`缩小图片`、`放大图片`、`原始大小`、`左转图片`、`右转图片`.
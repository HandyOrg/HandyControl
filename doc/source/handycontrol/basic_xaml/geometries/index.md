---
title: 几何形状
---

HandyControl中自带了一些几何形状定义，但要应用于生产中显然不够。我们不准备包罗万象，这永远没有底，所以我们就反其道而行，只包含了控件库自己要用的（当然用户也可使用），其余请自行扩展。下表中列出了控件库中所有的形状定义：

| 名称 | 描述 |
|-|-|
| CalendarGeometry | 日历 |
| DeleteGeometry | 删除 |
| DeleteFillCircleGeometry | 删除（圆形填充） |
| CloseGeometry | 关闭 |
| DownGeometry | 下 |
| UpGeometry | 上 |
| ClockGeometry | 时钟 |
| LeftGeometry | 左 |
| RightGeometry | 右 |
| RotateLeftGeometry | 向左旋转 |
| RotateRightGeometry | 向左旋转 |
| EnlargeGeometry | 放大 |
| ReduceGeometry | 缩小 |
| DownloadGeometry | 下载 |
| SaveGeometry | 保存 |
| WindowsGeometry | 窗口 |
| FullScreenGeometry | 全屏 |
| FullScreenReturnGeometry | 全屏返回 |
| SearchGeometry | 搜索 |
| UpDownGeometry | 上和下的组合 |
| WindowMinGeometry | 窗口最小化 |
| WindowRestoreGeometry | 窗口还原 |
| WindowMaxGeometry | 窗口最大化 |
| CheckedGeometry | 选中 |
| PageModeGeometry | 单页模式 |
| TwoPageModeGeometry | 双页模式 |
| ScrollModeGeometry | 滚动模式 |
| EyeOpenGeometry | 睁眼 |
| EyeCloseGeometry | 闭眼 |
| AudioGeometry | 声音 |
| BubbleTailGeometry | 气泡的尾巴 |
| StarGeometry | 爱心 |
| AddGeometry | 加 |
| SubGeometry | 减 |
| WarningGeometry | 警告 |
| InfoGeometry | 信息 |
| ErrorGeometry | 错误 |
| SuccessGeometry | 成功 |
| FatalGeometry | 严重 |
| AskGeometry | 询问 |
| AllGeometry | 所有 |
| DragGeometry | 拖拽（用于工具条） |

{% note info no-icon %}
用例：`Data="{StaticResource DragGeometry}"`
{% endnote %}
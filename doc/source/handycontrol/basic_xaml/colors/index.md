---
title: 颜色
---

内置的颜色xaml定义有3个，对它们的描述如下表所示：

| 名称 | 用途 |
|-|-|
| Colors.xaml | 默认配置 |
| ColorsDark.xaml | 暗色配置 |
| ColorsViolet.xaml | 紫色配置 |

每个文件中定义了相同的颜色名称，只是颜色值不同，对颜色名称的描述如下表所示：

| 名称 | 用途 |
|-|-|
| PrimaryColor | 主色调 |
| DarkPrimaryColor | 主色调（深色） |
| DangerColor | 错误、危险 |
| DarkDangerColor | 错误、危险（深色） |
| WarningColor | 警告 |
| DarkWarningColor | 警告（深色） |
| InfoColor | 信息 |
| DarkInfoColor | 信息（深色） |
| SuccessColor | 成功 |
| DarkSuccessColor | 成功（深色） |
| PrimaryTextColor | 主文本 |
| SecondaryTextColor | 次级文本 |
| ThirdlyTextColor | 末级文本 |
| ReverseTextColor | 反色文本 |
| TextIconColor | 一般用于深色背景下的文字或图标 |
| BorderColor | 边框 |
| SecondaryBorderColor | 次级边框 |
| BackgroundColor | 主背景色 |
| RegionColor | 区域块背景 |
| SecondaryRegionColor | 次级区域块背景 |
| ThirdlyRegionColor | 末级区域块背景 |
| TitleColor | 标题背景 |
| SecondaryTitleColor | 次级标题背景 |
| DefaultColor | 默认颜色 |
| DarkDefaultColor | 次级默认颜色 |
| AccentColor | 提醒 |
| DarkAccentColor | 提醒（深色） |
| DarkMaskColor | 作为遮罩使用 |
| DarkOpacityColor | 半透明背景 |
| BlurGradientValue | 仅用于模糊窗口的背景 |

{% note info no-icon %}
用例：`Color="{DynamicResource PrimaryColor}"`
{% endnote %}
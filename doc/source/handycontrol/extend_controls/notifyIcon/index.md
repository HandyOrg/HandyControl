---
title: NotifyIcon 托盘图标
---

系统托盘图标的wpf实现方式.

```cs
public class NotifyIcon : FrameworkElement, IDisposable
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Token|用于设置消息标记||用于在指定的托盘图标上显示气泡提示|
|Text|鼠标提示文本|||
|Icon|图标|||
|ContextContent|右键弹出内容|||
|BlinkInterval|闪烁间隔|500ms||
|IsBlink|是否闪烁|false|||

# 方法

|名称|说明|
|-|-|
| Init() | 初始化 |
| Register(string, NotifyIcon) | 为指定的托盘图标注册消息标记 |
| Unregister(string, NotifyIcon) | 为指定的托盘图标取消消息标记的注册 |
| Unregister(NotifyIcon) | 如果该托盘图标注册了消息标记则取消注册 |
| Unregister(string) | 如果该消息标记有对应的托盘图标则取消注册 |
| ShowBalloonTip(string, string, NotifyIconInfoType, string) | 显示气泡提示 |
| ShowBalloonTip(string, string, NotifyIconInfoType) | 显示气泡提示 |
| CloseContextControl() | 关闭上下文控件 |

# 事件

|名称|说明|
|-|-|
| Click | 单击时触发 |
| MouseDoubleClick | 双击时触发 |

# 案例

```xml
<hc:NotifyIcon Text="HandyControl" IsBlink="true" Visibility="Visible" Icon="/HandyControlDemo;component/Resources/Img/icon-white.ico"/>
```

![NotifyIcon](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/NotifyIcon.gif)
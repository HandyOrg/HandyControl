---
title: MessageBox 消息对话框
---

HC重写了一套消息对话框，使用方式和原生一致.

```cs
public sealed class MessageBox : Window
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Message|消息内容|||
|Image|消息类型示意图|||
|ImageBrush|消息类型示意图颜色|||
|ShowImage|是否显示消息类型示意图|false|||

# 方法

|名称|说明|
|-|-|
| Success(string, string) | 显示一则成功消息 |
| Info(string, string) | 显示一则通知消息 |
| Warning(string, string) | 显示一则警告消息 |
| Error(string, string) | 显示一则错误消息 |
| Fatal(string, string) | 显示一则严重消息 |
| Ask(string, string) | 显示一则询问消息 |
| Show(MessageBoxInfo) | 显示一则自定义消息 |
| Show(string, string, MessageBoxButton, MessageBoxImage, MessageBoxResult) | 显示一则消息 |
| Show(Window, string, string, MessageBoxButton, MessageBoxImage, MessageBoxResult) | 显示一则消息 |

# 案例

```cs
MessageBox.Show(Properties.Langs.Lang.GrowlAsk, Properties.Langs.Lang.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
```

![MessageBox](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/MessageBox.png)
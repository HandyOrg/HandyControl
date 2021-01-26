---
title: Shield 徽章
---

徽章由左半部分的名称和右半部分的值组成，点击徽章可以打开对应的链接.

```cs
[ContentProperty(nameof(Status))]
public class Shield : ButtonBase
```

# 属性

|属性|描述|默认值|备注|
|-|-|-|-|
|Subject|名称|||
|Status|值|||
|Color|颜色||||

# 案例

```xml
<StackPanel Orientation="Horizontal" Margin="32">
    <hc:Shield Subject=".net" Status=">=4.0" Color="#1182c3"/>
    <hc:Shield Subject="c#" Status="7.0" Margin="4,0,0,0" Color="#1182c3"/>
    <hc:Shield Subject="IDE" Status="2017" Margin="4,0,0,0" Color="#1182c3"/>
    <hc:Shield Subject="chat" Status="on gitter" Margin="4,0,0,0" Color="#4eb899" Command="hc:ControlCommands.OpenLink" CommandParameter="https://gitter.im/HandyControl/Lobby?utm_source=badge&amp;utm_medium=badge&amp;utm_campaign=pr-badge&amp;utm_content=badge"/>
    <hc:Shield Subject="qq" Status="714704041" Margin="4,0,0,0" Color="#d8624c" Command="hc:ControlCommands.OpenLink" CommandParameter="http://shang.qq.com/wpa/qunwpa?idkey=a571e5553c9d41e49c4f22f3a8b2865451497a795ff281fedf3285def247efc1"/>
</StackPanel>
```

![Shield](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Shield.png)
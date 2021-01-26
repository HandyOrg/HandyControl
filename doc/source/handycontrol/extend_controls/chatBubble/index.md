---
title: ChatBubble 对话气泡
---

对话气泡常出现于通讯类软件中，相对于纯文本形式的上下文对话，使用对话气泡可以让聊天界面更加生动有趣。通过扩展，还可以做出气泡皮肤，可使软件个性化功能更加丰富。

```cs
public class ChatBubble : SelectableItem
```

# 属性

| 属性                   | 用途                |
| ---------------------- | -------------------|
| Role                   | 对话角色            |
| Type                   | 消息类型            |
| IsRead                 | 是否已读            |
| ReadAction             | 该气泡被选中时触发   |

![ChatBubble](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/ChatBubble.png)

{% note warning %}
效果代码详见源码demo
{% endnote %}
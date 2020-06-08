---
title: Growl 信息通知
---

当需要在窗口的右侧区域，以从上到下排列通知信息时可以使用本控件。

# 控件效果

![Growl](https://raw.githubusercontent.com/HandyOrg/HandyOrgResource/master/HandyControl/Resources/Growl.gif)

# 准备工作

在指定的窗口右侧区域添加`StackPanel`容器
``` xml
<ScrollViewer VerticalScrollBarVisibility="Hidden" HorizontalAlignment="Right">
    <StackPanel VerticalAlignment="Top" Margin="0,10,10,10"/>
</ScrollViewer>
```
为了方便滚动，一般会在外层包一个`ScrollViewer`，同时为了美观，我们设置`ScrollViewer`的`VerticalScrollBarVisibility="Hidden"`，最好给`StackPanel`添加一个合适的`Margin`，也是为了美观。

# 基础用法

第一步：将`StackPanel`设置为`Growl`的容器

``` xml
<ScrollViewer VerticalScrollBarVisibility="Hidden" HorizontalAlignment="Right">
    <StackPanel controls:Growl.GrowlParent="True" VerticalAlignment="Top" Margin="0,10,10,10"/>
</ScrollViewer>
```
{% note warning %}
注意代码中的`controls:Growl.GrowlParent="True"`。
{% endnote %}

第二步：调用`Growl`的相关接口来显示指定的消息

{% note info no-icon %}
用例：`Growl.Success("文件保存成功！");`
{% endnote %}

# 只在激活中的窗口显示信息通知

为需要显示信息通知的窗口重写`OnActivated`和`OnDeactivated`方法，在`OnActivated`中调用`Growl.SetGrowlParent(GrowlPanel, true);`，在`OnDeactivated`中调用`Growl.SetGrowlParent(GrowlPanel, false);`

{% note warning %}
其中`GrowlPanel`是信息通知容器，类型推荐是`StackPanel`
{% endnote %}

{% note warning %}
这里需要说明一下，一个窗口对应一个`StackPanel`容器，Growl内部会维护一个变量用以存储这个容器，在每次通知触发时，会向这个容器中插入Growl的实例，也就是说，每次调用`Growl.SetGrowlParent(GrowlPanel, true);`时，`GrowlPanel`会成为内部的那个变量。
{% endnote %}

{% note warning %}
这种情况下就不需要在xaml中设置`controls:Growl.GrowlParent="True"`了。
{% endnote %}

# 在任意的`StackPanel`容器中显示信息通知

第一步：为`StackPanel`容器赋予消息标记：SuccessMsg
``` xml
<ScrollViewer VerticalScrollBarVisibility="Hidden" HorizontalAlignment="Right">
    <StackPanel controls:Growl.Token="SuccessMsg" VerticalAlignment="Top" Margin="0,10,10,10"/>
</ScrollViewer>
```
{% note warning %}
注意代码中的`controls:Growl.Token="SuccessMsg"`。
{% endnote %}

第二步：使用消息标记：SuccessMsg，并调用`Growl`的相关接口来显示指定的消息

{% note info no-icon %}
用例：`Growl.Success("文件保存成功！", "SuccessMsg");`
{% endnote %}

{% note warning %}
现在"文件保存成功！"这则消息只会发送到拥有"SuccessMsg"消息标记的`StackPanel`容器中。
{% endnote %}

# 属性

|名称|说明|
|-|-|
| GrowlPanel | 当前使用的消息容器 |

# 附加属性

|名称|说明|
|-|-|
| Token | 用于设置消息标记 |
| GrowlParent | 用于设置消息容器 |

# 方法

|名称|说明|
|-|-|
| Success(string) | 显示一则成功通知 |
| Success(string, string) | 使用指定的消息标记显示一则成功通知 |
| Success(GrowlInfo) | 使用完整的消息初始化模型显示一则成功通知 |
| SuccessGlobal(string) | 显示一则全局成功通知 |
| SuccessGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局成功通知 |
| Info(string) | 显示一则消息通知 |
| Info(string, string) | 使用指定的消息标记显示一则消息通知 |
| Info(GrowlInfo) | 使用完整的消息初始化模型显示一则消息通知 |
| InfoGlobal(string) | 显示一则全局消息通知 |
| InfoGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局消息通知 |
| Warning(string) | 显示一则警告通知 |
| Warning(string, string) | 使用指定的消息标记显示一则警告通知 |
| Warning(GrowlInfo) | 使用完整的消息初始化模型显示一则警告通知 |
| WarningGlobal(string) | 显示一则全局警告通知 |
| WarningGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局警告通知 |
| Error(string) | 显示一则错误通知 |
| Error(string, string) | 使用指定的消息标记显示一则错误通知 |
| Error(GrowlInfo) | 使用完整的消息初始化模型显示一则错误通知 |
| ErrorGlobal(string) | 显示一则全局错误通知 |
| ErrorGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局错误通知 |
| Fatal(string) | 显示一则严重通知 |
| Fatal(string, string) | 使用指定的消息标记显示一则严重通知 |
| Fatal(GrowlInfo) | 使用完整的消息初始化模型显示一则严重通知 |
| FatalGlobal(string) | 显示一则全局严重通知 |
| FatalGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局严重通知 |
| Ask(string, Func<bool, bool>) | 显示一则询问通知 |
| Ask(string, Func<bool, bool>, string) | 使用指定的消息标记显示一则询问通知 |
| Ask(GrowlInfo) | 使用完整的消息初始化模型显示一则询问通知 |
| AskGlobal(string, Func<bool, bool>) | 显示一则全局询问通知 |
| AskGlobal(GrowlInfo) | 使用完整的消息初始化模型显示一则全局询问通知 |
| Register(string, Panel) | 为指定的容器注册消息标记 |
| Unregister(string, Panel) | 为指定的容器取消消息标记的注册 |
| Unregister(Panel) | 如果该容器注册了消息标记则取消注册 |
| Unregister(string) | 如果该消息标记有对应的容器则取消注册 |
| Clear( ) | 清空当前使用的消息容器中的消息 |
| Clear(string) | 清空含有指定消息标记的消息容器中的消息 |
| Clear(Panel) | 清空指定容器中的消息 |
| ClearGlobal( ) | 清空全局容器中的消息 |

# 注意

`Error` 和 `Fatal`模式不会自动关闭。

# FAQ

{% note info no-icon %}
Ask比较特殊，咋用？
{% endnote %}

Ask方法主要的焦点为第二个参数，它的类型是`Func<bool, bool>`，我们先看示例代码：
``` csharp
Growl.Ask(Properties.Langs.Lang.GrowlAsk, isConfirmed =>
{
    Growl.Info(isConfirmed.ToString());
    return true;
});
```
使用Ask方法默认会显示“确认”和“取消”按钮，当点击“确认”时，上述代码中的`isConfirmed`的值为`true`，点击“取消”则为`false`，可以根据`isConfirmed`的值来采取相应操作，最后如果返回`true`则会关闭Growl通知，返回`false`则不关闭。